using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TransportService.Consumer;
using TripService.Repository;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();


builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<GetTransportOfferConsumer>();
    x.AddConsumer<ReserveTransportQueryConsumer>();
    x.AddConsumer<RollbackReserveTransportQueryConsumer>();
    x.AddConsumer<ConfirmTransportOrderConsumer>();
    x.AddConsumer<GetUserTripsTransportConsumer>();

    builder.Services.AddTransient<ITransportRepository, TransportRepository>();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("rsww_175741_rabbitmq", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ConfigureEndpoints(context);
    });
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();