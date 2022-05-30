using System;
using ApiGateway;
using ApiGateway.Consumers;
using ApiGateway.Hubs;
using ApiGateway.Services;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using CommonComponents;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSignalR();
builder.Services.AddMassTransit(x =>
{
    x.AddRequestClient<PaymentQuery>(RequestTimeout.After(s:3));
    x.AddConsumer<NotifyAboutTripPurchaseConsumer>();
    x.AddConsumer<NotifyAboutNewPopularCountryConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", 5672, "/", h =>
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

app.UseAuthorization();

app.MapControllers();
app.MapHub<NotificationHub>("/hubs/test");


app.UseCors(options =>
    options.WithOrigins("http://localhost:3000")
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials());

app.Run();