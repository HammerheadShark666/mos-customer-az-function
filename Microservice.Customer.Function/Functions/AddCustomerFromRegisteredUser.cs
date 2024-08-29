using Azure.Messaging.ServiceBus;
using MediatR;
using Microservice.Customer.Function.Helpers;
using Microservice.Customer.Function.Helpers.Exceptions;
using Microservice.Customer.Function.MediatR.AddCustomer;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Microservice.Customer.Function.Functions;

public class AddCustomerFromRegisteredUser(ILogger<AddCustomerFromRegisteredUser> logger, IMediator mediator)
{
    [Function(nameof(AddCustomerFromRegisteredUser))]
    public async Task Run([ServiceBusTrigger("%" + Constants.AzureServiceBusQueueRegisteredUserCustomer + "%",
                                             Connection = Constants.AzureServiceBusConnection)]
                           ServiceBusReceivedMessage message,
                           ServiceBusMessageActions messageActions)
    {
        var addCustomerRequest = JsonHelper.GetRequest<AddCustomerRequest>(message.Body.ToArray()) ?? throw new JsonDeserializeException("Error deserializing AddCustomerRequest.");

        try
        {
            logger.LogInformation("RegisteredUser - AddCustomer - {addCustomerRequest.Id}", addCustomerRequest.Id);

            await mediator.Send(addCustomerRequest);
            await messageActions.CompleteMessageAsync(message);

            return;
        }
        catch (FluentValidation.ValidationException validationException)
        {
            logger.LogError("Validation Failures: Id: {addCustomerRequest.Id}", addCustomerRequest.Id);
            await messageActions.DeadLetterMessageAsync(message, null, Constants.FailureReasonValidation, ErrorHelper.GetErrorMessagesAsString(validationException.Errors));
        }
        catch (Exception ex)
        {
            logger.LogError("Internal Error: Id: {addCustomerRequest.Id}", addCustomerRequest.Id);
            await messageActions.DeadLetterMessageAsync(message, null, Constants.FailureReasonInternal, ex.Message);
        }
    }
}
