using AutoMapper;
using MediatR;
using MoneyManager.Application.Contracts.Persistence.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Functions.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var cateogryDelete = await _categoryRepository.GetByIdAsync(request.UserId, request.CategoryId);

            await _categoryRepository.DeleteAsync(request.UserId, cateogryDelete);

            return Unit.Value;
        }
    }
}
