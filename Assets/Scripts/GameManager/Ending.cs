using System;
using UnityEngine;

namespace GameManager
{
    public class Ending : MonoBehaviour
    {
        private event EventHandler End;

        private void Awake()
        {
            End += (_, _) => { Endings(); };
        }

        private void Endings()
        {
            Debug.Log("end");
        }
    
        //test
        private void Update()
        {
            if (Input.GetKeyDown("q"))
            {
                End?.Invoke(null, EventArgs.Empty);
            }
        }
    }
}
