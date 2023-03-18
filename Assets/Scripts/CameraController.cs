using System;
using GameManager;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    private Camera thisCamera;
    private Camera ThisCamera => this.LazyGetComponent(thisCamera);
    LevelManager LevelManager => LevelManager.Instance;

    //四个角落位置相对于相机的位置
    [HideInInspector] public Vector2 topLeft, topRight, bottomRight, bottomLeft;
    private Vector2 center;
    public Vector2 Center => center + transform.position.XZ();

    #region EventAPI

    public event Action<CameraController> OnCameraMoved = controller => { };

    #endregion

    private void Awake()
    {
        int height = ThisCamera.pixelHeight;
        int width = ThisCamera.pixelWidth;
        Ray ray0 = ThisCamera.ScreenPointToRay(new Vector3(0, height, 0));
        Ray ray1 = ThisCamera.ScreenPointToRay(new Vector3(width, height, 0));
        Ray ray2 = ThisCamera.ScreenPointToRay(new Vector3(width, 0, 1));
        Ray ray3 = ThisCamera.ScreenPointToRay(new Vector3(0, 0, 1));

        topLeft = GetCornerPos(ray0);
        topRight = GetCornerPos(ray1);
        bottomRight = GetCornerPos(ray2);
        bottomLeft = GetCornerPos(ray3);

        center = (topLeft + topRight + bottomLeft + bottomRight) / 4;

        targetPos = transform.position;
    }

    private Vector2 GetCornerPos(Ray ray)
    {
        Plane ground = new Plane(Vector3.up, Vector3.zero);
        if (ground.Raycast(ray, out float enter))
        {
            Vector3 pos = ray.GetPoint(enter);
            var position = transform.position;
            return pos.XZ() - position.XZ();
        }

        return Vector2.zero;
    }

    private void Update()
    {
        Move();
        Color color = Color.red;
        Debug.DrawLine(topLeft.XYZ(), topRight.XYZ(), color);
        Debug.DrawLine(topRight.XYZ(), bottomRight.XYZ(), color);
        Debug.DrawLine(bottomRight.XYZ(), bottomLeft.XYZ(), color);
        Debug.DrawLine(bottomLeft.XYZ(), topLeft.XYZ(), color);
    }

    #region Move

    [Header("Move")] Vector3 targetPos;
    public Vector2 moveVelocity;
    [Range(0f, 1f)] public float followingSpeed;
    private bool isMoving;

    private void Move()
    {
        Vector2 move = LevelManager.Input.Move;
        if (move != Vector2.zero)
        {
            move *= moveVelocity * (Time.deltaTime * 50f);
            targetPos += move.XYZ();
            isMoving = true;
        }

        if (isMoving)
        {
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 25f * followingSpeed);
            if ((transform.position - targetPos).magnitude < 0.03f)
            {
                transform.position = targetPos;
                isMoving = false;
            }
            else
            {
                OnCameraMoved?.Invoke(this);
            }
        }
    }

    #endregion
}