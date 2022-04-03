using BackendGestionaleBar.DataAccessLayer.Entities.Common;
using BackendGestionaleBar.Shared.Models.Enums;

namespace BackendGestionaleBar.DataAccessLayer.Entities
{
    public class Order : BaseEntity
    {
        public Guid IdUser { get; set; }

        public DateTime OrderDate { get; set; }

        public TimeSpan OrderTime { get; set; }

        public OrderStatus OrderStatus { get; set; }
    }
}