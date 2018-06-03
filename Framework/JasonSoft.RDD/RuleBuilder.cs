using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace JasonSoft.RDD
{
    public class RuleBuilder<T> : IConditionStep<T>, IAdditionalConditionStep<T>, IActionStep<T>, IAdditionalActionStep<T>
    {
        private List<Predicate<T>> _states = new List<Predicate<T>>();
        private string _action;
        private List<Action<T>> _stateRules = new List<Action<T>>();

        private RuleBuilder()
        {

        }

        public static IConditionStep<T> Instance()
        {
            return new RuleBuilder<T>();
        }

        public IAdditionalConditionStep<T> Given(Predicate<T> state)
        {
            _states.Add(state);

            return this;
        }

        public IAdditionalConditionStep<T> And(Predicate<T> state)
        {
            return Given(state);
        }

        public IActionStep<T> When(Expression<Action<T>> action)
        {
            _action = ((MethodCallExpression)action.Body).Method.Name;
            return this;
        }

        public IAdditionalActionStep<T> Then(Action<T> stateRule)
        {
            _stateRules.Add(stateRule);
            return this;
        }

        public IAdditionalActionStep<T> And(Action<T> stateRule)
        {
            return Then(stateRule);
        }

        public StateRule<T> Build()
        {
            return new StateRule<T>(_states, _action, _stateRules);
        }
    }
}
