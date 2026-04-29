using System.Text;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces.Repositories;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ExportController(ISaleRepository saleRepo, IProductRepository productRepo) : ControllerBase
{
    [HttpGet("sales/csv")]
    public async Task<IActionResult> ExportSalesCsv()
    {
        var sales = await saleRepo.GetSalesWithItemsAsync();
        var sb = new StringBuilder();
        sb.AppendLine("Id,Fecha,Cliente,Canal,Total,Ganancia");

        foreach (var sale in sales)
        {
            var profit = sale.Items.Sum(i => i.Profit);
            sb.AppendLine($"{sale.Id},{sale.SaleDate:yyyy-MM-dd},{sale.CustomerName},{sale.Channel},{sale.TotalAmount},{profit}");
        }

        return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "ventas.csv");
    }

    [HttpGet("sales/excel")]
    public async Task<IActionResult> ExportSalesExcel()
    {
        var sales = await saleRepo.GetSalesWithItemsAsync();

        using var workbook = new XLWorkbook();
        var ws = workbook.Worksheets.Add("Ventas");

        ws.Cell(1, 1).Value = "ID";
        ws.Cell(1, 2).Value = "Fecha";
        ws.Cell(1, 3).Value = "Cliente";
        ws.Cell(1, 4).Value = "Canal";
        ws.Cell(1, 5).Value = "Total";
        ws.Cell(1, 6).Value = "Ganancia";

        int row = 2;
        foreach (var sale in sales)
        {
            ws.Cell(row, 1).Value = sale.Id;
            ws.Cell(row, 2).Value = sale.SaleDate.ToString("yyyy-MM-dd");
            ws.Cell(row, 3).Value = sale.CustomerName;
            ws.Cell(row, 4).Value = sale.Channel.ToString();
            ws.Cell(row, 5).Value = sale.TotalAmount;
            ws.Cell(row, 6).Value = sale.Items.Sum(i => i.Profit);
            row++;
        }

        ws.Columns().AdjustToContents();

        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return File(stream.ToArray(),
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "ventas.xlsx");
    }

    [HttpGet("products/csv")]
    public async Task<IActionResult> ExportProductsCsv()
    {
        var products = await productRepo.GetAllAsync();
        var sb = new StringBuilder();
        sb.AppendLine("Id,Nombre,Precio,Costo,Stock,Margen%,Activo");

        foreach (var p in products)
            sb.AppendLine($"{p.Id},{p.Name},{p.Price},{p.Cost},{p.Stock},{p.ProfitMargin:F1},{p.IsActive}");

        return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/csv", "productos.csv");
    }
}