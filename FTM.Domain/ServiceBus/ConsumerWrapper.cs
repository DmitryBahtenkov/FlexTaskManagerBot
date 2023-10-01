using System.Text.Json;
using FTM.Domain.Models.UserModel;
using FTM.Domain.Services;
using FTM.Domain.Units;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FTM.Domain.ServiceBus;

public class ConsumerWrapper
{
    private readonly AutoSubscriberContext _autoSubscriberContext;
    private readonly Type _consumerType;
    
    public ConsumerWrapper(AutoSubscriberContext autoSubscriberContext, Type consumerType)
    {
        _autoSubscriberContext = autoSubscriberContext;
        _consumerType = consumerType;
    }

    public async Task Consume(BaseEvent @event, IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<ConsumerWrapper>>();
            var consumer = (IConsumer) scope.ServiceProvider.GetRequiredService(_consumerType);
            var currentUserService = scope.ServiceProvider.GetRequiredService<ICurrentUserService>();
            var userRepository = scope.ServiceProvider.GetRequiredService<IRepository<User>>();
            
            logger.LogInformation("Start event: {data}", JsonSerializer.Serialize(@event));

            if (@event.UserId.HasValue)
            {
                var user = await userRepository.ByIdAsync(@event.UserId.Value);

                if (user is not null)
                {
                    currentUserService.Set(user);
                }
            }

            try
            {
                await consumer.Consume(@event);
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error consuming event {eventType}: {eventId}", @event.GetType().FullName, @event.EventId);
            }
        }
    }
}