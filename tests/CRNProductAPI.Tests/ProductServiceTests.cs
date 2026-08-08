using Application.DTOs;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Moq;
using Xunit;

namespace CRNProductAPI.Tests
{
    public class ProductServiceTests
    {

        private readonly Mock<IProductRepository> _repositoryMock;

        public ProductServiceTests()
        {
            _repositoryMock = new Mock<IProductRepository>();
        }

        [Fact]
        public async Task GetAllProductsAsync_ShouldReturnAllProducts()
        {

            // Arrange
            var products = new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Name = "Laptop",
                    Description = "Gaming Laptop",
                    Price = 75000,
                    StockQuantity = 10,
                    IsActive = true
                },

                new Product
                {
                    Id = 2,
                    Name = "Mouse",
                    Description = "Wireless Mouse",
                    Price = 1500,
                    StockQuantity = 25,
                    IsActive = true
                }
            };

            _repositoryMock
                .Setup(r => r.GetAllAsync())
                 .ReturnsAsync(products);

            // Act
            var service = new ProductService(_repositoryMock.Object);

            var result = await service.GetAllProductsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());

        }


        [Fact]
        public async Task CreateProductAsync_ShouldCreateProductSuccessfully()
        {

            // Arrange
            var createDto = new CreateProductDto
            {
                Name = "Keyboard",
                Description = "Mechanical Keyboard",
                Price = 3500,
                StockQuantity = 15
            };

            _repositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Product>()))
                .Returns(Task.CompletedTask);


            // Act
            var service = new ProductService(_repositoryMock.Object);

            var result = await service.CreateProductAsync(createDto);


            // Assert
            Assert.NotNull(result);
            Assert.Equal("Keyboard", result.Name);
            Assert.Equal("Mechanical Keyboard", result.Description);
            Assert.Equal(3500, result.Price);
            Assert.Equal(15, result.StockQuantity);
            Assert.True(result.IsActive);

        }


        [Fact]
        public async Task GetProductByIdAsync_ShouldReturnProduct_WhenProductExists()
        {

            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Laptop",
                Description = "Gaming Laptop",
                Price = 75000,
                StockQuantity = 10,
                IsActive = true
            };


            _repositoryMock
                .Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(product);

            // Act
            var service = new ProductService(_repositoryMock.Object);

            var result = await service.GetProductByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("Laptop", result.Name);
            Assert.Equal("Gaming Laptop", result.Description);
            Assert.Equal(75000, result.Price);
            Assert.Equal(10, result.StockQuantity);
            Assert.True(result.IsActive);

        }


        [Fact]
        public async Task UpdateProductAsync_ShouldReturnTrue_WhenProductExists()
        {

            // Arrange
            var updateDto = new UpdateProductDto
            {
                Id = 1,
                Name = "Updated Laptop",
                Description = "Updated Description",
                Price = 80000,
                StockQuantity = 20,
                IsActive = true
            };

            var existingProduct = new Product
            {
                Id = 1,
                Name = "Laptop",
                Description = "Gaming Laptop",
                Price = 75000,
                StockQuantity = 10,
                IsActive = true
            };

            _repositoryMock
                .Setup(r => r.GetByIdAsync(updateDto.Id))
                .ReturnsAsync(existingProduct);

            _repositoryMock
                .Setup(r => r.UpdateAsync(It.IsAny<Product>()))
                .Returns(Task.CompletedTask);


            // Act
            var service = new ProductService(_repositoryMock.Object);

            var result = await service.UpdateProductAsync(updateDto);


            // Assert
            Assert.True(result);

            _repositoryMock.Verify(
                r => r.UpdateAsync(It.IsAny<Product>()),
                Times.Once);

        }

        [Fact]
        public async Task DeleteProductAsync_ShouldReturnTrue_WhenProductExists()
        {

            // Arrange
            int productId = 1;

            var existingProduct = new Product
            {
                Id = 1,
                Name = "Laptop",
                Description = "Gaming Laptop",
                Price = 75000,
                StockQuantity = 10,
                IsActive = true
            };

            _repositoryMock
                .Setup(r => r.GetByIdAsync(productId))
                .ReturnsAsync(existingProduct);

            _repositoryMock
                .Setup(r => r.DeleteAsync(It.IsAny<Product>()))
                .Returns(Task.CompletedTask);

            // Act
            var service = new ProductService(_repositoryMock.Object);

            var result = await service.DeleteProductAsync(productId);

            // Assert
            Assert.True(result);

            _repositoryMock.Verify(
                r => r.DeleteAsync(It.IsAny<Product>()),
                Times.Once);

        }

    }
}