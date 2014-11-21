namespace Agile.Common.EventPublishing
{
    public interface IContainer
    {
        T Get<T>() where T : class;
    }
}
