using System.Collections.Generic;
using Level;
using UnityEngine;

namespace GameManager
{
    public class Counter : MonoBehaviour
    {
        private List<TrappedPerson> savedPersons;
        private List<TrappedPersonSettlementInfo> savedPersonSettlementInfos;
        private List<TrappedPerson> diedPersons;

        public List<TrappedPerson> SavedPersons => savedPersons;
        public List<TrappedPerson> DiedPersons => diedPersons;

        public List<TrappedPersonSettlementInfo> SavedPersonSettlementInfos => savedPersonSettlementInfos;
        public int SavedPersonsCount => savedPersons.Count;
        public int DiedPersonsCount => diedPersons.Count;

        private void Awake()
        {
            savedPersons = new List<TrappedPerson>();
            diedPersons = new List<TrappedPerson>();
        }

        public void AddSavedPerson(TrappedPerson tar)
        {
            savedPersonSettlementInfos.Add(tar.GetSettlementInfo());
            savedPersons.Add(tar);
        }

        public void AddDiedPerson(TrappedPerson tar)
        {
            diedPersons.Add(tar);
        }
    }
}
