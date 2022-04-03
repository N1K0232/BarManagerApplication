namespace BackendGestionaleBar.DataAccessLayer.Entities.Common
{
    public abstract class BaseEntity<TKey> where TKey : struct
    {
        public TKey Id { get; set; }
    }
}