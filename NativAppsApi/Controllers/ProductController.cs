
using Microsoft.AspNetCore.Mvc;
using NativApps.Core.Models;
using NativApps.Core.Services;
using ProductDto = NativApps.Core.Models.Dto.ProductDto;

#if IncludeAuthorization
using Microsoft.AspNetCore.Authorization;
#endif

namespace NativAppsApi.Controllers;

[ApiController]
[Route("[controller]")]
#if IncludeAuthorization
[Authorize]
#endif
public class ProductsController : ControllerBase
{
    private readonly IProductService service;

    public ProductsController(IProductService service)
    {
        this.service = service;
    }

    [HttpPost]
    public ActionResult<int> Post([FromBody] ProductDto product)
    {
        return service.Create(product) is { Success: true } result
            ? (ActionResult<int>) result.Result
            : BadRequest();
    }

    [HttpPatch("{productId}")]
    public ActionResult Patch(int productId, [FromBody] ProductDto product)
    {
        var readResult = service.Read(productId);
        if (!readResult.Success) return NotFound();
        
        return service.Update(productId, product) is { Success: true }
            ? Ok()
            : BadRequest();
    }

    [HttpGet("Search")]
    public ActionResult<Product[]> Search([FromQuery] string? name, [FromQuery] int? minPrice, [FromQuery] int? maxPrice)
    {
        if (name is not null)
        {
            return service.GetByName(name).ToArray();
        }
        if (minPrice.HasValue || maxPrice.HasValue)
        {
            return service.GetByPriceRange(minPrice ?? 0m, maxPrice ?? decimal.MaxValue).ToArray();
        }
        return BadRequest();
    }

    [HttpGet("All")]
    public ActionResult<Product[]> All([FromQuery] int? page, [FromQuery] int? itemsPerPage)
    {
        if (page.HasValue || itemsPerPage.HasValue)
        {
            return service.GetAll().Skip(((page ?? 1) - 1) * (itemsPerPage ?? 10)).Take(itemsPerPage ?? 10).ToArray();
        }
        else
        {
            return service.GetAll().ToArray();
        }
    }

    [HttpGet("{productId}")]
    public ActionResult<Product> Get(int productId)
    {
        return service.Read(productId) is { Success: true, Result: { } product } ? new ActionResult<Product>(product) : NotFound();
    }


    [HttpDelete("{productId}")]
    public ActionResult Delete(int productId)
    {
        return service.Delete(productId) is { Success: true } ? Ok() : NotFound();
    }
}