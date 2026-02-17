using Application.Common.Models;
using Application.CQRS.ProductCatalog.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.CQRS.ProductCatalog.Queries.GetCategories
{
    public record GetCategoriesQuery : IRequest<Result<List<CategoryDto>>>;
}
