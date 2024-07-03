using Azure.Messaging.ServiceBus;
using MediatR;
using Microservice.Customer.Function.Helpers;
using Microservice.Customer.Function.MediatR.AddCustomer;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Microservice.Customer.Function.Functions;

public class AddCustomerFromRegisteredUser(ILogger<AddCustomerFromRegisteredUser> logger, IMediator mediator)
{
    private ILogger<AddCustomerFromRegisteredUser> _logger { get; set; } = logger; 
    private IMediator _mediator { get; set; } = mediator;


    [Function(nameof(AddCustomerFromRegisteredUser))]
    public async Task Run([ServiceBusTrigger(Constants.RegisteredUserCustomerSBQueue, 
                                             Connection = Constants.AzureServiceBusConnection)]
                           ServiceBusReceivedMessage message,
                           ServiceBusMessageActions messageActions)
    {
        var addCustomerRequest = JsonHelper.GetRequest<AddCustomerRequest>(message.Body.ToArray());            

        try
        {               
            _logger.LogInformation(string.Format("RegisteredUser - AddCustomer - {0}", addCustomerRequest.Id.ToString()));
             
            await _mediator.Send(addCustomerRequest);
            await messageActions.CompleteMessageAsync(message);

            return; 
        }
        catch (FluentValidation.ValidationException validationException)
        { 
            _logger.LogError(String.Format("Validation Failures: Id: {0}", addCustomerRequest.Id.ToString()));
            await messageActions.DeadLetterMessageAsync(message, null, Constants.FailureReasonValidation, ErrorHelper.GetErrorMessagesAsString(validationException.Errors));
        }
        catch (Exception ex)
        {  
            _logger.LogError(String.Format("Internal Error: Id: {0}", addCustomerRequest.Id.ToString())); 
            await messageActions.DeadLetterMessageAsync(message, null, Constants.FailureReasonInternal, ex.Message);
        } 
    } 
}
