namespace BackendGestionaleBar.DataAccessLayer.Entities.Common;

public abstract class DeletableEntity : BaseEntity
{
    public bool IsDeleted { get; internal set; }

    public DateTime? DeletedDate { get; internal set; }
}