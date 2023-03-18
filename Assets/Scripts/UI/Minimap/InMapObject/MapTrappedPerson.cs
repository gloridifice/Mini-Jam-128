using TriageTags;
using UnityEngine;

namespace UI.Minimap
{
    public class MapTrappedPerson : InMapObject
    {
        private TrappedPerson trappedPerson;
        
        public void Init(TrappedPerson person)
        {
            trappedPerson = person;
            Init(MinimapUtils.WorldPositionToMapPosition(MinimapManager.Instance.RangeBox, person.transform.position));

            person.OnTrappedPersonTagChanged += OnTagChanged;
            person.OnPersonStatusChanged += OnStatusChanged;
        }

        void OnTagChanged(TriageTag preTag, TriageTag newTag)
        {
            
        }

        void OnStatusChanged(PersonStatus preStatus, PersonStatus newStatus)
        {
            
        }
    }
}