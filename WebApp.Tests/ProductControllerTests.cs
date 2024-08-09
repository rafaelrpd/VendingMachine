using Bogus;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.Controllers;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Tests
{
    public class ProductControllerTests
    {
        #region Properties
        private readonly WebAppContext _context;
        private readonly ProductsController _controller;
        #endregion Properties

        #region Constructor
        public ProductControllerTests()
        {
            var options = new DbContextOptionsBuilder<WebAppContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _context = new WebAppContext(options);
            _context.Product.RemoveRange(_context.Product);
            _context.SaveChanges();

            _controller = new ProductsController(_context);
        }
        #endregion Constructor

        #region Routes
        #region Index
        [Fact]
        public async Task Index_ReturnsAViewResult_WithAListOfProducts()
        {
            // Arrange
            _context.Product.AddRange(GetFakeProducts(5));
            _context.SaveChanges();

            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Product>>(viewResult.ViewData.Model);
            Assert.Equal(5, model.Count());
        }

        [Fact]
        public async Task Index_ReturnsViewResult_WithEmptyList()
        {
            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Product>>(viewResult.ViewData.Model);
            Assert.Empty(model);
        }

        [Fact]
        public async Task Index_SetViewDataTitleCorrectly()
        {
            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Products", viewResult.ViewData["Title"]);
        }
        #endregion Index

        #region Details
        [Fact]
        public async Task Details_ReturnViewResult_WithProduct()
        {
            // Arrange
            var product = GetFakeProducts(1).FirstOrDefault();
            _context.Product.Add(product);
            _context.SaveChanges();

            // Act
            var result = await _controller.Details(product.Id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Product>(viewResult.ViewData.Model);
            Assert.Equal(product.Id, model.Id);
        }

        [Fact]
        public async Task Details_ReturnsNotFound_ForInvalidId()
        {
            // Act
            var result = await _controller.Details(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsNotFound_WhenIdIsNull()
        {
            // Act
            var result = await _controller.Details(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_SetViewDataTitleCorrectly()
        {
            // Arrange
            var product = GetFakeProducts(1).FirstOrDefault();
            _context.Product.Add(product);
            _context.SaveChanges();

            // Act
            var result = await _controller.Details(product.Id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal($"Details of {product.Name}", viewResult.ViewData["Title"]);
        }

        [Fact]
        public async Task Details_ReturnsNotFound_WhenProductIsNull()
        {
            // Arrange
            var product = GetFakeProducts(1).FirstOrDefault();
            _context.Product.Add(product);
            _context.SaveChanges();

            _context.Product.Remove(product);
            _context.SaveChanges();

            // Act
            var result = await _controller.Details(product.Id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
        #endregion Details

        #region Create - GET
        [Fact]
        public void Create_ReturnsViewResult()
        {
            // Act
            var result = _controller.Create();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
        }

        [Fact]
        public void Create_SetsViewDataTitleCorrectly()
        {
            // Act
            var result = _controller.Create();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Create Product", viewResult.ViewData["Title"]);
        }

        [Fact]
        public void Create_Get_IgnoresModelStateErrors()
        {
            // Arrange
            _controller.ModelState.AddModelError("Name", "Error should not affect GET");

            // Act
            var result = _controller.Create();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
        }
        #endregion Create - GET

        #region Create - POST
        [Fact]
        public async Task Create_Post_ValidProduct_RedirectsToIndex()
        {
            // Arrange
            var product = GetFakeProducts(1).First();

            // Act
            var result = await _controller.Create(product);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal(1, _context.Product.Count());
        }

        [Fact]
        public async Task Create_Post_InvalidProduct_ReturnsViewWithModel()
        {
            // Arrange
            var product = GetFakeProducts(1).First();
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _controller.Create(product);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Product>(viewResult.ViewData.Model);
            Assert.Equal(product, model);
        }

        [Fact]
        public async Task Create_Post_InvalidModelState_DoesNotAddProduct()
        {
            // Arrange
            var product = GetFakeProducts(1).First();
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _controller.Create(product);

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.Equal(0, _context.Product.Count());
        }

        [Fact]
        public async Task Create_Post_ModelBindingFailure_ReturnsViewWithModel()
        {
            // Arrange
            var product = new Product();
            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _controller.Create(product);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Product>(viewResult.ViewData.Model);
            Assert.Equal(product, model);
        }

        [Fact]
        public async Task Create_Post_DoesNotUpdateExcludedProperties()
        {
            // Arrange
            var product = GetFakeProducts(1).First();
            var initialDate = new DateTime(2000, 1, 1);
            product.CreatedDate = initialDate;

            // Act
            var result = await _controller.Create(product);

            // Assert
            var savedProduct = _context.Product.First();
            Assert.NotEqual(initialDate, savedProduct.CreatedDate);
            Assert.Equal(DateTime.Now.Date, savedProduct.CreatedDate.Date);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Create_Post_SetsViewDataTitleAndLoadsCorrectView(bool isModelValid)
        {
            var product = GetFakeProducts(1).First();

            if (!isModelValid)
            {
                _controller.ModelState.AddModelError("Name", "Required");
            }

            var result = await _controller.Create(product);

            if (isModelValid)
            {
                var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
                Assert.Equal("Index", redirectToActionResult.ActionName);
            }
            else
            {
                var viewResult = Assert.IsType<ViewResult>(result);
                Assert.Equal("Create Product", viewResult.ViewData["Title"]);
                Assert.Null(viewResult.ViewName);
            }
        }
        #endregion Create - POST

        #region Edit - GET
        [Fact]
        public async Task Edit_ReturnsNotFound_WhenIdIsNull()
        {
            // Act
            var result = await _controller.Edit(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ReturnsNotFound_WhenProductNotFound()
        {
            // Act
            var result = await _controller.Edit(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_ReturnsViewResult_WithProduct()
        {
            // Arrange
            var product = GetFakeProducts(1).First();
            _context.Product.Add(product);
            _context.SaveChanges();

            // Act
            var result = await _controller.Edit(product.Id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Product>(viewResult.ViewData.Model);
            Assert.Equal(product.Id, model.Id);
            Assert.Equal(product.CreatedDate, model.CreatedDate);
        }

        [Fact]
        public async Task Edit_SetsViewDataTitleCorrectly()
        {
            // Arrange
            var product = GetFakeProducts(1).First();
            _context.Product.Add(product);
            _context.SaveChanges();

            // Act
            var result = await _controller.Edit(product.Id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal($"Edit Product - {product.Name}", viewResult.ViewData["Title"]);
        }
        #endregion Edit - GET

        #region Edit - POST
        [Fact]
        public async Task Edit_Post_ValidProduct_RedirectsToIndex()
        {
            // Arrange
            var product = GetFakeProducts(1).First();
            _context.Product.Add(product);
            _context.SaveChanges();

            var productToUpdate = _context.Product.Find(product.Id);
            productToUpdate.Name = "Updated Name";

            // Act
            var result = await _controller.Edit(product.Id, productToUpdate);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);

            var savedProduct = _context.Product.Find(product.Id);
            Assert.Equal("Updated Name", savedProduct.Name); 
            Assert.Equal(product.CreatedDate, savedProduct.CreatedDate); 
        }


        [Fact]
        public async Task Edit_Post_ReturnsNotFound_WhenIdsDoNotMatch()
        {
            // Arrange
            var product = GetFakeProducts(1).First();
            _context.Product.Add(product);
            _context.SaveChanges();

            var updatedProduct = new Product
            {
                Id = product.Id + 1,
                Name = product.Name,
                Price = product.Price,
                Quantity = product.Quantity,
                CreatedDate = product.CreatedDate
            };

            // Act
            var result = await _controller.Edit(product.Id, updatedProduct);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_Post_ReturnsNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            var product = GetFakeProducts(1).First();
            _context.Product.Add(product);
            _context.SaveChanges();

            var updatedProduct = new Product
            {
                Id = product.Id,
                Name = "Updated Name",
                Price = product.Price,
                Quantity = product.Quantity,
                CreatedDate = product.CreatedDate
            };

            _context.Product.Remove(product);
            _context.SaveChanges();

            // Act
            var result = await _controller.Edit(product.Id, updatedProduct);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_Post_ReturnsViewResult_WhenModelStateIsInvalid()
        {
            // Arrange
            var product = GetFakeProducts(1).First();
            _context.Product.Add(product);
            _context.SaveChanges();

            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _controller.Edit(product.Id, product);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Product>(viewResult.ViewData.Model);
            Assert.Equal(product, model);
        }

        [Fact]
        public async Task Edit_Post_DoesNotUpdateExcludedProperties()
        {
            // Arrange
            var product = GetFakeProducts(1).First();
            _context.Product.Add(product);
            _context.SaveChanges();

            var productToUpdate = _context.Product.Find(product.Id);

            productToUpdate.Name = "Updated Name";
            productToUpdate.Price = product.Price;
            productToUpdate.Quantity = product.Quantity;

            var originalCreatedDate = product.CreatedDate;

            // Act
            var result = await _controller.Edit(product.Id, productToUpdate);

            // Assert
            var savedProduct = _context.Product.Find(product.Id);
            Assert.Equal(originalCreatedDate, savedProduct.CreatedDate);
        }


        [Fact]
        public async Task Edit_Post_SetsViewDataTitleCorrectly_WhenModelStateIsInvalid()
        {
            // Arrange
            var product = GetFakeProducts(1).First();
            _context.Product.Add(product);
            _context.SaveChanges();

            _controller.ModelState.AddModelError("Name", "Required");

            // Act
            var result = await _controller.Edit(product.Id, product);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal($"Edit Product - {product.Name}", viewResult.ViewData["Title"]);
        }
        #endregion Edit - POST

        #region Delete - GET
        [Fact]
        public async Task Delete_ReturnsNotFound_WhenIdIsNull()
        {
            // Act
            var result = await _controller.Delete(null);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenProductDoesNotExist()
        {
            // Act
            var result = await _controller.Delete(999);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsNotFound_WhenProductIsDeleted()
        {
            // Arrange
            var product = GetFakeProducts(1).First();
            product.IsDeleted = true;
            _context.Product.Add(product);
            _context.SaveChanges();

            // Act
            var result = await _controller.Delete(product.Id);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_ReturnsViewResult_WithProduct()
        {
            // Arrange
            var product = GetFakeProducts(1).First();
            _context.Product.Add(product);
            _context.SaveChanges();

            // Act
            var result = await _controller.Delete(product.Id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Product>(viewResult.ViewData.Model);
            Assert.Equal(product.Id, model.Id);
        }

        [Fact]
        public async Task Delete_SetsViewDataTitleCorrectly()
        {
            // Arrange
            var product = GetFakeProducts(1).First();
            _context.Product.Add(product);
            _context.SaveChanges();

            // Act
            var result = await _controller.Delete(product.Id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal($"Delete Product - {product.Name}", viewResult.ViewData["Title"]);
        }
        #endregion Delete - GET

        #region Delete - POST
        [Fact]
        public async Task DeleteConfirmed_MarksProductAsDeleted_WhenIdIsValid()
        {
            // Arrange
            var product = GetFakeProducts(1).First();
            _context.Product.Add(product);
            _context.SaveChanges();

            // Act
            var result = await _controller.DeleteConfirmed(product.Id);

            // Assert
            var deletedProduct = _context.Product.Find(product.Id);
            Assert.True(deletedProduct.IsDeleted);
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", ((RedirectToActionResult)result).ActionName);
        }

        [Fact]
        public async Task DeleteConfirmed_RedirectsToIndex_AfterSoftDelete()
        {
            // Arrange
            var product = GetFakeProducts(1).First();
            _context.Product.Add(product);
            _context.SaveChanges();

            // Act
            var result = await _controller.DeleteConfirmed(product.Id);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
        }

        [Fact]
        public async Task DeleteConfirmed_DoesNothing_WhenProductNotFound()
        {
            // Act
            var result = await _controller.DeleteConfirmed(999);

            // Assert
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", ((RedirectToActionResult)result).ActionName);
        }


        #endregion Delete - POST
        #endregion Routes

        #region Private Methods
        private List<Product> GetFakeProducts(int count)
        {
            var faker = new Faker<Product>()
                .RuleFor(p => p.Id, f => f.IndexFaker + 1)
                .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                .RuleFor(p => p.Price, f => decimal.Parse(f.Commerce.Price(1, 15, 2)))
                .RuleFor(p => p.Quantity, f => f.Random.Int(1, 100))
                .RuleFor(p => p.CreatedDate, f => DateTime.Now);

            return faker.Generate(count);
        }
        #endregion Private Methods
    }
}
