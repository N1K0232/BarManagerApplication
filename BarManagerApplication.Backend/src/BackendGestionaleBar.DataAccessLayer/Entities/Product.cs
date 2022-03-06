﻿using System;

namespace BackendGestionaleBar.DataAccessLayer.Entities
{
    public class Product
    {
        public Guid IdProduct { get; set; }

        public int IdCategory { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public virtual Category Category { get; set; }
    }
}