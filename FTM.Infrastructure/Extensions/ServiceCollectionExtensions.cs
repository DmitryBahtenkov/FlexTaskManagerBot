using FTM.Domain.Exceptions;
using FTM.Domain.Factories;
using FTM.Domain.Models.IssueModel;
using FTM.Domain.Models.IssueModel.DTO;
using FTM.Domain.Models.IssueModel.FieldSetters;
using FTM.Domain.Models.IssueModel.Pipelines;
using FTM.Domain.ServiceBus;
using FTM.Domain.Services;
using FTM.Domain.Services.Issues;
using FTM.Domain.Services.TextParser;
using FTM.Domain.Units;
using FTM.Infrastructure.BotImplementation.Commands;
using FTM.Infrastructure.BotImplementation.Services;
using FTM.Infrastructure.Cron;
using FTM.Infrastructure.DataAccess.Context;
using FTM.Infrastructure.Initialization;
using FTM.Infrastructure.Redis;
using FTM.Infrastructure.Services;
using FTM.Infrastructure.TextParser;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types;

namespace FTM.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection)
    {
        var infrastructureTypes = typeof(ICollector).Assembly.GetTypes()
            .Union(typeof(BusinessException).Assembly.GetTypes())
            .ToArray();

        Array.ForEach(Collectors.GetCollectors, collector =>
        {
            collector.Collect(serviceCollection, infrastructureTypes);
        });
        
        Array.ForEach(infrastructureTypes, type =>
        {
            foreach (var collector in Collectors.ServiceCollectors)
            {
                if (collector.NeedToCollect(type))
                {
                    collector.Collect(serviceCollection, type);
                }
            }
        });

        serviceCollection.AddScoped<InitializerService>();
        serviceCollection.AddScoped<BotHandlerFactory<Update>>();
        serviceCollection.AddScoped<BotCommandFactory<Update>>();
        serviceCollection.AddScoped<BotProcessorFactory<Update>>();
        
        serviceCollection.AddDbContext<FtmDbContext>();
        serviceCollection.AddScoped(typeof(IUnitOfWork<>), typeof(UnitOfWork<>));
        serviceCollection.AddScoped<CommandWrapper>();

        serviceCollection.AddScoped<BotCommandFactory<Update>>();
        serviceCollection.AddScoped<BotHandlerFactory<Update>>();
        serviceCollection.AddScoped<BotProcessorFactory<Update>>();

        serviceCollection.AddScoped<RepositoryContext>();
        
        serviceCollection.AddScoped<TelegramService>();
        serviceCollection.AddScoped<BotClientService>();
        serviceCollection.AddScoped<MessageAttachmentService>();

        serviceCollection.AddScoped<IEmailService, EmailService>();
        
        serviceCollection.AddScoped<ICurrentUserService, CurrentUserService>();
        serviceCollection.AddScoped<MessengerService>();
        serviceCollection.AddScoped<IStatisticService, StatisticService>();

        serviceCollection.AddScoped<IPublisherService, RedisPublisher>();
        serviceCollection.AddScoped<ISubscriberService, RedisSubscriber>();
        serviceCollection.AddScoped<ICacheService,CacheService>();
        serviceCollection.AddScoped<AutoSubscriberContext>();
        serviceCollection.AddScoped<AutoSubscriber>();
        serviceCollection.AddScoped<BotCommandsService>();

        serviceCollection.AddTransient<JobFactory>();
        serviceCollection.AddTransient<RemindJob>();
        serviceCollection.AddTransient<DailyJob>();

        serviceCollection.AddScoped<ITextParser, HorsTextParserAdapter>();
        serviceCollection.AddPipeline<IssuePipeline, ICreateIssuePipelineHandler>(
            typeof(TextParserIssuePipelineHandler),
            typeof(FolderIssuePipelineHandler));

        serviceCollection.AddScoped<CreateIssueCommand>()
            .AddChain<IssueDateTimeChainHandler, CreateIssueResult>(typeof(TextParserChainHandler), typeof(IssueFolderChainHandler));

        serviceCollection
            .AddSettersForType<Issue>(typeof(RemindTimeFieldSetter))
            .AddScoped(typeof(FieldSetterFactory<>));
        
        return serviceCollection;
    }

    public static IServiceCollection AddPipeline<TPipeline, THandler>(
        this IServiceCollection serviceCollection,
        params Type[] handlerTypes) where TPipeline : class
    {
        serviceCollection.AddScoped<TPipeline>();

        foreach (var handlerType in handlerTypes)
        {
            serviceCollection.AddScoped(typeof(THandler), handlerType);
        }

        return serviceCollection;
    }

    public static IServiceCollection AddChain<TInitial, TInput>(this IServiceCollection collection, params Type[] types) where TInitial : class, IChainHandler<TInput>
    {
        collection.AddScoped<TInitial>();
        
        Array.ForEach(types, x => collection.AddScoped(x));

        return collection;
    }
    public static IServiceCollection AddSettersForType<T>(
        this IServiceCollection serviceCollection,
        params Type[] setterTypes)
    {
        foreach (var setterType in setterTypes)
        {
            serviceCollection.AddScoped(typeof(IFieldSetter<T>), setterType);
        }

        return serviceCollection;
    }
}