using Azure.Messaging.ServiceBus;
using MediatR;
using Microservice.Customer.Function.Functions;
using Microservice.Customer.Function.MediatR.AddCustomer;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;

namespace Microservice.Customer.Function.Test.Unit;

public class AddCustomerFromRegisteredUserAzureFunctionTests
{
    private readonly Mock<IMediator> _mockMediator;
    private readonly Mock<ILogger<AddCustomerFromRegisteredUser>> _mockLogger;
    private readonly AddCustomerFromRegisteredUser _addCustomerFromRegisteredUser;

    public AddCustomerFromRegisteredUserAzureFunctionTests()
    {
        _mockMediator = new Mock<IMediator>();
        _mockLogger = new Mock<ILogger<AddCustomerFromRegisteredUser>>();
        _addCustomerFromRegisteredUser = new AddCustomerFromRegisteredUser(_mockLogger.Object, _mockMediator.Object);
    }

    [Test]
    public async Task Azure_function_trigger_service_bus_receive_return_succeed()
    {
        var addCustomerRequest = new AddCustomerRequest(Guid.NewGuid(), "ValidEmail@hotmail.com", "TestSurname", "TestFirstName");

        var mockMessage = ServiceBusModelFactory.ServiceBusReceivedMessage(BinaryData.FromString(JsonConvert.SerializeObject(addCustomerRequest)), correlationId: Guid.NewGuid().ToString());

        var mockServiceBusMessageActions = new Mock<ServiceBusMessageActions>();
        mockServiceBusMessageActions.Setup(x => x.CompleteMessageAsync(mockMessage, CancellationToken.None)).Returns(Task.FromResult(true));

        await _addCustomerFromRegisteredUser.Run(mockMessage, mockServiceBusMessageActions.Object);
    }
}