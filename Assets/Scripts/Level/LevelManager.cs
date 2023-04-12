using System;
using System.Collections.Generic;
using GameInput;
using Level;
using MiniJam128.LevelManagement;
using UI;
using UI.Minimap;
using UI.Viewport;
using TriageTags;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace GameManager
{
    [RequireComponent(typeof(PersonCounter))]
    public class LevelManager : MonoBehaviour
    {
        private event Action End = () => { };

        public static LevelManager Instance;

        #region EditorInspector

        public WorldRangeBox rangeBox;
        public LevelUIManager levelUIManager;
        public Transform trappedPersonParent;

        #endregion

        private SubtitlesPool subtitlesPool;
        public SubtitlesPool SubtitlesPool => this.LazyGetComponent(subtitlesPool);
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
                            trappedPerson.onBeFound.AddListener(cameraController.onFindTrappedPerson.Invoke);
                            trappedPerson.onStatusChanged.AddListener(onPersonStatusChanged.Invoke);
                        }
                    }
                }

                return trappedPersons;
            }
        }

        private PersonCounter personCounter;
        public PersonCounter PersonCounter => this.LazyGetComponent(personCounter);
        private Timer timer;
        [FormerlySerializedAs("novelRescue")] public NovelRescue rescue;
        public Timer Timer => this.LazyGetComponent(timer);
        public float TimeRemain => timeToEnd - Timer.Tick; // how long from now to end
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

        #region Inspector

        [Header("Assets")] public PersonalInfoDatabase personalInfoDatabase;
        public GameObject trappedPersonPrefab;

        [Header("Level")]
        // how long this game will be
        [SerializeField]
        private int timeToEnd;

        public float scanningTime = 5;
        public float rescueTime = 15;

        public float soundScanningTime = 0.2f;
        public float healthScanningTime = 0.4f;
        public float lifeScanningTime = 1.0f;
        [Header("Trapped Person Life Time")] public float lowLifeTime = 240;
        public float midLifeTime = 300;
        public float highLifeTime = 360;

        #endregion

        [HideInInspector] public LevelInstance levelInstance;

        [HideInInspector] public bool isEnded;
        [HideInInspector] public FloatEvent onBatteryChanged;

        private void Start()
        {
            rescue = new NovelRescue();
            End += OnEnd;
            levelUIManager.Init();

            CameraController.onFindTrappedPerson.AddListener(OnFindTrappedPerson);
            onPersonStatusChanged.AddListener(OnPersonStatusChanged);
        }

        [HideInInspector] public int foundPersonCount;
        [HideInInspector] public int savedPersonCount;
        [HideInInspector] public UnityEvent<int> onFoundPersonCountChanged;
        [HideInInspector] public UnityEvent<int> onSavedPersonCountChanged;
        [HideInInspector] public UnityEvent<TrappedPerson, PersonStatus, PersonStatus> onPersonStatusChanged;

        void OnFindTrappedPerson(TrappedPerson person)
        {
            foundPersonCount++;
            onFoundPersonCountChanged.Invoke(foundPersonCount);
        }

        void OnPersonStatusChanged(TrappedPerson person, PersonStatus preStatus, PersonStatus newStatus)
        {
            if (newStatus == PersonStatus.Saved)
            {
                savedPersonCount++;
                onSavedPersonCountChanged.Invoke(savedPersonCount);
            }
        }

        private LevelInput levelInput;
        public LevelInput LevelInput => this.LazyGetComponent(levelInput);

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            FreshPersonStatus();
            FreshRescueList();
            BatteryUpdate();
            DebugUpdate();
            CheckEndings();
            InputUpdate();
        }

        void InputUpdate()
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                if (isEnded && !MiniJam128.GameManager.Instance.IsLastLevel)
                {
                    EnterNextLevel();
                }
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                MiniJam128.GameManager.Instance.LoadMainMenu();
            }
        }

        void EnterNextLevel()
        {
            MiniJam128.GameManager.Instance.LoadLevel(levelInstance.index + 1);
        }

        public void BatteryUpdate()
        {
            if (TimeRemain >= 0)
            {
                onBatteryChanged.Invoke(TimeRemain / timeToEnd);
            }
        }

        private void FreshPersonStatus()
        {
            foreach (var person in TrappedPersons)
            {
                if (person.Status is PersonStatus.Died or PersonStatus.Saved) continue;
                if (person.Time < Timer.Tick)
                {
                    person.Status = PersonStatus.Died;
                    PersonCounter.AddDiedPerson(person);
                    rescue.Remove(person);
                    person.gameObject.SetActive(false);
                }
            }

            var tmp = new List<TrappedPerson>();
            foreach (var person in rescue.rescuing)
            {
                if (person.rescueTime >= rescueTime)
                {
                    person.Status = PersonStatus.Saved;
                    PersonCounter.AddSavedPerson(person);
                    tmp.Add(person);
                }
            }

            foreach (var person in tmp)
            {
                rescue.Remove(person);
            }
        }

        private void FreshRescueList()
        {
            if (rescue.rescuing.Count < NovelRescue.RescuingLimit)
            {
                rescue.ShiftUp();
            }
        }


        public void MarkTag(TrappedPerson person, TriageTag triageTag)
        {
            if (person.TriageTag == triageTag) return;
            if (triageTag == TriageTags.TriageTags.Black)
            {
                rescue.Remove(person);
                person.TriageTag = TriageTags.TriageTags.Black;
                return;
            }

            rescue.Insert(person, triageTag);
        }


        #region OnEnd

        private void OnEnd()
        {
            levelUIManager.settlementUIManager.DisplayAndInit();
            PersonCounter.IncomeDataToGameManager();
            isEnded = true;
            End -= OnEnd;
        }

        private void CheckEndings()
        {
            if (Timer.IntTick > timeToEnd)
            {
                End?.Invoke();
            }

            if (PersonCounter.SavedPersonsCount + PersonCounter.DiedPersonsCount >= TrappedPersons.Count)
            {
                End?.Invoke();
            }
        }

        #endregion


        #region Debug

        private void DebugUpdate()
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                End?.Invoke();
            }
        }

        #endregion
    }
}