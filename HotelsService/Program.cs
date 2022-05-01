using System;
using HotelsService;
using HotelsService.Consumers;
using HotelsService.Models;
using HotelsService.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MassTransit;
using MassTransit.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

builder.Services.AddDbContext<hotelsContext>(
    o => o.UseNpgsql(builder.Configuration.GetConnectionString("HotelsDb"))
);
builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddMassTransit(x =>
    {
        x.AddConsumer<GetHotelsConsumer>();
        x.SetKebabCaseEndpointNameFormatter();
        x.UsingRabbitMq((context, cfg) =>
        {
            cfg.Host("rabbitmq", 5672, "/", h =>
            {
                h.Username("guest");
                h.Password("guest");
            });
            cfg.ConfigureEndpoints(context);
        });
    });

builder.Services.Configure<HotelDescriptionDbSettings>(
    builder.Configuration.GetSection("DescriptionDb")
);

var app = builder.Build();

app.MapControllers();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});
app.Run();

