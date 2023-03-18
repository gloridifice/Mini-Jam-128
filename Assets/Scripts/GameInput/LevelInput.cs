using System;
using GameInput;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

namespace GameInput
{
    public class LevelInput : MonoBehaviour
    {
        private Vector2 move = Vector2.zero;
        public Vector2 Move
        {
            get => move;
            private set => move = value;
        }
        private PlayerInput playerInput;
        public PlayerInput PlayerInput => this.LazyGetComponent(playerInput);
        
        public readonly ButtonInput 
            redTagButton = new("RedTag"),
            yellowTagButton = new("YellowTag"),
            greenTagButton = new("GreenTag"),
            blackTagButton = new("BlackTag");

        private void OnEnable()
        {
            PlayerInput.onActionTriggered += OnActionTriggered;
        }

        void OnActionTriggered(InputAction.CallbackContext value)
        {
            print(value.action.name);
            if (value.action.name == "Move")
            {
                Move = value.ReadValue<Vector2>();
            }
        }

        private void OnDisable()
        {
            PlayerInput.onActionTriggered -= OnActionTriggered;
        }
    }
}