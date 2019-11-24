using Bogus;
using GlobomnaticsShop.Api.Models;
using System;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.Results;

namespace GlobomnaticsShop.Controllers
{
    public class ProductsController : ApiController
    {
        public IHttpActionResult Get()
        {
            var imageSizes = new int[] { 640, 480, 300, 500 };
            var products = new Faker<ProductModel>()
                            .RuleFor(o => o.Id, f => Guid.NewGuid())
                            .RuleFor(o => o.Name, f => f.Commerce.ProductName())
                            .RuleFor(o => o.Image, f => f.Image.PicsumUrl(f.PickRandomParam(imageSizes), f.PickRandomParam(imageSizes)))
                            .RuleFor(o => o.Price, f => f.Commerce.Price());

            return Ok(products.Generate(100));
        }

    }
}