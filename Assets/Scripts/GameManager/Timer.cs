using UnityEngine;

namespace GameManager
{
    public class Timer : MonoBehaviour
    {
        private float time;
        /// <summary>
        /// time passed in seconds
        /// </summary>
        public float Tick { get => time; }
        public int IntTick { get => (int)time; }

        private void FixedUpdate()
        {
            time += Time.fixedDeltaTime;
            // Debug.Log("time: " + Tick);
        }
    }
}
