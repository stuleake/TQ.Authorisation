namespace TQ.Geocoding.Repository
{
    public interface ILocationReadOnlyRepository<TEntity> : IReadOnlyRepository<TEntity> where TEntity : class
    {
    }
}