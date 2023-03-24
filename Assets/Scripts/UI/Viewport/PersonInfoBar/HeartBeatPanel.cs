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
            OnHeartBeatChanged(person.GetBPM((int)person.TimeRemain, person.severity));
        }


        private int counter;
        public float interval = 2;

        private void FixedUpdate()
        {
            if (person == null) return;
            counter++;
            if (counter % (int)(50 * interval) == 0)
            {
                OnHeartBeatChanged(person.GetBPM((int)person.TimeRemain, person.severity));
            }
        }

        public void OnHeartBeatChanged(float value)
        {
            CounterWithIcon.Counter = (int)value;
        }
    }
}