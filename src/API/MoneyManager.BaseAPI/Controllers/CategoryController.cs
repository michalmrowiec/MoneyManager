using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyManager.API.Services;
using MoneyManager.Application.Functions.Categories.Commands.CreateCategory;
using MoneyManager.Application.Functions.Categories.Commands.DeleteCategory;
using MoneyManager.Application.Functions.Categories.Commands.UpdateCategory;
using MoneyManager.Application.Functions.Categories.Queries;
using MoneyManager.Application.Functions.Categories.Queries.GetAllCategories;
using MoneyManager.Application.Functions.Categories.Queries.GetCategoryById;
using System.Security.Claims;

namespace MoneyManager.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/category")]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUserContextService _userContextService;
        public CategoryController(IMediator mediator, IUserContextService userContextService)
        {
            _mediator = mediator;
            _userContextService = userContextService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateCategory([FromBody] CreateCategoryCommand createCategoryCommand)
        {
            createCategoryCommand.UserId = _userContextService.GetUserId;
            var category = await _mediator.Send(createCategoryCommand);
            return Created("", category);
        }

        [HttpGet("{categoryId}")]
        public async Task<ActionResult<CategoryDto>> GetRecordById([FromRoute] int categoryId)
        {
            return Ok(await _mediator.Send(new GetCategoryByIdQuery(_userContextService.GetUserId, categoryId)));
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoryDto>>> GetAllCategories()
        {
            return Ok(await _mediator.Send(new GetAllCategoriesQuery(_userContextService.GetUserId)));
        }

        [HttpDelete("{categoryId}")]
        public async Task<ActionResult> DeleteCategory([FromRoute] int categoryId)
        {
            await _mediator.Send(new DeleteCategoryCommand(_userContextService.GetUserId, categoryId));
            return NoContent();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateRecord([FromBody] UpdateCategoryCammand categoryItem)
        {
            categoryItem.UserId = _userContextService.GetUserId;
            await _mediator.Send(categoryItem);
            return Ok();
        }
    }
}
