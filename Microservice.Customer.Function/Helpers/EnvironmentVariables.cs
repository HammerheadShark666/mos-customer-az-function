namespace Microservice.Customer.Function.Helpers;

public class EnvironmentVariables
{
    public static string AzureServiceBusConnection => Environment.GetEnvironmentVariable(Constants.AzureServiceBusConnection);
}