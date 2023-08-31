using MediatR;
using MoneyManager.Application.Functions.Categories.Commands.CreateCategory;

namespace MoneyManager.Application.Functions.Users.Services
{
    public class CreateStartCategoryForNewUser
    {
        private readonly List<CreateCategoryCommand> categories = new()
        {
            new CreateCategoryCommand { Name = "Work 💵" },
            new CreateCategoryCommand { Name = "Education 🎓" },
            new CreateCategoryCommand { Name = "Healthy 💊" },
            new CreateCategoryCommand { Name = "Entertainment 🎬" },
            new CreateCategoryCommand { Name = "Shopping 🛍️" },
            new CreateCategoryCommand { Name = "Clothes 👕" },
            new CreateCategoryCommand { Name = "Transport 🚗" },
            new CreateCategoryCommand { Name = "Food 🍽️" },
            new CreateCategoryCommand { Name = "Other 🧩" }
        };

        private readonly IMediator _mediator;

        public CreateStartCategoryForNewUser(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task CrateStartCategory(int userId)
        {
            foreach (var category in categories)
            {
                category.UserId = userId;
                await _mediator.Send(category);
            }
        }
    }
}
