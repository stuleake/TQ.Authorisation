namespace TQ.Geocoding.Repository
{
    public interface IAddressReadOnlyRepository<TEntity> : IReadOnlyRepository<TEntity> where TEntity : class
    {
    }
}