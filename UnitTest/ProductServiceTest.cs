using LogicService.Dto;
using LogicService.IAdapter;
using LogicService.Service;
using Microsoft.AspNetCore.Hosting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    public class ProductServiceTest
    {
        readonly Mock<IProductAdapter> mockProductAdapter = new Mock<IProductAdapter>();
        ProductService productService;
        [SetUp]
        public void Setup()
        {
            mockProductAdapter.Setup(pa => pa.AddAndChange(It.IsAny<ProductDto>())).Returns((ProductDto product) =>
            {
                product.Id = 1;
                return product;
            });
            productService = new ProductService(mockProductAdapter.Object, null, null, null, null);
        }
        [Test]
        public void CompleteProductCreationTest()
        {
            ProductDto product = new ProductDto() { Id = 0};
            var res1 = productService.CompleteProductCreation(product);
            Assert.That(res1.Id, Is.EqualTo(1));
        }
    }
}
