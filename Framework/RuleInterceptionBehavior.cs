using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace JasonSoft.RDD
{
    public class RuleInterceptionBehavior<T> : IInterceptionBehavior
    {
        public bool WillExecute
        {
            get
            {
                return true;
            }
        }

        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            if(input.MethodBase.Name == "get_Rules")
            {
                return getNext()(input, getNext);
            }

            var processRuleAttribute = input.MethodBase.CustomAttributes.SingleOrDefault(a => a.AttributeType == typeof(ProcessRules));
            var rulesInterface = input.Target as IDomainAggregate<T>;

            if (processRuleAttribute == null || rulesInterface == null)
            {
                return getNext()(input, getNext);
            }

            var target = (T)input.Target;

            var advise = (ProcessRulesAdvise)processRuleAttribute.ConstructorArguments.Single().Value;

            if(advise == ProcessRulesAdvise.Before || advise == ProcessRulesAdvise.Around)
            {
                RunRules(rulesInterface, target);
            }

            IMethodReturn result = getNext()(input, getNext);

            if (advise == ProcessRulesAdvise.After || advise == ProcessRulesAdvise.Around)
            {
                RunRules(rulesInterface, target);
            }

            return result;
        }

        private static void RunRules(IDomainAggregate<T> rulesInterface, T target)
        {
            foreach (var rule in rulesInterface.Rules.Where(r => r.StateConditions.All(c => c(target))).SelectMany(r => r.StateActions))
            {
                rule(target);
            }
        }
    }
}
