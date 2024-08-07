using MassTransit;

namespace Api.MassTransit;

public static class BusRegistrationConfiguratorExtensions
{
    public static void UseRabbitMq(this IBusRegistrationConfigurator x, RabbitMqSettings settings)
    {
        x.UsingRabbitMq((context, config) =>
        {
            config.Host(settings.Host, "/", host =>
            {
                host.Username(settings.UserName);
                host.Password(settings.Password);
            });

            //One receive endpoint (queue) for all consumers
            config.ReceiveEndpoint(settings.QueueName, e => { e.ConfigureConsumers(context); });
        });
    }
}