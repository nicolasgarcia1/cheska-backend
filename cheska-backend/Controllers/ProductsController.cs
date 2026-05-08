using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs.Products;
using Application.UseCases.Products;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(
    GetProductsUseCase getProducts,
    GetProductByIdUseCase getProductById,
    CreateProductUseCase createProduct,
    UpdateProductUseCase updateProduct,
    ReplenishProductStockUseCase replenishProductStock,
    DeleteProductUseCase deleteProduct) : ControllerBase
{
    // Público
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await getProducts.ExecuteAsync(isAdmin: false);
        return Ok(result.Data);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await getProductById.ExecuteAsync(id, isAdmin: false);
        if (!result.IsSuccess) return NotFound(new { message = result.Error });
        return Ok(result.Data);
    }

    // Admin
    [HttpGet("admin")]
    [Authorize]
    public async Task<IActionResult> GetAllAdmin()
    {
        var result = await getProducts.ExecuteAsync(isAdmin: true);
        return Ok(result.Data);
    }

    [HttpPost]
    [Authorize]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Create([FromForm] CreateProductDto dto)
    {
        var result = await createProduct.ExecuteAsync(dto);
        if (!result.IsSuccess) return BadRequest(new { message = result.Error });
        return CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data);
    }

    [HttpPut("{id}")]
    [Authorize]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Update(int id, [FromForm] UpdateProductDto dto)
    {
        dto.Id = id;
        var result = await updateProduct.ExecuteAsync(dto);
        if (!result.IsSuccess) return BadRequest(new { message = result.Error });
        return Ok(result.Data);
    }

    [HttpPatch("{id}/stock/replenish")]
    [Authorize]
    public async Task<IActionResult> ReplenishStock(int id, [FromBody] ReplenishProductStockDto dto)
    {
        var result = await replenishProductStock.ExecuteAsync(id, dto);
        if (!result.IsSuccess) return BadRequest(new { message = result.Error });
        return Ok(result.Data);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await deleteProduct.ExecuteAsync(id);
        if (!result.IsSuccess) return NotFound(new { message = result.Error });
        return NoContent();
    }
}
