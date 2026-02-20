using Application.CQRS.ProductCatalog.Dtos;

namespace Application.CQRS.ProductCatalog.DTOs
{
    public record ProductDto(
    Guid Id,
    string Name,
    string Article,
    decimal PricePerItem,
    int Quantity,
    decimal PricePerPack,
    int ItemsPerPack,
    string Unit, // Передаем название Piece/Pair
    Guid CategoryId,
    Guid? SubcategoryId,
    List<ParameterDto> Parameters);
}
