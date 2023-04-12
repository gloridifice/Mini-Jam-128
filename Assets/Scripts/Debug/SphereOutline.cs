using System;
using UnityEngine;

namespace Game.Debug
{
#if UNITY_EDITOR

    public class SphereOutline : MonoBehaviour
    {
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, 2f);
        }
    }

#endif
}