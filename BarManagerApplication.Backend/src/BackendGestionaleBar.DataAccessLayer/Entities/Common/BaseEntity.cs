using System;

namespace BackendGestionaleBar.DataAccessLayer.Entities.Common
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
    }
}