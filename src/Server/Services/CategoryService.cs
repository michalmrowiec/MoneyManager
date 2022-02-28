using MoneyManager.Server.Entities;
using MoneyManager.Server.Exceptions;
using MoneyManager.Shared;

namespace MoneyManager.Server.Services
{
    public interface ICategoryService
    {
        public CategoryItemDto Post(CategoryItemDto categoryItem);
        public List<CategoryItemDto> GetAllCategories();
        public void Delete(int categoryId);
        public void Update(CategoryItemDto categoryItem);
    }
    public class CategoryService : ICategoryService
    {
        private readonly TrackerDbContext _dbContext;
        private readonly ILogger<CategoryService> _logger;
        private readonly IUserContextService _userContextService;
        public CategoryService(TrackerDbContext dbContext, ILogger<CategoryService> logger, IUserContextService userContextService)
        {
            _dbContext = dbContext;
            _logger = logger;
            _userContextService = userContextService;
            if (!_dbContext.Database.CanConnect())
            {
                _logger.LogError("Database cannot connect");
                throw new InternalServerErrorException("Unable to connect to the database");
            }
        }

        public void Delete(int categoryId)
        {
            var userId = _userContextService.GetUserId;
            var category = _dbContext.Categories.Where(x => x.UserId == userId).FirstOrDefault(x => x.Id == categoryId);
            var records = _dbContext.RecordItems.Where(x => x.CategoryId == categoryId).ToList();

            if (records is null)
            {
                throw new InternalServerErrorException("category has a reference to recordS");
            }

            if (category is null)
            {
                _logger.LogError($"Category with id: {categoryId}, try delete by user with id: {userId} - category not found");
                throw new NotFoundException("Category not found");
            }

            _dbContext.Categories.Remove(category);
            _dbContext.SaveChanges();
        }

        public List<CategoryItemDto> GetAllCategories()
        {
            var userId = _userContextService.GetUserId;
            var categories = _dbContext.Categories.Where(x => x.UserId == userId).ToList();

            if (categories is null)
            {
                _logger.LogError($"User categories with id: {userId} were not found");
                throw new NotFoundException("Categories not found");
            }

            List<CategoryItemDto> categoryItemDtos = new();
            foreach (var item in categories)
            {
                categoryItemDtos.Add(
                    new CategoryItemDto
                    {
                        Id = item.Id,
                        Name = item.CategoryName
                    });
            }
            return categoryItemDtos;
        }

        public CategoryItemDto Post(CategoryItemDto categoryItem)
        {
            int userId = _userContextService.GetUserId is null ? throw new ForbiddenException() : (int)_userContextService.GetUserId;

            _dbContext.Categories.Add(new Category
            {
                Id = categoryItem.Id,
                CategoryName = categoryItem.Name,
                UserId = userId
            });
            _dbContext.SaveChanges();
            var newCategory = _dbContext.Categories.Where(_x => _x.UserId == userId).OrderBy(x => x.Id).Last();
            return new CategoryItemDto { Id = newCategory.Id, Name = newCategory.CategoryName };
        }

        public void Update(CategoryItemDto categoryItem)
        {
            var userId = _userContextService.GetUserId;

            var category = _dbContext.Categories.Where(x => x.UserId == userId).FirstOrDefault(x => x.Id == categoryItem.Id);
            if (category is null)
            {
                _logger.LogError($"Category with id: {categoryItem.Id}, try update by user with id: {userId} - category not found");
                throw new NotFoundException("Caategory not found");
            }

            category.CategoryName = categoryItem.Name;

            _dbContext.SaveChanges();
        }
    }
}
