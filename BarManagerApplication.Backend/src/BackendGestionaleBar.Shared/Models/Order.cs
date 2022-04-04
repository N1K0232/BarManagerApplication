using BackendGestionaleBar.Shared.Models.Common;
using BackendGestionaleBar.Shared.Models.Enums;

namespace BackendGestionaleBar.Shared.Models
{
    public class Order : BaseModel
    {
        public User User { get; set; }

        public DateTime OrderDate { get; set; }

        public TimeSpan OrderTime { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public decimal TotalPrice { get; set; }
    }
}