using System.Collections.Generic;
using TriageTags;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameManager
{
    [RequireComponent(typeof(Counter), typeof(Ending))]
    public class LevelManager : MonoBehaviour
    {
        public List<TrappedPerson> trappedPersons;
        
        private Counter counter;
        private Timer timer;
        [FormerlySerializedAs("novelRescue")] public NovelRescue rescue;

        private void Start()
        {
            timer = GetComponent<Timer>();
            counter = GetComponent<Counter>();
            rescue = new NovelRescue();
        }

        private void Update()
        {
            FreshPersonStatus();
            FreshRescueList();
        }

        private void FreshPersonStatus()
        {
            foreach (var person in trappedPersons)
            {
                if (person.status is PersonStatus.Died or PersonStatus.Saved) continue;
                if (person.time < timer.Tick)
                {
                    person.status = PersonStatus.Died;
                    counter.AddDiedPerson(person);
                    rescue.Remove(person);
                    // TODO: how does ui deal with a person's death?
                }
            }

            foreach (var person in rescue.rescuing)
            {
                if (person.rescueTime >= TrappedPerson.TimeToRescue)
                {
                    person.status = PersonStatus.Saved;
                    counter.AddSavedPerson(person);
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

        public void AddRescue(TrappedPerson person, TriageTag triageTag)
        {
            // todo: deal with black tag
            if (person.triageTag == triageTag || person.triageTag == TriageTags.TriageTags.Black) return;
            
            rescue.Insert(person, triageTag);
        }
    }
}
