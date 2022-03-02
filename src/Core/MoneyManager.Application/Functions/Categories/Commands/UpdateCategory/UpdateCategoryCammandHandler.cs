using AutoMapper;
using MediatR;
using MoneyManager.Application.Contracts.Persistence.Items;
using MoneyManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Functions.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCammandHandler : IRequestHandler<UpdateCategoryCammand>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public UpdateCategoryCammandHandler(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateCategoryCammand request, CancellationToken cancellationToken)
        {
            var category = _mapper.Map<Category>(request);

            await _categoryRepository.UpdateAsync(category);

            return Unit.Value;
        }
    }
}
