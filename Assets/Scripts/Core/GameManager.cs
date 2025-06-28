using Sirenix.OdinInspector;
using VMFramework.Procedure;

namespace RoomPuzzle
{
    [ManagerCreationProvider("Game Core")]
    public class GameManager : ManagerBehaviour<GameManager>
    {
        public bool autoStartGame = true;

        protected override void OnBeforeInitStart()
        {
            base.OnBeforeInitStart();
            
            ProcedureManager.Instance.OnEnterProcedureEvent += OnEnterProcedure;
        }

        protected virtual void OnEnterProcedure(string procedureID)
        {
            if (procedureID == MainMenuProcedure.ID)
            {
                if (autoStartGame)
                {
                    StartGame();
                }
            }
        }

        [Button]
        public void StartGame()
        {
            ProcedureManager.Instance.EnterProcedure(ClientRunningProcedure.ID);
        }
    }
}