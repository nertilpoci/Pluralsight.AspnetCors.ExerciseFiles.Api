using System;

namespace GlobomnaticsShop.Api.Models
{
     class ProductModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Price { get; set; }
    }
}