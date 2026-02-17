using Application.Common.Models;
using Application.CQRS.ProductCatalog.DTOs;
using Application.CQRS.ProductCatalog.Queries.GetProducts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi.Controllers
{
    [ApiController]
    [AllowAnonymous]         
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProductsController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        [AllowAnonymous]
        [SwaggerOperation(OperationId = "GetPagedProducts", Tags = new[] { "ProductCatalog" })]
        [ProducesResponseType(typeof(PaginatedList<ProductDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPaged([FromQuery] GetPagedProductsQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result.Value);
        }
    }
}
