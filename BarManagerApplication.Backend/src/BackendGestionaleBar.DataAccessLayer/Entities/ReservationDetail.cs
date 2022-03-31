using BackendGestionaleBar.DataAccessLayer.Entities.Common;
using System;

namespace BackendGestionaleBar.DataAccessLayer.Entities
{
    public class ReservationDetail : BaseEntity
    {
        public Guid IdReservation { get; set; }
        public Guid IdProduct { get; set; }
    }
}