using System;
using TMPro;
using UI.Module.Button;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

[UnityEditor.CustomEditor(typeof(RoundedButton))]
public class RoundedButtonEditor : UnityEditor.Editor
{
    protected RoundedButton component;
    private void OnEnable()
    {
        component = target as RoundedButton;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TMP_Text text = component.buttonText;
        Image frame = component.frameImage;
        Image icon = component.iconImage;
        Undo.RecordObject(text, "Change Text");
        Undo.RecordObject(frame, "Change Frame");
        Undo.RecordObject(icon, "Change Icon");
        Undo.RecordObject(component, "Change C");

        text.text = EditorGUILayout.TextField("Text", text.text);
        text.color = EditorGUILayout.ColorField("Text Color", text.color);
        component.highlightColor = EditorGUILayout.ColorField("Highlight Text Color", component.highlightColor);

        frame.color = EditorGUILayout.ColorField("Frame Color", frame.color);

        icon.sprite = EditorGUILayout.ObjectField("Icon", icon.sprite, typeof(Sprite), icon.sprite) as Sprite;
        
        EditorUtility.SetDirty(text);
        EditorUtility.SetDirty(frame);
        EditorUtility.SetDirty(icon);
        EditorUtility.SetDirty(component);
        PrefabUtility.RecordPrefabInstancePropertyModifications(text);
        PrefabUtility.RecordPrefabInstancePropertyModifications(frame);
        PrefabUtility.RecordPrefabInstancePropertyModifications(icon);
        PrefabUtility.RecordPrefabInstancePropertyModifications(component);
    }
    
}