using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using refactor_me.Helpers;
using refactor_me.Models;

namespace refactor_me.Repositories
{
    public class ProductRepository
    {
        private static ProductRepository _instance = null;

        private ProductRepository()
        {

        }

        public static ProductRepository Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ProductRepository();
                }

                return _instance;
            }
        }

        public List<Product> GetProducts(string name)
        {
            using (var conn = Helper.NewConnection())
            {
                var products = new List<Product>();

                var cmd = new SqlCommand($"select id, name, Description, Price, DeliveryPrice from product", conn);

                if (!string.IsNullOrEmpty(name))
                {
                    cmd = new SqlCommand($"select id from product where lower(name) like '%{name.ToLower()}%'", conn);
                }

                conn.Open();

                var rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    var id = Guid.Parse(rdr["id"].ToString());
                    var productName = rdr["name"]?.ToString() ?? "";

                    var description = (DBNull.Value == rdr["Description"]) ? null : rdr["Description"].ToString();
                    var price = decimal.Parse(rdr["Price"].ToString());
                    var deliveryPrice = decimal.Parse(rdr["DeliveryPrice"].ToString());
                    products.Add(new Product
                    {
                        Id = id,
                        Name = productName,
                        Description = description,
                        Price = price,
                        DeliveryPrice = deliveryPrice
                    });
                }

                return products;
            }
        }

        public Product GetProduct(Guid id)
        {
            using (var conn = Helper.NewConnection())
            {
                var cmd = new SqlCommand($"select * from product where id = '{id}'", conn);
                conn.Open();

                var rdr = cmd.ExecuteReader();
                if (!rdr.Read())
                    return null;

                return new Product
                {
                    Id = Guid.Parse(rdr["Id"].ToString()),
                    Name = rdr["Name"].ToString(),
                    Description = (DBNull.Value == rdr["Description"]) ? null : rdr["Description"].ToString(),
                    Price = decimal.Parse(rdr["Price"].ToString()),
                    DeliveryPrice = decimal.Parse(rdr["DeliveryPrice"].ToString()),
                };
            }
        }

        public void AddProduct(Product product)
        {
            using (var conn = Helper.NewConnection())
            {
                var cmd = new SqlCommand(
                    $"insert into product (id, name, description, price, deliveryprice) values ('{product.Id}', '{product.Name}', '{product.Description}', {product.Price}, {product.DeliveryPrice})",
                    conn);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateProduct(Product product)
        {
            using (var conn = Helper.NewConnection())
            {
                var cmd = new SqlCommand(
                    $"update product set name = '{product.Name}', description = '{product.Description}', price = {product.Price}, deliveryprice = {product.DeliveryPrice} where id = '{product.Id}'",
                    conn);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteProduct(Guid id)
        {
            var productOptions = GetProductOptions(id);
            foreach (var option in productOptions)
                DeleteProductOption(option.Id);

            using (var conn = Helper.NewConnection())
            {
                conn.Open();
                var cmd = new SqlCommand($"delete from product where id = '{id}'", conn);
                cmd.ExecuteNonQuery();
            }
        }

        public void AddProductOption(ProductOption productOption)
        {
            using (var conn = Helper.NewConnection())
            {
                var cmd = new SqlCommand(
                    $"insert into productoption (id, productid, name, description) values ('{productOption.Id}', '{productOption.ProductId}', '{productOption.Name}', '{productOption.Description}')",
                    conn);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateProductOption(ProductOption productOption)
        {
            using (var conn = Helper.NewConnection())
            {
                var cmd = new SqlCommand(
                    $"update productoption set name = '{productOption.Name}', description = '{productOption.Description}' where id = '{productOption.Id}'",
                    conn);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteProductOption(Guid id)
        {
            using (var conn = Helper.NewConnection())
            {
                conn.Open();
                var cmd = new SqlCommand($"delete from productoption where id = '{id}'", conn);
                cmd.ExecuteReader();
            }
        }

        public List<ProductOption> GetProductOptions(Guid id)
        {
            using (var conn = Helper.NewConnection())
            {
                var cmd = new SqlCommand($"select * from productoption where id = '{id}'", conn);
                conn.Open();
                var productOptions = new List<ProductOption>();

                var rdr = cmd.ExecuteReader();
                if (!rdr.Read())
                    return productOptions;

                productOptions.Add(new ProductOption
                {
                    Id = Guid.Parse(rdr["Id"].ToString()),
                    ProductId = Guid.Parse(rdr["ProductId"].ToString()),
                    Name = rdr["Name"].ToString(),
                    Description = (DBNull.Value == rdr["Description"]) ? null : rdr["Description"].ToString()
                });

                return productOptions;
            }
        }

    }
}