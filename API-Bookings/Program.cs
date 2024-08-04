using API_Bookings.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

var app = builder.Build();

app.MapGrpcService<GreeterService>();
app.MapGet("/", () =>
{
    return "API Booking is running.....";
});

app.Run();
