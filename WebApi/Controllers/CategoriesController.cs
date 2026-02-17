using Application.CQRS.ProductCatalog.Dtos;
using Application.CQRS.ProductCatalog.Queries.GetCategories;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace WebApi.Controllers
{

    [ApiController]
    [AllowAnonymous]
    [Route("api/[controller]")]    
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CategoriesController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        [AllowAnonymous]
        [EndpointName("GetAllCategories")] // Orval создаст метод getAllCategories()
        [SwaggerOperation(OperationId = "GetAllCategories", Tags = new[] { "ProductCatalog" })]
        [ProducesResponseType(typeof(List<CategoryDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> Get()
        {
            var result = await _mediator.Send(new GetCategoriesQuery());
            return Ok(result.Value);
        }
    }
}
