using gRPC.FirstSample.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();
builder.Services.AddGrpcReflection();

var app = builder.Build();

app.MapGrpcService<PersonGrpcService>();
app.MapGrpcReflectionService();

app.Run();