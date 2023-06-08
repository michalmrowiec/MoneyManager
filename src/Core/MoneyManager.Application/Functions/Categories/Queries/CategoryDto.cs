namespace MoneyManager.Application.Functions.Categories.Queries
{
    public record CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
