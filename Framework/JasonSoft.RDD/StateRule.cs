using System;
using System.Collections.Generic;

namespace JasonSoft.RDD
{
    public class StateRule<T>
    {
        public StateRule(IEnumerable<Predicate<T>> stateConditions, string transitionName, IEnumerable<Action<T>> stateActions)
        {
            if(stateConditions == null)
            {
                throw new ArgumentException("stateConditions are required");
            }

            if (stateActions == null)
            {
                throw new ArgumentException("stateActions are required");
            }

            StateConditions = stateConditions;
            TransitionName = transitionName;
            StateActions = stateActions;
        }

        public IEnumerable<Predicate<T>> StateConditions { get; private set; }

        public string TransitionName { get; private set; }

        public IEnumerable<Action<T>> StateActions { get; private set; }
    }
}
