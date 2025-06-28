using UnityEngine.InputSystem;
using VMFramework.Core;

namespace VMFramework.GameEvents
{
    public class InputSystemGameEvent : ParameterizedGameEvent<InputAction.CallbackContext>
    {
        public InputSystemGameEventConfig InputSystemGameEventConfig => (InputSystemGameEventConfig)GamePrefab;
        
        public InputAction InputAction => InputSystemGameEventConfig.InputAction;

        protected override void OnGet()
        {
            base.OnGet();

            InputAction.performed += OnPerformed;
            InputAction.canceled += OnCanceled;
        }

        protected override void OnReturn()
        {
            base.OnReturn();

            InputAction.performed -= OnPerformed;
            InputAction.canceled -= OnCanceled;
        }

        protected virtual void OnPerformed(InputAction.CallbackContext context)
        {
            if (disableToken.IsEnabled == false)
            {
                return;
            }

            Propagate(context);

            if (IsDebugging)
            {
                Debugger.LogWarning(context);
            }
        }
        
        protected virtual void OnCanceled(InputAction.CallbackContext context)
        {
            if (disableToken.IsEnabled == false)
            {
                return;
            }
            
            Propagate(context, propagateAction: false);
            
            if (IsDebugging)
            {
                Debugger.LogWarning(context);
            }
        }
    }
}