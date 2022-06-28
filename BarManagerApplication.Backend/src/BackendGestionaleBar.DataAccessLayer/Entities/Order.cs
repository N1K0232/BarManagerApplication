﻿using BackendGestionaleBar.Authentication.Entities;
using BackendGestionaleBar.DataAccessLayer.Entities.Common;
using BackendGestionaleBar.Shared.Enums;

namespace BackendGestionaleBar.DataAccessLayer.Entities;

public class Order : BaseEntity
{
    public Guid UserId { get; set; }

    public DateTime OrderDate { get; set; }

    public OrderStatus OrderStatus { get; set; }

    public ApplicationUser User { get; set; }

    public ICollection<OrderDetail> OrderDetails { get; set; }
}