using UnityEngine;

public static class UniversalUtils
{
    public static T LazyGetComponent<T>(this MonoBehaviour behaviour, T component)
    {
        if (component == null)
        {
            component = behaviour.GetComponent<T>();
        }

        return component;
    }

    public static Vector2 XZ(this Vector3 vector3)
    {
        return new Vector2(vector3.x, vector3.z);
    }

    public static Vector3 XYZ(this Vector2 vector2, float y = 0)
    {
        return new Vector3(vector2.x, y, vector2.y);
    }   
}