using NatsExtensions.Attributes;
using NatsExtensions.Models;
using NatsExtensions.Sender.Messages;

namespace NatsExtensions.Sender.Models;

[ServiceBus(MessageCodes.GetProductsRequest)]
public class GetProductsRequest : Request
{
    
}