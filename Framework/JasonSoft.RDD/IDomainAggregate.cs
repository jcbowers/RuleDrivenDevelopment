namespace JasonSoft.RDD
{
    public interface IDomainAggregate<T>
    {
        StateRule<T>[] Rules { get; }
    }
}
