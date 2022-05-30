using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TripService.Consumers;
using TripService.Repository;
using TripService.Saga;
using TripService.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ITripsRepository, TripsRepository>();
builder.Services.AddSingleton<IDepartueDirectionsPerferances,DepartueDirectionsPerferances>();

builder.Services.AddMassTransit(x =>
{
    x.AddSagaStateMachine<ReservationStateMachine, ReservationState>()
        .InMemoryRepository();
    x.AddConsumer<GetTripsQueryConsumer>();
    x.AddConsumer<GetTripOfferQueryConsumer>();
    x.AddConsumer<GetUserTripsQueryConsumer>();
    x.AddConsumer<CreateUserTripQueryConsumer>();
    x.AddConsumer<NewReservationCompleatedQueryConsumer>();
    x.AddConsumer<GetNotificationAboutPopularCountryConsumer>();

    x.AddDelayedMessageScheduler();
    x.SetKebabCaseEndpointNameFormatter();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.UseDelayedMessageScheduler();
        cfg.Host("rabbitmq", 5672, "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });
        cfg.ConfigureEndpoints(context);
    });
});

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