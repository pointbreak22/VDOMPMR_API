using Application.Common.Models;
using Application.CQRS.ProductCatalog.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.ProductCatalog.Queries.GetProducts
{
    public record GetPagedProductsQuery(int PageNumber = 1, int PageSize = 10)
    : IRequest<Result<PaginatedList<ProductDto>>>;
}
