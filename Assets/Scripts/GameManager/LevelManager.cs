using System;
using System.Collections.Generic;
using GameInput;
using UI;
using UI.Minimap;
using UI.Viewport;
using TriageTags;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameManager
{
    [RequireComponent(typeof(Counter))]
    public class LevelManager : MonoBehaviour
    {
        private event EventHandler End;

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
        [FormerlySerializedAs("novelRescue")] public NovelRescue rescue;
        public Timer Timer => this.LazyGetComponent(timer);
        public int TimeRemain => timeToEnd - Timer.IntTick;// how long from now to end
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

        private void Start()
        {
            rescue = new NovelRescue();
            End += (_, _) => { Endings(); };
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
            FreshRescueList();
            DebugUpdate();
            CheckEndings();
        }

        private void FreshPersonStatus()
        {
            foreach (var person in TrappedPersons)
            {
                if (person.Status is PersonStatus.Died or PersonStatus.Saved) continue;
                if (person.time < Timer.Tick)
                {
                    person.Status = PersonStatus.Died;
                    Counter.AddDiedPerson(person);
                    rescue.Remove(person);
                    // TODO: how does ui deal with a person's death?
                }
            }

            foreach (var person in rescue.rescuing)
            {
                if (person.rescueTime >= TrappedPerson.TimeToRescue)
                {
                    person.Status = PersonStatus.Saved;
                    Counter.AddSavedPerson(person);
                }
            }
        }

        private void FreshRescueList()
        {
            if (rescue.rescuing.Count < NovelRescue.RescuingLimit)
            {
                rescue.ShiftUp();
            }
        }


        public void MakeTag(TrappedPerson person, TriageTag triageTag)
        {
            // todo: deal with black tag
            if (person.TriageTag == triageTag) return;
            
            rescue.Insert(person, triageTag);
        }

        #region Endings

        // how long this game will be
        [SerializeField] private int timeToEnd;

        private void Endings()
        {
            // TODO: impl endings
        }

        private void CheckEndings()
        {
            if (Timer.IntTick > timeToEnd)
            {
                End?.Invoke(null, EventArgs.Empty);
            }

            if (Counter.SavedPersonsCount + Counter.DiedPersonsCount >= TrappedPersons.Count)
            {
                End?.Invoke(null, EventArgs.Empty);
            }
        }

        #endregion


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