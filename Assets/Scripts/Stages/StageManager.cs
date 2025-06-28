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
        [Required]
        public PlayerController player;
        
        [IsNotNullOrEmpty]
        public List<StageCore> stages = new();
        
        public int initialStageIndex = 0;

        [GamePrefabID(typeof(IUIPanelConfig))]
        [IsNotNullOrEmpty]
        public string switchUI;
        
        public StageCore CurrentStage { get; protected set; }
        
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
            
            var stage = stageLookup[stageIndex];
            CurrentStage = stage;
            
            bool hasOldStage = oldStage != null;

            if (hasOldStage)
            {
                oldStage.RemoveElement(player.StageElement);
            }
            
            stage.AddElement(stage.startPosition, player.StageElement);

            if (hasOldStage && stage.requireSwitch)
            {
                UIPanelManager.GetAndOpenUniquePanel(switchUI);
            }
        }
    }
}