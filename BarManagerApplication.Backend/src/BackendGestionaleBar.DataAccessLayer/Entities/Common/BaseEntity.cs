namespace BackendGestionaleBar.DataAccessLayer.Entities.Common;

public abstract class BaseEntity
{
    protected BaseEntity()
    {
    }

    public Guid Id { get; set; }

    internal DateTime CreatedDate { get; set; }

    internal Guid CreatedBy { get; set; }

    internal DateTime? LastModifiedDate { get; set; }

    public Guid? UpdatedBy { get; set; }
}