using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    /// <summary>
    /// Provides RESTful endpoints for managing products.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductsController"/> class.
        /// </summary>
        /// <param name="service">Product service used to perform product operations.</param>
        public ProductsController(IProductService service)
        {
            _service = service;
        }

        /// <summary>
        /// Retrieves a paginated list of products.
        /// </summary>
        /// <param name="page">The page number. Default is 1.</param>
        /// <param name="pageSize">The number of products per page. Default is 10.</param>
        /// <returns>A paginated list of products.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (page < 1)
                return BadRequest("Page must be greater than 0.");

            if (pageSize < 1 || pageSize > 100)
                return BadRequest("PageSize must be between 1 and 100.");

            var (products, totalCount) =
                await _service.GetAllProductsAsync(page, pageSize);

            return Ok(new
            {
                page,
                pageSize,
                totalCount,
                totalPages = (int)Math.Ceiling((double)totalCount / pageSize),
                products
            });
        }

        /// <summary>
        /// Retrieves a product by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the product.</param>
        /// <returns>The requested product if found; otherwise, a 404 response.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _service.GetProductByIdAsync(id);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="dto">The product details.</param>
        /// <returns>The newly created product.</returns>
        [HttpPost]
        public async Task<IActionResult> Create(CreateProductDto dto)
        {
            var product = await _service.CreateProductAsync(dto);

            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        /// <summary>
        /// Updates an existing product.
        /// </summary>
        /// <param name="dto">The updated product details.</param>
        /// <returns>No content if the product was updated successfully; otherwise, a 404 response.</returns>
        [HttpPut]
        public async Task<IActionResult> Update(UpdateProductDto dto)
        {
            var updated = await _service.UpdateProductAsync(dto);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        /// <summary>
        /// Deletes a product by its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the product.</param>
        /// <returns>No content if the product was deleted successfully; otherwise, a 404 response.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteProductAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}