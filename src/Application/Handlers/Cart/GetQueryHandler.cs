using System.ComponentModel;
using System.Reflection;
using System.Linq.Dynamic.Core;
using System.Net;
using Application.Queries.Cart;
using Domain.Dto;
using Domain.Enums;
using Infraestructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Handlers.Cart;

public class GetQueryHandler : IRequestHandler<GetQuery, BaseResult<BasePaginationResult<List<CartDto>>?>>
{
    private readonly ICartDomain _cartDomain;
    private static readonly HashSet<string> AllowedFilterFields = new()
    {
        "CartId", "UserId", "Data"
    };

    public GetQueryHandler(ICartDomain cartDomain)
    {
        _cartDomain = cartDomain;
    }

    public async Task<BaseResult<BasePaginationResult<List<CartDto>>?>> Handle(GetQuery request, CancellationToken cancellationToken)
    {
        var query = _cartDomain.Carts
            .Include(c => c.CartProducts)
                .ThenInclude(cp => cp.Product)
            .AsQueryable();

        static string? GetEnumDescription(Enum value) =>
            value.GetType()
                 .GetField(value.ToString())
                 ?.GetCustomAttribute<DescriptionAttribute>()
                 ?.Description;

        if (request.Filters != null)
        {
            var entityType = typeof(Domain.Entities.Cart);
            foreach (var filter in request.Filters)
            {
                var key = filter.Key;
                var value = filter.Value;

                var field = key.StartsWith("_min") || key.StartsWith("_max") ? key[4..] : key;

                if (!AllowedFilterFields.Contains(field))
                    continue;

                var propInfo = entityType.GetProperty(field);
                if (propInfo == null)
                    continue;

                var targetType = Nullable.GetUnderlyingType(propInfo.PropertyType) ?? propInfo.PropertyType;

                try
                {
                    if (key.StartsWith("_min"))
                    {
                        var converted = Convert.ChangeType(value, targetType);
                        query = query.Where($"{field} >= @0", converted);
                    }
                    else if (key.StartsWith("_max"))
                    {
                        var converted = Convert.ChangeType(value, targetType);
                        query = query.Where($"{field} <= @0", converted);
                    }
                    else if (value.Contains("*"))
                    {
                        var clean = value.Replace("*", "");

                        if (targetType == typeof(string))
                        {
                            if (value.StartsWith("*") && value.EndsWith("*"))
                                query = query.Where($"{field}.Contains(@0)", clean);
                            else if (value.StartsWith("*"))
                                query = query.Where($"{field}.EndsWith(@0)", clean);
                            else if (value.EndsWith("*"))
                                query = query.Where($"{field}.StartsWith(@0)", clean);
                        }
                        else
                        {
                            // tentativa opcional para int, DateTime, etc.
                            var stringified = value.Replace("*", "");
                            var typed = Convert.ChangeType(stringified, targetType);
                            query = query.Where($"{field} == @0", typed);
                        }
                    }
                    else
                    {
                        var typed = Convert.ChangeType(value, targetType);
                        query = query.Where($"{field} == @0", typed);
                    }
                }
                catch
                {
                    // Ignora filtro inválido
                    continue;
                }
            }
        }

        if (!string.IsNullOrEmpty(request._order))
        {
            var orderFields = request._order.Replace("\"", "").Split(',');

            var orderClauses = new List<string>();

            foreach (var orderFieldRaw in orderFields)
            {
                var parts = orderFieldRaw.Trim().Split(' ');
                var fieldNameRaw = parts[0];
                var direction = parts.Length > 1 ? parts[1].ToLower() : "asc";

                if (Enum.TryParse<OrderEnums>(fieldNameRaw, true, out var orderEnum))
                {
                    var propName = GetEnumDescription(orderEnum) ?? orderEnum.ToString();

                    if (direction != "asc" && direction != "desc")
                        direction = "asc";

                    orderClauses.Add($"{propName} {direction}");
                }
            }

            if (orderClauses.Count > 0)
            {
                query = query.OrderBy(string.Join(", ", orderClauses));
            }
        }
        else
        {
            query = query.OrderBy("CartId asc");
        }

        var total = await query.CountAsync(cancellationToken);

        var data = await query
            .Skip((request._page - 1) * request._size)
            .Take(request._size)
            .ToListAsync(cancellationToken);

        var dto = data.Select(c => new CartDto
        {
            CartId = c.CartId,
            UserId = c.UserId,
            Data = c.Data,
            Products = c.CartProducts.Select(cp => new ProductResumeDto
            {
                ProductId = cp.ProductId,
                Quantity = cp.Quantity
            }).ToList()
        }).ToList();

        return new BaseResult<BasePaginationResult<List<CartDto>>?>
        {
            Status = HttpStatusCode.OK,
            Result = new BasePaginationResult<List<CartDto>>
            {
                CurrentPage = request._page,
                TotalPages = (int)Math.Ceiling(total / (double)request._size),
                TotalItems = total,
                Data = dto
            }
        };
    }
}
