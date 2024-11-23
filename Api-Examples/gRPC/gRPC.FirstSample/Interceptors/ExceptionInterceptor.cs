using Grpc.Core;
using Grpc.Core.Interceptors;

namespace gRPC.FirstSample.Interceptors;

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
