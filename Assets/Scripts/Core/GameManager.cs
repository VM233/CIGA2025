using Sirenix.OdinInspector;
using VMFramework.Procedure;

namespace RoomPuzzle
{
    [ManagerCreationProvider("Game Core")]
    public class GameManager : ManagerBehaviour<GameManager>
    {
        [Button]
        public void StartGame()
        {
            ProcedureManager.Instance.EnterProcedure(ClientRunningProcedure.ID);
        }
    }
}