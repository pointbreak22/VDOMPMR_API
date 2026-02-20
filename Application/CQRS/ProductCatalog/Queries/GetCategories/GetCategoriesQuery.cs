using Application.Common.Models;
using Application.CQRS.ProductCatalog.Dtos;
using MediatR;

namespace Application.CQRS.ProductCatalog.Queries.GetCategories
{
    public record GetCategoriesQuery : IRequest<Result<List<CategoryDto>>>;
}
