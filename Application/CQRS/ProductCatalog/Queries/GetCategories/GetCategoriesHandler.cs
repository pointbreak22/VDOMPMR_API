using Application.Common.Interfaces;
using Application.Common.Models;
using Application.CQRS.ProductCatalog.Dtos;
using Domain.Entities;
using MediatR;

namespace Application.CQRS.ProductCatalog.Queries.GetCategories
{
    public class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, Result<List<CategoryDto>>>
    {
        private readonly ICategoryRepository _repository;

        public GetCategoriesHandler(ICategoryRepository repository) => _repository = repository;

        public async Task<Result<List<CategoryDto>>> Handle(GetCategoriesQuery request, CancellationToken ct)
        {
            var categories = await _repository.GetCategoriesWithDetailsAsync(ct);

            var tree = BuildTree(categories);

            return Result<List<CategoryDto>>.Success(tree);
        }

        private List<CategoryDto> BuildTree(List<Category> categories, Guid? parentId = null)
        {
            return categories
                .Where(c => c.ParentId == parentId)
                .Select(c => new CategoryDto(
                    c.Id,
                    c.Name,
                    c.Products.SelectMany(p => p.Parameters)
                        .Select(param => new ParameterDto(
                            param.Id,
                            param.Type.ToString(),
                            param.Value,
                            param.Stock))
                        .ToList(),
                    BuildTree(categories, c.Id)
                ))
                .ToList();
        }
    }
}
