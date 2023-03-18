using System;
using UnityEngine.InputSystem;

namespace GameInput
{
    public class ButtonInput
    {
        bool lastPressed;
        bool isPressed;
        private readonly String name;

        public ButtonInput(String name)
        {
            this.name = name;
        }

        public void Refresh(InputAction.CallbackContext value)
        {
            if (value.action.name == name)
            {
                lastPressed = isPressed;
                isPressed = value.ReadValueAsButton();
            }
        }

        public bool IsJustDown()
        {
            return !lastPressed && isPressed;
        }

        public bool IsJustUp()
        {
            return lastPressed && !isPressed;
        }
    }
}