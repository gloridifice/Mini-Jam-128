using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI.Viewport
{
    public class FloatingUI : UIBehaviour
    {
        [HideInInspector] public Vector2 actualPos;
        [HideInInspector] public Quaternion actualRot;

        [Range(0, 1)] public float offsetGain;
        [FormerlySerializedAs("rotateGain")] [Range(0, 1)] public float rotationGain;
        [Range(0, 1)] public float deltaSpeed;

        public bool inverseOffset;
        [FormerlySerializedAs("inverseRotate")] public bool inverseRotation;

        private Vector2 targetPos;
        private Vector3 targetRot;

        private void Awake()
        {
            actualPos = Rect.anchoredPosition;
            actualRot = Rect.rotation;
        }

        private void Update()
        {
            Vector2 mousePos = Input.mousePosition;

            int width = Camera.main.pixelWidth;
            int height = Camera.main.pixelHeight;

            Vector2 vctToCenter = (mousePos - new Vector2(width / 2f, height / 2f));
            
            //offset
            Vector2 v2 = vctToCenter / 2 * offsetGain;
            v2 = inverseOffset ? - v2 : v2;
            targetPos = actualPos + v2;

            //rotation
            Rect.rotation = actualRot;
            float xRot = vctToCenter.y / height * (rotationGain * 60);
            float yRot = -vctToCenter.x / width * (rotationGain * 60);

            int sign = inverseRotation ? -1 : 1;
            Vector3 rot = new Vector3(sign * xRot, sign * yRot, 0);
            Rect.Rotate(rot);

            Vector2 pos = Rect.anchoredPosition;
            pos = Vector2.Lerp(pos, targetPos, deltaSpeed * 50f * Time.deltaTime);
            Rect.anchoredPosition = pos;
        }
    }
}