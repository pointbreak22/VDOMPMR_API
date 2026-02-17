using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.ProductCatalog.Dtos
{
    public record ParameterDto(Guid Id, string Type, string Value, int Stock);
}
