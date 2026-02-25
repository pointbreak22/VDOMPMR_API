using Application.Common.Interfaces;
using Application.Common.Models;
using Application.CQRS.ProductCatalog.Dtos;
using Application.CQRS.ProductCatalog.DTOs;
using MediatR;

namespace Application.CQRS.ProductCatalog.Queries.GetProducts
{
    public class GetPagedProductsHandler : IRequestHandler<GetPagedProductsQuery, Result<PaginatedList<ProductDto>>>
    {
        private readonly IProductRepository _productRepository;

        public GetPagedProductsHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result<PaginatedList<ProductDto>>> Handle(GetPagedProductsQuery request, CancellationToken ct)
        {
            // Получаем кортеж (Items, TotalCount) из репозитория
            var result = await _productRepository.GetPagedProductsAsync(request.PageNumber, request.PageSize, ct);

            var items = result.Items;
            var totalCount = result.TotalCount;

            // Маппим сущности Domain в DTO
            var dtos = items.Select(p => new ProductDto(
                p.Id,
                p.Name,
                p.Article,
                p.PricePerItem,
                p.Quantity,
                p.PricePerPack,
                p.ItemsPerPack,
                p.Unit.ToString(),
                p.CategoryId,
          
                p.Parameters.Select(param => new ParameterDto(
                    param.Id,
                    param.Type.ToString(),
                    param.Value,
                    param.Stock)).ToList()
            )).ToList();

            // Создаем пагинированный список для Orval
            var paginatedResult = new PaginatedList<ProductDto>(
                dtos,
                totalCount,
                request.PageNumber,
                request.PageSize);

            return Result<PaginatedList<ProductDto>>.Success(paginatedResult);
        }
    }
}
