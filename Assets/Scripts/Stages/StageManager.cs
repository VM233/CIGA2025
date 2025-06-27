using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using VMFramework.OdinExtensions;

namespace RoomPuzzle
{
    public class StageManager : UniqueMonoBehaviour<StageManager>
    {
        [Required]
        public PlayerController player;
        
        [IsNotNullOrEmpty]
        public List<StageCore> stages = new();
        
        public int initialStageIndex = 0;
        
        public StageCore CurrentStage { get; protected set; }
        
        protected readonly Dictionary<int, StageCore> stageLookup = new();

        private void Start()
        {
            StartGame();
        }

        public void StartGame()
        {
            stageLookup.Clear();

            foreach (var stage in stages)
            {
                stageLookup.Add(stage.stageIndex, stage);
            }
            
            LoadStage(initialStageIndex);
        }

        public void LoadStage(int stageIndex)
        {
            var oldStage = CurrentStage;
            
            var stage = stageLookup[stageIndex];
            CurrentStage = stage;

            if (oldStage != null)
            {
                oldStage.RemoveElement(player.StageElement);
            }
            
            stage.AddElement(stage.startPosition, player.StageElement);
        }
    }
}