using DefaultNamespace.Utility;
using UnityEngine.InputSystem;

namespace DefaultNamespace
{
    using NUnit.Framework.Constraints;
    using UnityEngine;

    public class GameInput : Monobehaviour_Singleton<GameInput>
    {
    
        private InputActions _inputActions;
        [SerializeField] private bool InputActions_enabled = false;
       
        private void Awake()
        {
            base.Awake();
            _inputActions =  new InputActions();
        
            _inputActions.Enable();
            InputActions_enabled =  true;
     
        }

        private void OnDestroy()
        {
            InputActions_enabled = false;
            _inputActions.Disable();
        
        }

        public bool IsUpActionPressed()
        {
            return _inputActions.Player.MoveForward.IsPressed();
        }
        public bool IsLeftActionPressed()
        {
            return _inputActions.Player.TurnLeft.IsPressed();
        }
        public bool IsRightActionPressed()
        {
            return _inputActions.Player.TurnRight.IsPressed();
        }

        public bool IsWeaponDischargeActionPressed()
        {
            return _inputActions.Player.Fire.IsPressed();
        }


        public bool IsPausedActionPressed()
        {
            
            return _inputActions.Player.Pause.IsPressed();
        }
        
        public bool IsWarpActionPressed()
        {
            return _inputActions.Player.Warp.IsPressed();
        }

        public string GetFireKeyName()
        {

            return _inputActions.Player.Fire.GetBindingDisplayString(0);
            
        }

    }

}