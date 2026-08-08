using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResponseDto>> GetAllProductsAsync();

        Task<ProductResponseDto?> GetProductByIdAsync(int id);

        Task<ProductResponseDto> CreateProductAsync(CreateProductDto dto);

        Task<bool> UpdateProductAsync(UpdateProductDto dto);

        Task<bool> DeleteProductAsync(int id);

    }
}
