using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.ProductCatalog.Dtos
{
    public record CategoryDto(Guid Id, string Name, List<ParameterDto> Parameters);
}
