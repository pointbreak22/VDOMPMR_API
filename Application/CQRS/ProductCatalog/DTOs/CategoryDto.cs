namespace Application.CQRS.ProductCatalog.Dtos
{
    public record CategoryDto(Guid Id, string Name, List<ParameterDto> Parameters);
}
