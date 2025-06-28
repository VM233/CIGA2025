using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using VMFramework.OdinExtensions;
using VMFramework.Procedure;
using VMFramework.UI;

namespace RoomPuzzle
{
    [ManagerCreationProvider("Stage")]
    public class StageManager : ManagerBehaviour<StageManager>
    {
        [IsNotNullOrEmpty]
        public List<StageCore> stages = new();

        public int initialStageIndex = 0;

        [GamePrefabID(typeof(IUIPanelConfig))]
        [IsNotNullOrEmpty]
        public string switchUI;

        [ShowInInspector]
        public PlayerController CurrentPlayer { get; protected set; }

        [ShowInInspector]
        public StageCore CurrentStage { get; protected set; }

        public event Action OnStageChanged;

        protected readonly Dictionary<int, StageCore> stageLookup = new();

        protected override void OnBeforeInitStart()
        {
            base.OnBeforeInitStart();

            ProcedureManager.Instance.OnEnterProcedureEvent += OnEnterProcedure;
        }

        protected virtual void OnEnterProcedure(string procedureID)
        {
            if (procedureID == ClientRunningProcedure.ID)
            {
                stageLookup.Clear();

                foreach (var stage in stages)
                {
                    stageLookup.Add(stage.stageIndex, stage);
                }

                LoadStage(initialStageIndex);
            }
        }

        [Button]
        public void LoadStage(int stageIndex)
        {
            var oldStage = CurrentStage;
            var oldPlayer = CurrentPlayer;

            var stage = stageLookup[stageIndex];
            CurrentStage = stage;

            bool hasOldStage = oldStage != null;

            if (hasOldStage && oldPlayer.StageElement != null)
            {
                oldStage.RemoveElement(oldPlayer.StageElement);
                Destroy(oldPlayer.gameObject);
            }

            CurrentPlayer = Instantiate(CurrentStage.player);

            stage.AddElement(stage.startPosition, CurrentPlayer.StageElement);

            OnStageChanged?.Invoke();
            
            if (hasOldStage && stage.requireSwitch)
            {
                UIPanelManager.GetAndOpenUniquePanel(switchUI);
            }
        }
    }
}