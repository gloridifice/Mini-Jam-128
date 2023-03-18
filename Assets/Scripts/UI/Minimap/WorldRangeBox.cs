using System;
using UnityEngine;

namespace UI.Minimap
{
    [ExecuteAlways]
    public class WorldRangeBox : MonoBehaviour
    {
        public Vector3 Center => transform.position;
        public float XSize => transform.localScale.x;
        public float ZSize => transform.localScale.z;
        public float Height => transform.localScale.y;

        public Vector2 Zero => new Vector2(Center.x - XSize / 2, Center.z - ZSize / 2);

        private void Update()
        {
            float h1 = Height / 2;
            float h2 = -Height / 2;
            Color color = Color.blue;
            Vector3 vt1 = Center + new Vector3(-XSize / 2, h1, ZSize / 2);
            Vector3 vt2 = Center + new Vector3(XSize / 2, h1, ZSize / 2);
            Vector3 vt3 = Center + new Vector3(XSize / 2, h1, -ZSize / 2);
            Vector3 vt4 = Center + new Vector3(-XSize / 2, h1, -ZSize / 2);

            Vector3 vb1 = Center + new Vector3(-XSize / 2, h2, ZSize / 2);
            Vector3 vb2 = Center + new Vector3(XSize / 2, h2, ZSize / 2);
            Vector3 vb3 = Center + new Vector3(XSize / 2, h2, -ZSize / 2);
            Vector3 vb4 = Center + new Vector3(-XSize / 2, h2, -ZSize / 2);

            Debug.DrawLine(vt1, vt2, color);
            Debug.DrawLine(vt2, vt3, color);
            Debug.DrawLine(vt3, vt4, color);
            Debug.DrawLine(vt4, vt1, color);

            Debug.DrawLine(vb1, vb2, color);
            Debug.DrawLine(vb2, vb3, color);
            Debug.DrawLine(vb3, vb4, color);
            Debug.DrawLine(vb4, vb1, color);

            Debug.DrawLine(vb1, vt1, color);
            Debug.DrawLine(vb2, vt2, color);
            Debug.DrawLine(vb3, vt3, color);
            Debug.DrawLine(vb4, vt4, color);
        }
    }
}