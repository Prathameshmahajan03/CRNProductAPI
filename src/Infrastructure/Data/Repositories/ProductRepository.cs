using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    /// <summary>
    /// Provides data access operations for products using Entity Framework Core.
    /// </summary>
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductRepository"/> class.
        /// </summary>
        /// <param name="context">The application database context.</param>
        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all products from the database.
        /// </summary>
        /// <returns>A collection of products.</returns>
        public async Task<(IEnumerable<Product> Products, int TotalCount)> GetAllAsync(
            int page,
            int pageSize)
        {
            var query = _context.Products
                .AsNoTracking();

            var totalCount = await query.CountAsync();

            var products = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (products, totalCount);
        }
        /// <summary>
        /// Retrieves a product by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the product.</param>
        /// <returns>The product if found; otherwise, null.</returns>
        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        /// <summary>
        /// Adds a new product to the database.
        /// </summary>
        /// <param name="product">The product to add.</param>
        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates an existing product in the database.
        /// </summary>
        /// <param name="product">The product with updated values.</param>
        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a product from the database.
        /// </summary>
        /// <param name="product">The product to delete.</param>
        public async Task DeleteAsync(Product product)
        {
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Saves pending changes to the database.
        /// </summary>
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}