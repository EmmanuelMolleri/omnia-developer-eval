using Application.Queries.Categories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace products_api.Controllers;

[Route("api/products/categories")]
[ApiController]
public class CategoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{category}")]
    public async Task<IActionResult> GetById([FromRoute] GetCategoryRatingsQuery query)
    {
        var result = await _mediator.Send(query);

        if (!result.Result.Any())
        {
            return NoContent();
        }

        return Ok(result.Result);
    }
}