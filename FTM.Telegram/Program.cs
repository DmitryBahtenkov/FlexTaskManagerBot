using System.Data;
using FTM.Domain.Events.Bot;
using FTM.Domain.Helpers;
using FTM.Domain.ServiceBus;
using FTM.Infrastructure.BotImplementation.Services;
using FTM.Infrastructure.DataAccess.Context;
using FTM.Infrastructure.Extensions;
using FTM.Infrastructure.Initialization;
using FTM.Infrastructure.Services;
using FTM.Telegram;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddInfrastructure();

var opt = new BotOptions();
builder.Configuration.Bind("BotOptions", opt);
builder.Services.Configure<BotOptions>(builder.Configuration.GetSection(nameof(BotOptions)));
Configuration.Init(builder.Configuration);
var app = builder.Build();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
using (var scope = app.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<InitializerService>();
    await initializer.Init();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapPost("api/v1/callback", async ([FromBody] object data, [FromServices] TelegramService telegramService) =>
{
    await telegramService.FromRaw(data.ToString()!);
});

app.MapPost("api/v1/async-callback", async ([FromBody] object data, [FromServices] IPublisherService publisherService) =>
{
    await publisherService.Publish(new RawUpdateEvent()
    {
        Source = Source.Telegram,
        UpdateRaw = data.ToString()!
    });
});

app.MapGet("api/v1/healthcheck", async (
    [FromServices] ILogger<object> logger,
    [FromServices] FtmDbContext dbContext
    ) =>
{
    logger.LogInformation("Healthcheck");
    var connection = dbContext.Database.GetDbConnection();
    try
    {
        await connection.OpenAsync();
        if (connection.State != ConnectionState.Open)
        {
            return new CheckResult(false, DateTime.Now) { Error = "Connection to db not opened" };
        }
    }
    catch (Exception e)
    {
        logger.LogInformation(e, "Healthcheck error");
        return new CheckResult(false, DateTime.Now) { Error = e.Message };
    }
    finally
    {
        await connection.CloseAsync();
    }
    
    return new CheckResult(true, DateTime.Now);
});

app.Run();