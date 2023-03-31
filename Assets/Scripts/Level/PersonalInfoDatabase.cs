using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    
    [CreateAssetMenu(fileName = "PersonalInfo", menuName = "MiniJam128/PersonalInfo", order = 0)]
    public class PersonalInfoDatabase : ScriptableObject
    {
        public List<TrappedPersonInfo> infos;
    }
}