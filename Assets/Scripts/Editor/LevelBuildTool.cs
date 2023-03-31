using System;
using System.Collections.Generic;
using GameManager;
using Level;
using UI.Minimap;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

namespace Game.Editor
{
    public class LevelBuildTool : EditorWindow
    {
        [MenuItem("MiniJam128/Level Build Tool")]
        private static void ShowWindow()
        {
            var window = GetWindow<LevelBuildTool>();
            window.titleContent = new GUIContent("TITLE");
            window.Show();
        }

        private GameObject trappedPersonPrefab => levelManager.trappedPersonPrefab;
        private PersonalInfoDatabase personalInfoDatabase => levelManager.personalInfoDatabase;
        private Transform trappedPersonParent => levelManager.trappedPersonParent;
        private WorldRangeBox rangeBox => levelManager.rangeBox;
        private LevelManager levelManager;
        private int genCount;
        private Vector2 lifeOffset;
        private Transform levelBuildingsParent;

        private void OnGUI()
        {
            levelManager = UniversalUtils.ObjectField<LevelManager>("Level Manager", levelManager);
            genCount = EditorGUILayout.IntField("Count", genCount);
            lifeOffset = EditorGUILayout.Vector2Field("Life Offset", lifeOffset);
            if (GUILayout.Button("Generate Trapped Person"))
            {
                if (trappedPersonPrefab != null && trappedPersonParent != null && personalInfoDatabase != null &&
                    genCount > 0)
                {
                    List<TrappedPersonInfo> newInfos =
                        new List<TrappedPersonInfo>(personalInfoDatabase.infos.ToArray());
                    UniversalUtils.Shuffle(newInfos);

                    for (int i = 0; i < genCount; i++)
                    {
                        GameObject obj = GameObject.Instantiate(trappedPersonPrefab, trappedPersonParent);
                        if (obj.TryGetComponent(out TrappedPerson person))
                        {
                            person.personalInfo = newInfos[i];
                            person.severity = (Severity)Random.Range(0, 3);
                            person.timeOffset = Random.Range((int)lifeOffset.x, (int)lifeOffset.y);
                        }

                        Vector2 v2 = RandomPosition(0.8f);
                        Vector3 pos = new Vector3(v2.x, 1, v2.y);

                        obj.transform.position = pos;
                    }
                }
            }

            if (GUILayout.Button("Reset Life Offset"))
            {
                foreach (Transform person in trappedPersonParent)
                {
                    if (person.TryGetComponent(out TrappedPerson component))
                    {
                        component.timeOffset = Random.Range((int)lifeOffset.x, (int)lifeOffset.y);
                    }
                }
            }

            if (GUILayout.Button("Clear"))
            {
                if (trappedPersonParent != null)
                    for (int i = trappedPersonParent.childCount - 1; i >= 0; i--)
                    {
                        DestroyImmediate(trappedPersonParent.GetChild(i).gameObject);
                    }
            }

            if (GUILayout.Button("Add Personal Infos"))
            {
                personalInfoDatabase.infos.Add(new TrappedPersonInfo());
            }

            if (GUILayout.Button("Random Age"))
            {
                foreach (var info in personalInfoDatabase.infos)
                {
                    info.age.age = (uint)Random.Range(1, 100);
                }
            }

            if (GUILayout.Button("Random Families Count"))
            {
                foreach (var info in personalInfoDatabase.infos)
                {
                    if (Random.Range(0f, 1f) > 0.5f)
                    {
                        info.familiesCount = Random.Range(0, 4);
                    }
                    else
                    {
                        info.familiesCount = 0;
                    }
                }
            }

            levelBuildingsParent =
                UniversalUtils.ObjectField<Transform>("Level Buildings Parent", levelBuildingsParent);
            if (GUILayout.Button("Random Buildings"))
            {
                foreach (Transform trans in levelBuildingsParent)
                {
                    Vector2 v2 = RandomPosition(0.8f);
                    trans.position = v2.XYZ();
                    trans.rotation = new Quaternion();
                    trans.Rotate(Vector3.forward, Random.Range(-15f, 15f));
                    trans.Rotate(Vector3.left, Random.Range(-15f, 15f));
                    trans.Rotate(Vector3.up, Random.Range(0f, 360f));
                }
            }
        }

        public Vector2 RandomPosition(float size)
        {
            Vector2 v2 = new Vector2(rangeBox.XSize, rangeBox.ZSize) * (0.5f * size);
            Vector2 max = rangeBox.Center.XZ() + v2;
            Vector2 min = rangeBox.Center.XZ() - v2;
            float x = Random.Range(min.x, max.x);
            float z = Random.Range(min.y, max.y);
            return new Vector2(x, z);
        }
    }
}