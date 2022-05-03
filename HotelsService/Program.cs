using System;
using HotelsService;
using HotelsService.Consumers;
using HotelsService.Jobs;
using HotelsService.Models;
using HotelsService.Repositories;
using HotelsService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MassTransit;
using MassTransit.Configuration;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();

builder.Services.AddDbContext<hotelsContext>(
    o => o.UseNpgsql(builder.Configuration.GetConnectionString("HotelsDb"))
);
builder.Services.AddScoped<IHotelRepository, HotelRepository>();
builder.Services.AddScoped<IHotelService, HotelService>();
builder.Services.AddMassTransit(x =>
    {
        x.AddConsumer<GetHotelsConsumer>();
        x.AddConsumer<GetHotelOfferConsumer>();
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
/*
builder.Services.AddQuartz(q =>
    {
        q.UseMicrosoftDependencyInjectionJobFactory();
        JobKey jobKey = new JobKey("RemoveOverdueReservationsJob");
        q.AddJob<RemoveOverduedReservationsJob>(opts => opts.WithIdentity(jobKey));
        q.AddTrigger(opts => opts
            .ForJob(jobKey)
            .WithIdentity("RemoveOverdueReservationsJob-trigger")
            .WithCronSchedule("0/2 * * ? * * *"));
    });
builder.Services.AddQuartzHostedService(
    q => q.WaitForJobsToComplete = true
    );
*/
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

