using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Exceptions
{
    public class InsufficientStockException : DomainException
    {
        public InsufficientStockException(string productName)
            : base($"Недостаточно товара '{productName}' на складе.") { }
    }
}
