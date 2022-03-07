using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.Application.Functions.Categories.Commands.CreateCategory;
using MoneyManager.Application.Functions.Categories.Commands.DeleteCategory;
using MoneyManager.Application.Functions.Categories.Commands.UpdateCategory;
using MoneyManager.Application.Functions.Categories.Queries;
using MoneyManager.Application.Functions.Categories.Queries.GetAllCategories;
using System.Security.Claims;

namespace MoneyManager.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/category")]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;
        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult> CreateCategory([FromBody] CreateCategoryCommand createCategoryCommand)
        {
            createCategoryCommand.UserId = GetUserId();
            var category = await _mediator.Send(createCategoryCommand);
            return Created("", category.CategoryDto);
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoryDto>>> GetAllCategories()
        {
            return Ok(await _mediator.Send(new GetAllCategoriesQuery(GetUserId())));
        }

        [HttpDelete("{categoryId}")]
        public async Task<ActionResult> DeleteCategory([FromRoute] int categoryId)
        {
            await _mediator.Send(new DeleteCategoryCommand(GetUserId(), categoryId));
            return NoContent();
        }

        [HttpPatch]
        public async Task<ActionResult> UpdateRecord([FromBody] UpdateCategoryCammand categoryItem)
        {
            categoryItem.UserId = GetUserId();
            await _mediator.Send(categoryItem);
            return Ok();
        }

        private int GetUserId()
        {
            var f = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            return f == null ? 0 : int.Parse(f.Value);
        }
    }
}
