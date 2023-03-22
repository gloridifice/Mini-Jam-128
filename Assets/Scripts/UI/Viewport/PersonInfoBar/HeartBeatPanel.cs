using System;
using UI.Module;
using UnityEngine;

namespace UI.Viewport.PersonInfoBar
{
    public class HeartBeatPanel : FadeTwnUIBehaviour
    {
        private TrappedPerson person;
        private CounterWithIcon counterWithIcon;
        public CounterWithIcon CounterWithIcon => this.LazyGetComponent(counterWithIcon);

        public void Init(TrappedPerson person)
        {
            this.person = person;
        }

        public void OnHeartBeatChanged(float value)
        {
            CounterWithIcon.Counter = (int) value;
        }
    }
}