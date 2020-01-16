using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiDemo.DataAccess;
using WebApiDemo.Entities;

namespace WebApiDemo.Controllers
{
    [Route("api/products")]
    public class ProductsController:Controller
    {
        IProductDal _productDal;

        public ProductsController(IProductDal productDal)
        {
            _productDal = productDal;
        }

        [HttpGet("")]
        public IActionResult Get()
        {
            var products = _productDal.GetList();
            return Ok(products);
        }
        [HttpGet("{productId}")]
        public IActionResult Get(int productId)
        {
            try
            {
            var product = _productDal.Get(p=>p.ProductId==productId);
                if (product == null)
                {
                    return NotFound($"There is no product with this Id : {productId}");
                }
            return Ok(product);

            }
            catch (Exception)
            {
            }
            return BadRequest();
        }
        [HttpPost]
        public IActionResult Post(Product product)
        {
            try
            {
                _productDal.Add(product);
                return new StatusCodeResult(201);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return BadRequest();
        }
        [HttpPut]
        public IActionResult Put(Product product)
        {
            try
            {
                _productDal.Update(product);
                return Ok(product);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return BadRequest();
        }
        [HttpDelete("{productId}")]
        public IActionResult Delete(int productId)
        {
            try
            {
              _productDal.Delete(new Product() { ProductId=productId });
              return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return BadRequest();
         }
        [HttpGet("GetProductDetails")]
        public IActionResult GetProductsWithDetails()
        {
            try
            {
                var result = _productDal.GetProductsWithDetails();
                return Ok(result);
            }
            catch (Exception)
            {
            }
            return BadRequest();
        }
    }
}
