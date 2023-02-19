﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Firlansa.WebUI.Models.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int ColorId { get; set; }
        public virtual Color Color { get; set; }
        public int SizeId { get; set; }
        public virtual Size Size { get; set; }
        public int Quantity { get; set; }
    }
}