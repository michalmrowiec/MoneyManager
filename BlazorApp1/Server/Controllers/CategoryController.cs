using BlazorApp1.Server.Services;
using BlazorApp1.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlazorApp1.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/category")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        public ActionResult CreateCategory([FromBody] CategoryItemDto categoryItemDto)
        {
            return Created("", _categoryService.Post(categoryItemDto));
        }

        [HttpGet]
        public ActionResult<List<CategoryItemDto>> GetAllCategories()
        {
            return Ok(_categoryService.GetAllCategories());
        }

        [HttpDelete("{categoryId}")]
        public ActionResult DeleteCategory([FromRoute] int categoryId)
        {
            _categoryService.Delete(categoryId);
            return NoContent();
        }

        [HttpPatch]
        public ActionResult UpdateRecord([FromBody] CategoryItemDto categoryItem)
        {
            _categoryService.Update(categoryItem);
            return Ok();
        }
    }
}
