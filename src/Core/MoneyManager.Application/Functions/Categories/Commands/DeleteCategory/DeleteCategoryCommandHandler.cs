using MediatR;
using MoneyManager.Application.Contracts.Persistence.Items;

namespace MoneyManager.Application.Functions.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;

        public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var cateogryDelete = await _categoryRepository.GetByIdAsync(request.UserId, request.CategoryId);

            await _categoryRepository.DeleteAsync(request.UserId, cateogryDelete);

            return Unit.Value;
        }
    }
}
