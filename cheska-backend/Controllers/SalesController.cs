using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.DTOs.Sales;
using Application.Interfaces.Repositories;
using Application.UseCases.Sales;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SalesController(CreateSaleUseCase createSale, ISaleRepository saleRepo) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var sales = await saleRepo.GetSalesWithItemsAsync();
        var dtos = sales.Select(CreateSaleUseCase.MapToDto);
        return Ok(dtos);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSaleDto dto)
    {
        var result = await createSale.ExecuteAsync(dto);
        if (!result.IsSuccess) return BadRequest(new { message = result.Error });
        return CreatedAtAction(nameof(GetAll), result.Data);
    }
}