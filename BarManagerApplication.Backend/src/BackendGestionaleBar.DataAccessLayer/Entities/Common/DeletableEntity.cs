namespace BackendGestionaleBar.DataAccessLayer.Entities.Common;

public abstract class DeletableEntity : BaseEntity
{
    protected DeletableEntity() : base()
    {
    }

    internal bool IsDeleted { get; set; }

    internal DateTime? DeletedDate { get; set; }

    internal Guid? DeletedBy { get; set; }
}