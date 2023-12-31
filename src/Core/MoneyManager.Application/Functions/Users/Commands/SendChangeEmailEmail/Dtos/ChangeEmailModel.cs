namespace MoneyManager.Application.Functions.Users.Commands.SendChangeEmailEmail.Dto
{
    internal record ChangeEmailModel(int UserId, string NewEmail, string OldEmail);
}
