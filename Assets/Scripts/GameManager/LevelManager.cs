using System;
using System.Collections.Generic;
using UI;
using UI.Minimap;
using UI.Viewport;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameManager
{
    [RequireComponent(typeof(Counter), typeof(Ending), typeof(Timer))]
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance;

        #region EditorInspector

        public WorldRangeBox rangeBox;
        public LevelUIManager levelUIManager;
        public Transform trappedPersonParent;

        #endregion

        MinimapManager MinimapManager => levelUIManager.minimapManager;
        ViewportUIManager ViewportUIManager => levelUIManager.viewportUIManager;
        private List<TrappedPerson> trappedPersons;
        public List<TrappedPerson> TrappedPersons
        {
            get
            {
                if (trappedPersons == null)
                {
                    trappedPersons = new List<TrappedPerson>();
                    foreach (Transform personTran in trappedPersonParent)
                    {
                        if (personTran.TryGetComponent(out TrappedPerson trappedPerson))
                        {
                            trappedPersons.Add(trappedPerson);
                        }
                    }
                }

                return trappedPersons;
            }
        }

        private Counter counter;
        public Counter Counter => this.LazyGetComponent(counter);

        private Timer timer;
        public Timer Timer => this.LazyGetComponent(timer);

        private Rescue rescue;

        private CameraController cameraController;
        public CameraController CameraController
        {
            get
            {
                if (cameraController == null)
                {
                    if (Camera.main.TryGetComponent(out CameraController controller))
                    {
                        cameraController = controller;
                    }
                    else
                    {
                        Debug.LogError("CameraController is not exist.");
                    }
                }

                return cameraController;
            }
        }

        private LevelInput input;
        public LevelInput Input => this.LazyGetComponent(input);
        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            FreshPersonStatus();
            DebugUpdate();
        }

        private void FreshPersonStatus()
        {
            foreach (var person in TrappedPersons)
            {
                if (person.status is PersonStatus.Died or PersonStatus.Saved) continue;
                if (person.time < Timer.Tick)
                {
                    person.status = PersonStatus.Died;
                    Counter.AddDiedPerson(person);
                    // Debug.Log("person " + person.name + " died");
                }
            }
        }

        #region Debug

        private void DebugUpdate()
        {
            //init minimap
            if (UnityEngine.Input.GetKeyDown((KeyCode.O)))
            {
                MinimapManager.Init(TrappedPersons);
                ViewportUIManager.Init(TrappedPersons);
            }
        }

        #endregion
    }
}