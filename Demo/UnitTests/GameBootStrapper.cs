using System.Linq;

using BowlingKata;
using JasonSoft.RDD;
using JasonSoft.RDD.Helpers;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace BowlingKataTest
{
    class GameBootStrapper
    {
        public static void Configure(IUnityContainer container)
        {
            container.AddNewExtension<Interception>();

            container.RegisterType<Game>(
                new InjectionProperty("Rules", new[]
            {
                RuleBuilder<Game>.Instance()
                .Given(State<Game>.IsAny())
                .Then(g => g.ScoreRule = (frames) => frames.Sum(frame => frame.Score)).Build(),

                RuleBuilder<Game>.Instance()
                .Given(State<Game>.IsAny())
                .Then(g => g.TooManyPins = (frame, pins) => (!frame.IsStrikeOrSpare && frame.Rolls.FirstOrDefault() + pins > 10) || (frame.IsStrikeOrSpare && frame.Rolls[0] + pins > 20 ) ).Build(),

                RuleBuilder<Game>.Instance()
                .Given(g => g.CurrentFrame.IsFinalFrame)
                .Then(g => g.CurrentFrame.ScoreRule = (f) => f.Rolls.Sum<ushort>(r => r)).Build(),

                RuleBuilder<Game>.Instance()
                .Given(g => !g.CurrentFrame.IsFinalFrame)
                .And(g =>  g.CurrentFrame.Rolls.Sum(r => r) < 10)
                .Then(g => g.CurrentFrame.ScoreRule = (f) => f.Rolls.Sum<ushort>(r => r)).Build(),

                RuleBuilder<Game>.Instance()
                .Given(g => !g.CurrentFrame.IsFinalFrame)
                .And(g => g.CurrentFrame.Rolls.Sum(r => r).Between(10,19))
                .Then(g => g.CurrentFrame.ScoreRule = (f) => f.Rolls.Sum(r=>r) + f.NextFrame.Rolls.FirstOrDefault()).Build(),

                RuleBuilder<Game>.Instance()
                .Given(g => !g.CurrentFrame.IsFinalFrame)
                .And(g => g.CurrentFrame.Rolls.Sum(r => r) == 20)
                .Then(g => g.CurrentFrame.ScoreRule = (f) => 20 + f.NextFrame.Rolls.Take(2).Sum(r=>r).Max(10)).Build()
                }),
                new Interceptor<VirtualMethodInterceptor>(),
                new InterceptionBehavior<RuleInterceptionBehavior<Game>>());
        }
    }
}
