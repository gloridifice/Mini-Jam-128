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
        }
    }
}