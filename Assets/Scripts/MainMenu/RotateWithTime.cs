using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MainMenu
{
    public class RotateWithTime : MonoBehaviour
    {
        public Vector3 axis = Vector3.up;
        public float speed = 1f;
        public bool random;

        private void Start()
        {
            if (random)
            {
                axis = new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            } 
        }

        private void FixedUpdate()
        {
                transform.Rotate(axis, speed * 0.1f);
        }
    }
}