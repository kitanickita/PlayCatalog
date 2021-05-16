using System.Reflection;
using MassTransit;
using MassTransit.Definition;
using MassTransit.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Play.Catalog.Common.Settings;
using Play.Common.Settings;

namespace Play.Common.MassTransit
{
	public static class Extensions
	{
		public static IServiceCollection AddMassTransitWithRabbitMQ(this IServiceCollection services)
		{
			services.AddMassTransit(configure =>
			{

				// Context is actually a service provider
				configure.UsingRabbitMq((context, configurator) =>
				{
					//configure.AddConsumer<>(Assembly.GetEntryAssembly());

					var configuration = context.GetService<IConfiguration>();
					var serviceSettings = configuration.GetSection(nameof(ServiceSettings))
													   .Get<ServiceSettings>();
					var rabbitMQSettings = configuration.GetSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();
					configurator.Host(rabbitMQSettings.Host);
					configurator.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(serviceSettings.ServiceName, false));
				});
			});
			// this service starts masstransit bas
			services.AddMassTransitHostedService();

			return services;
		}
	}
}
