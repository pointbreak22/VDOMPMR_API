using Application.Common.Interfaces;
using Application.Common.Models;
using Application.CQRS.ProductCatalog.Dtos;
using MediatR;

namespace Application.CQRS.ProductCatalog.Queries.GetCategories
{
    public class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, Result<List<CategoryDto>>>
    {
        private readonly IProductCategoryRepository _repository;

        public GetCategoriesHandler(IProductCategoryRepository repository) => _repository = repository;

        public async Task<Result<List<CategoryDto>>> Handle(GetCategoriesQuery request, CancellationToken ct)
        {
            // 1. Получаем данные через репозиторий (Infrastructure)
            var categories = await _repository.GetCategoriesWithDetailsAsync(ct);

            // 2. Маппим сущности в DTO (Application)
            var dtos = categories.Select(c => new CategoryDto(
                c.Id,
                c.Name,
                c.Products.SelectMany(p => p.Parameters)
                    .Select(param => new ParameterDto(
                        param.Id,
                        param.Type.ToString(),
                        param.Value,
                        param.Stock))
                    .ToList()
            )).ToList();

            return Result<List<CategoryDto>>.Success(dtos);
        }
    }
}
