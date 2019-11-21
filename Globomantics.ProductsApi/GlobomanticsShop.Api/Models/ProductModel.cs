using System;

namespace GlobomanticsShop.Api.Controllers
{
     class ProductModel
    {
        public Guid Id { get; internal set; }
        public string Name { get; internal set; }
        public string Image { get; internal set; }
        public string Price { get; internal set; }
    }
}