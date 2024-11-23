using gRPC.FirstSample.Interceptors;
using gRPC.FirstSample.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc(config => {

    config.EnableDetailedErrors = true;
    config.Interceptors.Add<ExceptionInterceptor>();
    
});
builder.Services.AddGrpcReflection();

var app = builder.Build();

app.MapGrpcService<PersonGrpcService>();
app.MapGrpcReflectionService();

app.Run();