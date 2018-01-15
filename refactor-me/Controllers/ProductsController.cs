using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using refactor_me.Helpers;
using refactor_me.Models;
using refactor_me.Repositories;

namespace refactor_me.Controllers
{
    [RoutePrefix("products")]
    public class ProductsController : ApiController
    {
        private ProductRepository productRepository { get; set; }

        [Route]
        [HttpGet]
        public List<Product> GetAll()
        {
            productRepository = Helper.GetProductRepository();
            var products = productRepository.GetProducts(null);
            return products;
        }

        [Route]
        [HttpGet]
        public List<Product> SearchByName(string name)
        {
            productRepository = Helper.GetProductRepository();
            var products = productRepository.GetProducts(name);
            return products;
        }

        [Route("{id}")]
        [HttpGet]
        public Product GetProduct(Guid id)
        {
            productRepository = Helper.GetProductRepository();
            var product = productRepository.GetProduct(id);
            return product;
        }

        [Route]
        [HttpPost]
        public void Create(Product product)
        {
            productRepository = Helper.GetProductRepository();
            productRepository.AddProduct(product);
        }

        [Route]
        [HttpPut]
        public void Update(Product product)
        {
            productRepository = Helper.GetProductRepository();
            productRepository.UpdateProduct(product);
        }

        [Route("{id}")]
        [HttpDelete]
        public void Delete(Guid id)
        {
            productRepository = Helper.GetProductRepository();
            productRepository.DeleteProduct(id);
        }

        [Route("{productId}/options")]
        [HttpGet]
        public List<ProductOption> GetOptions(Guid productId)
        {
            productRepository = Helper.GetProductRepository();
            var productOptions = productRepository.GetProductOptions(productId);
            return productOptions;
        }

        [Route("{productId}/options/{id}")]
        [HttpGet]
        public ProductOption GetOption(Guid productId, Guid id)
        {
            productRepository = Helper.GetProductRepository();
            var options = productRepository.GetProductOptions(id);

            if (options.Any())
            {
                return options.First();
            }

            return null;
        }

        [Route("{productId}/options")]
        [HttpPost]
        public void CreateOption(Guid productId, ProductOption option)
        {
            option.ProductId = productId;
            productRepository = Helper.GetProductRepository();
            productRepository.AddProductOption(option);
        }

        [Route]
        [HttpPut]
        public void UpdateOption(Guid id, ProductOption option)
        {
            productRepository = Helper.GetProductRepository();
            productRepository.UpdateProductOption(option);
        }

        [Route]
        [HttpDelete]
        public void DeleteOption(Guid id)
        {
            productRepository = Helper.GetProductRepository();
            productRepository.DeleteProductOption(id);
        }
    }
}
