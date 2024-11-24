# gRPC

[Source](https://blog.postman.com/what-is-grpc/)

### Definition :

gRPC Trasfers data with binary format with HTTP2

### What is the difference between RPC and REST

- RPC (Remote procedure call)

  - Communication with Network protocol
  - It isn't asynchronous

- REST
  - Speak according to the HTTP

### what is the gRPC channel

- Critical data for initial a channel:
  - Host address
  - Port
  - Connection credential
    - SSL / TLS
    - Application layer transport security (ALTS)
    - Token based authentication

> [!TIP]
>
> 1. You can set a credential for a connection
> 2. You can set a credential for a request

- Channel status :
  - Idle
  - Connecting
  - Ready
  - Transent Failure
  - Shutdown

### How many type of services in gRPC

- Unary

![Unary](https://techdozo.dev/wp-content/uploads/2021/09/grpc-Page-2.png.webp)

- Service streaming

![Service streaming](https://techdozo.dev/wp-content/uploads/2021/10/grpc-Server-Streaming.drawio.png.webp)

- Client streaming

![Client streaming](https://techdozo.dev/wp-content/uploads/2023/01/image-1024x380.png.webp)

- Biderectional streaming

![Biderectional streaming](https://techdozo.dev/wp-content/uploads/2023/02/image-1024x520.png.webp)

### gRPC status

gRPC status are different with http status. There are some status of gRPC

- OK (0)
- CANCELLED (1)
- UNKNOWN (2)
- INVALID_ARGUMENT (3)
- DEADLINE_EXCEEDED (4)
- NOT_FOUND (5)
- ALREADY_EXISTS (6)
- PERMISSION_DENIED (7)
- RESOURCE_EXHAUSTED (8)
- FAILED_PRECONDITION (9)
- ABORTED (10)
- OUT_OF_RANGE (11)
- UNIMPLEMENTED (12)
- INTERNAL (13)
- UNAVAILABLE (14)
- DATA_LOSS (15)
- UNAUTHENTICATED (16)

### What are the advantages and disadvantages of gRPC

- Advantages :

  - High Performance: gRPC is built on HTTP/2 and Protocol Buffers (Protobuf),
    which make data transmission faster and more efficient.
    The binary serialization of Protobuf is more compact and quicker to process compared to text formats like JSON.

  - Supports Multiplexing: With HTTP/2, gRPC can handle multiple simultaneous requests over a single connection,
    which is particularly useful for real-time applications.

  - Rich Communication Options: gRPC supports four types of communication:

  - Strongly Typed APIs: With Protobuf, APIs are strongly typed, enabling better data validation, documentation, and type-safety.\

  - Binary Format : the bester security

- Disadvantages :

  - Binary Format Not Human-Readable: Protobuf is a binary format, making it harder to debug directly compared to text-based formats like JSON.

  - Limited Browser Support: gRPC cannot be used natively in browsers because HTTP/2 support is limited in certain browser contexts.
    Workarounds like RPC-web are required.

  - Learning Curve for Protobuf: Protobuf requires defining schemas and learning a new serialization format,
    which can increase development complexity for teams not familiar with it.

  - Limited RESTful Integration: gRPC doesn’t align with traditional REST/HTTP conventions,
    which may not be ideal for public APIs or services needing REST compatibility.

  - Incompatibility with HTTP/1.1: Since gRPC relies on HTTP/2, legacy systems or networks using HTTP/1.1 may face compatibility issues.

### What is the Protocol Buffer

ProtoBuff file consist of 3 sections

1. Indivisual declaration (ProtoBuff version, ...)

```c#
    syntax = "proto3";

    option csharp_namespace = "ProtobufSamples"

    package greet;
```

> [!IMPORTANT]
> If you just use 'package' section, your service namespace will the name of package.
> However, When you declare 'option csharp_namespace', your service will be worked with the name's option

> [!TIP]
> When you have both choice 'option' and 'package', the package name comes after service name as a prefix.Moreover,
> the option wont be changed and will work as a namespace

2. Service declaration

```c#

service ServiceName{
    rpc MethodName(InputParam) returns (OutputParam){};
}

```

> [!TIP]
> Each method has to have Input Parameter and Outout parameters

3. Message declaration

### Exception handling

You have 2 approaches against exception handling

- **Implement throw exception every time when you want to handle a code**

<br>

```c#
    public override async Task GetAll()
    {
        try
        {
            foreach (var person in _people)
            {
                await responseStream.WriteAsync(person);
            }

            throw new InvalidTimeZoneException("Timezone isn't correct");
        }
        catch (Exception ex)
        {
            var correlationId = Guid.NewGuid().ToString();
            var trilers = new Metadata
          {
              { "CorrelationId", correlationId },
              { "Interceptor", "true" }
          };

            throw new RpcException(new Status(StatusCode.Internal, ex.Message), trilers, "Interceptor exception");
        }
    }
```

<br>

- **Use Interceptor**

  - Interceptors are a gRPC concept that allows apps to interact with incoming or outgoing gRPC calls. They offer a way to enrich the request processing pipeline.

<br>

**ExceptionInterceptor.cs**

```c#
public class ExceptionInterceptor : Interceptor
{
  public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
  {
      try
      {
          return await continuation(request, context);
      }
      catch (Exception ex)
      {
          var correlationId = Guid.NewGuid().ToString();
          var trilers = new Metadata
          {
              { "CorrelationId", correlationId },
              { "Interceptor", "true" }
          };

          throw new RpcException(new Status(StatusCode.Internal, ex.Message), trilers, "Interceptor exception");
      }
  }
}
```

**Program.cs**

```c#
builder.Services.AddGrpc(config => {

    config.EnableDetailedErrors = true;
    config.Interceptors.Add<ExceptionInterceptor>();

});
```


### Versioning

### Expose Protobuf

### Client