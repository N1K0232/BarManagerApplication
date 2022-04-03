using BackendGestionaleBar.DataAccessLayer.Entities.Common;
using BackendGestionaleBar.Shared.Models.Enums;

namespace BackendGestionaleBar.DataAccessLayer.Entities
{
    public class Order : BaseEntity<int>
    {
        public Guid IdUser { get; set; }

        public DateOnly OrderDate { get; set; }

        public TimeOnly OrderTime { get; set; }

        public OrderStatus OrderStatus { get; set; }
    }
}
