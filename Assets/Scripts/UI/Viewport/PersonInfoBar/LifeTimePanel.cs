using System;
using UI.Module;
using UnityEngine;

namespace UI.Viewport.PersonInfoBar
{
    public class LifeTimePanel : FadeTwnUIBehaviour
    {
        private CounterWithIcon counterWithIcon;
        public CounterWithIcon CounterWithIcon => this.LazyGetComponent(counterWithIcon);
        private TrappedPerson person;

        public void Init(TrappedPerson person)
        {
            this.person = person;
            CounterWithIcon.Init((int)person.TimeRemain);
        }

        private void FixedUpdate()
        {
            if (person == null) return;
            OnRemainTimeChanged(person.TimeRemain);
        }

        public void OnRemainTimeChanged(float remainTime)
        {
            int life = (int)remainTime;
            CounterWithIcon.Counter = life;
        }
    }
}