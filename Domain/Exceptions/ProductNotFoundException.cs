using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Exceptions
{
    public class ProductNotFoundException : DomainException
    {
        public ProductNotFoundException(Guid productId)
            : base($"Товар с идентификатором {productId} не найден.") { }
    }
}
