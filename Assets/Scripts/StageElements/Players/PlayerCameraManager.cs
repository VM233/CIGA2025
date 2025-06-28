using Sirenix.OdinInspector;
using Unity.Cinemachine;
using VMFramework.Procedure;

namespace RoomPuzzle
{
    [ManagerCreationProvider(ManagerType.EnvironmentCore)]
    public class PlayerCameraManager : ManagerBehaviour<PlayerCameraManager>
    {
        [Required]
        public CinemachineCamera cinemachineCamera;

        protected override void OnBeforeInitStart()
        {
            base.OnBeforeInitStart();
            
            StageManager.Instance.OnStageChanged += OnStageChanged;
        }

        protected virtual void OnStageChanged()
        {
            cinemachineCamera.Follow = StageManager.Instance.CurrentPlayer.transform;
        }
    }
}