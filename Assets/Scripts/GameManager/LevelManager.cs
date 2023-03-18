using System.Collections.Generic;
using TriageTags;
using UnityEngine;

namespace GameManager
{
    [RequireComponent(typeof(Counter), typeof(Ending))]
    public class LevelManager : MonoBehaviour
    {
        public List<TrappedPerson> trappedPersons;
        
        private Counter counter;
        private Timer timer;
        //private Rescue rescue;
        public NovelRescue novelRescue;

        private void Start()
        {
            timer = GetComponent<Timer>();
            counter = GetComponent<Counter>();
            //rescue = new Rescue();
            novelRescue = new NovelRescue();
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
                    // if (rescue.Find(person))
                    // {
                    //     rescue.Remove(person);
                    // }
                    novelRescue.Remove(person);
                    // TODO: how does ui deal with a person's death?
                    // Debug.Log("person " + person.name + " died");
                }
            }

            foreach (var person in novelRescue.rescuing)
            {
                if (person.rescueTime >= TrappedPerson.TimeToRescue)
                {
                    person.status = PersonStatus.Saved;
                    counter.AddSavedPerson(person);
                    //Debug.Log("person " + person.name + " saved");
                }
            }
        }

        private void FreshRescueList()
        {
            if (novelRescue.rescuing.Count < NovelRescue.RescuingLimit)
            {
                novelRescue.ShiftUp();
            }
        }

        public void AddRescue(TrappedPerson person, TriageTag triageTag)
        {
            // rescue.Insert(person, priority);
            
            // todo: deal with black tag
            if (person.triageTag == triageTag || person.triageTag == TriageTags.TriageTags.Black) return;
            
            novelRescue.Insert(person, triageTag);

        }
    }
}
