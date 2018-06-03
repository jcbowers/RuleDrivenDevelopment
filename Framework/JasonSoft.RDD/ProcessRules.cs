namespace JasonSoft.RDD
{
    [System.AttributeUsage(System.AttributeTargets.Method, AllowMultiple = false)]
    public class ProcessRules : System.Attribute
    {
        readonly ProcessRulesAdvise _advise;

        public ProcessRules(ProcessRulesAdvise advise)
        {
            _advise = advise;
        }
    }
}
