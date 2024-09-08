namespace Microservice.Customer.Function.Helpers;

public class Constants
{
    public const string DatabaseConnectionString = "SQLAZURECONNSTR_CUSTOMER";

    public const string AzureServiceBusConnection = "ServiceBusConnection";
    public const string AzureServiceBusQueueRegisteredUserCustomer = "AZURE_SERVICE_BUS_QUEUE_CUSTOMER";

    public const string FailureReasonValidation = "Validation Errors.";
    public const string FailureReasonInternal = "Internal Error.";
}