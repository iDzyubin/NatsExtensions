# NatsExtensions

This package contains some extensions for [NATS .NET Client](https://github.com/nats-io/nats.net).

## Introduction

This package introduces some abstractions for communication between services:

1. **Service Codes** is a codes for sending to NATS and receiving data from NATS; 
2. **Request** is an object that contains data for sending from Service1 to Service2;
3. **Reply** is an object that contains result from **Handler**;
4. **Handler** is an class for handling input requests from external services;
5. **Proxy** is a class for interaction between Services (send Request and get Reply).

## Code examples



### Create Request

In order to create request model, you should inherit *Request* model and use ```[ServiceBus]``` attribute like this:
```C#
    // ./Models/GetCustomersRequest.cs
    
    [ServiceBus(Code = ServiceBusCodes.GetCustomersRequest)]
    public class GetCustomersRequest : Request
    {
        // Some props here...
    }
```

### Create Reply
In order to create reply model, you should inherit *Reply* model and use ```[ServiceBus]``` attribute like this:
```C#
    // ./Models/GetCustomersReply.cs 
    
    [ServiceBus(Code = ServiceBusCodes.GetCustomersReply)]
    public class GetCustomersReply : Reply
    {
        // Some props here...
    }
```

### Create Handler
For those cases when you should get requests from external services, you should use *Handler*. 
When you create a handler, you should specify request and reply types in generic's angle brackets.
>**Tip**: custom Request/Reply models **must inherit** from Request/Reply abstract classes.
 ```C#
    // ./Handlers/GetCustomersHandler.cs
 
    public class GetCustomersHandler : IHandler<GetCustomersRequest, GetCustomersReply>
    {
        public async Task<GetCustomersReply> Handle(GetCustomersRequest request)
        {
            // Some handler's logic here...
        }
    }
```

### Create Proxy
For those cases when you should send request to external services and expects reply, you should use *Proxy*.

### Sync Proxy

There is an interface for sync proxy interaction - ```ISyncProxy<TRequest, TReply>```. If you want to use default logic, you can omit custom method realization and use short proxy realization from example below. 
Otherwise, you can implement interface ```ISyncProxy<TRequest, TReply>``` and realize your custom logic.  

Example of sync proxy declaration:
```C#
    // ./Proxies/OrderServiceProxy.cs

    public class OrderServiceProxy : BaseSyncProxy<GetOrdersByCustomerIdRequest, GetOrdersByCustomerIdReply>
    {
        public OrderServiceProxy(INatsService natsService) : base(natsService) { }
    }
```

Example of proxy usage:
```C#
  [ApiController]
  [Route("/api/[controller]")]
  public class CustomerController : ControllerBase
  {
      ...
      
      [HttpGet("{customerId:long}")]
      public IActionResult GetCustomerWithOrdersById(long customerId)
      {
          ...
          
          var reply = _orderServiceProxy.Execute(
              request: new GetOrdersByCustomerIdRequest { CustomerId = customerId },
              subject: ServiceBusSubjects.OrderSubject);
              
          ...
      }
                    
      ...
  }
```

### Async Proxy

There is an interface for async proxy interaction - ```IAsyncProxy<TRequest>```. If you want to use default logic, you can omit custom method realization and use short proxy realization from example below. 
Otherwise, you can implement interface ```IAsyncProxy<TRequest>``` and realize your custom logic.  

Example of async proxy declaration:
```C#
    // ./Proxies/OrderServiceProxy.cs

    public class OrderServiceProxy : BaseAsyncProxy<GetOrdersByCustomerIdRequest>
    {
        public OrderServiceProxy(INatsService natsService) : base(natsService) { }
    }
```

Example of proxy usage:
```C#
  [ApiController]
  [Route("/api/[controller]")]
  public class CustomerController : ControllerBase
  {
      ...
      
      [HttpGet("{customerId:long}")]
      public async Task<IActionResult> GetCustomerWithOrdersById(long customerId)
      {
          ...
          
          await _orderServiceProxy.ExecuteAsync(
              request: new GetOrdersByCustomerIdRequest { CustomerId = customerId },
              subject: ServiceBusSubjects.OrderSubject);
              
          ...
      }
                    
      ...
  }
```

### Dependency Injections
For use NatsExtensions, you should register it in DI.
```C#
    // ./Extensions/ServiceCollectionExtensions.cs

    public static class ServiceCollectionExtensions
    {
        ...

        public static IServiceCollection AddNats(this IServiceCollection services, IConfiguration configuration) =>
            services.AddNatsExtensions(builder =>
            {
                builder.Subject = configuration.GetSection("Nats")["Subject"];
                builder.ConnectionString = configuration.GetConnectionString("NatsConnection");
            })
            .AddNatsHandlers()
            .AddNatsProxies();
        
        private static IServiceCollection AddNatsHandlers(this IServiceCollection services) =>
            services.AddNatsHandler<
                GetCustomersRequest, 
                GetCustomersReply, 
                GetCustomersHandler>();
        
        private static IServiceCollection AddNatsProxies(this IServiceCollection services) =>
            services.AddNatsProxy<
                GetOrdersByCustomerIdRequest, 
                GetOrdersByCustomerIdReply,
                OrderServiceProxy>();
            
        ...
    }
```
