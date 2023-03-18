using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelInput: MonoBehaviour
{
    private Vector2 move = Vector2.zero;
    public Vector2 Move
    {
        get => move;
        private set => move = value;
    }

    void OnMove(InputValue value)
    {
        Move = value.Get<Vector2>();
    }
}