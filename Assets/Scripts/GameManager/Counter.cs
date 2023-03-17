using System.Collections.Generic;
using UnityEngine;

namespace GameManager
{
    public class Counter : MonoBehaviour
    {
        private List<TrappedPerson> savedPersons;
        private List<TrappedPerson> diedPersons;

        public List<TrappedPerson> SavedPersons
        {
            get => savedPersons;
        }

        public List<TrappedPerson> DiedPersons
        {
            get => diedPersons;
        }

        private void Awake()
        {
            savedPersons = new List<TrappedPerson>();
            diedPersons = new List<TrappedPerson>();
        }

        public void AddSavedPerson(TrappedPerson tar)
        {
            savedPersons.Add(tar);
        }

        public void AddDiedPerson(TrappedPerson tar)
        {
            diedPersons.Add(tar);
        }
    }
}
