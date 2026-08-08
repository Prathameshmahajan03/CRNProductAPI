using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;

namespace Application.Services
{
    /// <summary>
    /// Provides business logic for product management operations.
    /// </summary>
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductService"/> class.
        /// </summary>
        /// <param name="repository">Product repository used for data access.</param>
        public ProductService(IProductRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Retrieves all products from the repository.
        /// </summary>
        /// <returns>A collection of product response DTOs.</returns>
        public async Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync()
        {
            var products = await _repository.GetAllAsync();

            return products.Select(p => new ProductResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                IsActive = p.IsActive
            });
        }

        /// <summary>
        /// Retrieves a product by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the product.</param>
        /// <returns>
        /// The product response DTO if found; otherwise, null.
        /// </returns>
        public async Task<ProductResponseDto?> GetProductByIdAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);

            if (product == null)
                return null;

            return new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                IsActive = product.IsActive
            };
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="dto">The product creation details.</param>
        /// <returns>The newly created product response DTO.</returns>
        public async Task<ProductResponseDto> CreateProductAsync(CreateProductDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                StockQuantity = dto.StockQuantity,
                IsActive = true
            };

            await _repository.AddAsync(product);

            return new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                IsActive = product.IsActive
            };
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="dto">The updated product details.</param>
        /// <returns>
        /// True if the product was successfully updated; otherwise, false.
        /// </returns>
        public async Task<bool> UpdateProductAsync(UpdateProductDto dto)
        {
            var product = await _repository.GetByIdAsync(dto.Id);

            if (product == null)
                return false;

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.Price = dto.Price;
            product.StockQuantity = dto.StockQuantity;
            product.IsActive = dto.IsActive;

            await _repository.UpdateAsync(product);

            return true;
        }

        /// <summary>
        /// Deletes a product by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the product.</param>
        /// <returns>
        /// True if the product was successfully deleted; otherwise, false.
        /// </returns>
        public async Task<bool> DeleteProductAsync(int id)
        {
            var product = await _repository.GetByIdAsync(id);

            if (product == null)
                return false;

            await _repository.DeleteAsync(product);

            return true;
        }
    }
}