using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using VMFramework.OdinExtensions;

namespace RoomPuzzle
{
    public class PlayerController : MonoBehaviour
    {
        [IsNotNullOrEmpty]
        public string moveInputName;

        [MinValue(0)]
        public float moveSpeed = 5f;

        protected InputAction moveAction;
        protected Rigidbody2D rb;

        protected virtual void Awake()
        {
            moveAction = InputSystem.actions.FindAction(moveInputName);
            rb = GetComponent<Rigidbody2D>();
        }

        protected virtual void Start()
        {

        }

        protected virtual void Update()
        {
            var moveValue = moveAction.ReadValue<Vector2>();
            var moveVector = moveValue.normalized * moveSpeed;
            rb.linearVelocity = moveVector;
        }
    }
}
