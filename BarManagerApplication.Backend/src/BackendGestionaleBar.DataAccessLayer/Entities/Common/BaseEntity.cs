namespace BackendGestionaleBar.DataAccessLayer.Entities.Common;

public abstract class BaseEntity
{
    public Guid Id { get; set; }

    public DateTime CreatedDate { get; internal set; }

    public DateTime? LastModifiedDate { get; internal set; }
}