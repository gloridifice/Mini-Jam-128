using System.Collections.Generic;
using UnityEngine;

namespace GameManager
{
    [RequireComponent(typeof(Counter), typeof(Ending))]
    public class LevelManager : MonoBehaviour
    {
        public List<TrappedPerson> trappedPersons;
        private Counter counter;

        private Timer timer;
        private Rescue rescue;

        private void Start()
        {
            timer = GetComponent<Timer>();
            counter = GetComponent<Counter>();
        }

        private void Update()
        {
            FreshPersonStatus();
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
                    // Debug.Log("person " + person.name + " died");
                }

                if (person.rescueTime >= TrappedPerson.TimeToRescue)
                {
                    person.status = PersonStatus.Saved;
                    counter.AddSavedPerson(person);
                    //Debug.Log("person " + person.name + " saved");
                }
            }
        }
    }
}
