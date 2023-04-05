namespace MoneyManager.API.Middlewaare
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ApiKeyRequiredAttribute : Attribute
    { }
}