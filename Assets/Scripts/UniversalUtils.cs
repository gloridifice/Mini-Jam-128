using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

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

    public static T LazyGetComponentInParent<T>(this MonoBehaviour behaviour, T componet)
    {
        if (componet == null)
        {
            componet = behaviour.GetComponentInParent<T>();
        }

        return componet;
    }

    public static T LazyGetComponentInChildren<T>(this MonoBehaviour behaviour, T componet)
    {
        if (componet == null)
        {
            componet = behaviour.GetComponentInChildren<T>();
        }

        return componet;
    }

    public static Vector2 XZ(this Vector3 vector3)
    {
        return new Vector2(vector3.x, vector3.z);
    }

    public static Vector3 XYZ(this Vector2 vector2, float y = 0)
    {
        return new Vector3(vector2.x, y, vector2.y);
    }

    public static Color ColorFrom255(int r, int g, int b, int a = 255)
    {
        return new Color((float)r / 255f, (float)g / 255f, (float)b / 255f, (float)a / 255f);
    }

    public static Color FromHtmlString(String color)
    {
        return Color.black;
    }

    public static void Shuffle<T>(List<T> list)
    {
        for (var i = 0; i != list.Count - 1; i++)
        {
            var index = Random.Range(i + 1, list.Count);
            Assert.IsTrue(index >= 0);
            (list[i], list[index]) = (list[index], list[i]);
        }
    }

#if UNITY_EDITOR

    public static T ObjectField<T>(string name, Object obj) where T : Object
    {
        return EditorGUILayout.ObjectField(name, obj, typeof(T), obj) as T;
    }
#endif
}