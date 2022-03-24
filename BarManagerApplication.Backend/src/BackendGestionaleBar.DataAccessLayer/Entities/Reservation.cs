using BackendGestionaleBar.DataAccessLayer.Entities.Common;
using System;

namespace BackendGestionaleBar.DataAccessLayer.Entities
{
    public class Reservation : BaseEntity
    {
        public Guid IdUser { get; set; }
        public DateTime? Date { get; set; }
        public TimeSpan? Time { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}