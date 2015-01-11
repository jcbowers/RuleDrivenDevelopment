using System;
using System.Linq.Expressions;

namespace JasonSoft.RDD
{
    public interface IConditionStep<T>
    {
        IAdditionalConditionStep<T> Given(Predicate<T> state);
    }

    public interface IAdditionalConditionStep<T>
    {
        IAdditionalConditionStep<T> And(Predicate<T> state);
        IActionStep<T> When(Expression<Action<T>> action);
        IAdditionalActionStep<T> Then(Action<T> stateRule);
    }

    public interface IActionStep<T>
    {
        IAdditionalActionStep<T> Then(Action<T> stateRule);
        StateRule<T> Build();
    }

    public interface IAdditionalActionStep<T>
    {
        IAdditionalActionStep<T> And(Action<T> stateRule);
        StateRule<T> Build();
    }
}
