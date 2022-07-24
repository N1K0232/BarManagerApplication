namespace BackendGestionaleBar.DataAccessLayer.Entities.Common;

public abstract class BaseEntity
{
    protected BaseEntity()
    {
    }

    public Guid Id { get; set; }

    internal DateTime CreatedDate { get; set; }

    internal DateTime? LastModifiedDate { get; set; }
}