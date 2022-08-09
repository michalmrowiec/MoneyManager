using MediatR;
using MoneyManager.Application.Functions.Categories.Commands.CreateCategory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyManager.Application.Functions.Users.Services
{
    public class CreateStartCategoryForNewUser
    {
        private List<CreateCategoryCommand> categories = new()
        {
            new CreateCategoryCommand { Name = "Food" },
            new CreateCategoryCommand { Name = "Transport" },
            new CreateCategoryCommand { Name = "Education" }
        };

        private readonly IMediator _mediator;

        public CreateStartCategoryForNewUser(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task CrateStartCategory(int userId)
        {
            //categories.ForEach(async c => { c.UserId = userId; await _mediator.Send(c); });

            foreach (var category in categories)
            {
                category.UserId = userId;
                await _mediator.Send(category);
            }
        }
    }
}
