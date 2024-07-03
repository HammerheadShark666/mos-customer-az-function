﻿namespace Microservice.Customer.Function.Helpers;

public class Constants
{  
    public const string DatabaseConnectionString = "CUSTOMER_SQL_CONNECTIONSTRING";

    public const string AzureServiceBusConnection = "AZURE_SERVICE_BUS_CONNECTION";

    public const string RegisteredUserCustomerSBQueue = "registered-user-customer";

    public const string FailureReasonValidation = "Validation Errors.";
    public const string FailureReasonInternal = "Internal Error.";
}