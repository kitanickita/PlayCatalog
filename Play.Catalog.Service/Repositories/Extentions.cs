﻿
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Settings;

namespace Play.Catalog.Service.Repositories
{
	public static class Extentions
	{
		public static IServiceCollection AddMongo(this IServiceCollection services)
		{
			BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
			BsonSerializer.RegisterSerializer(new DateTimeOffsetSerializer(BsonType.String));

			// Here we deserializing values from appsettings.json.
			
			// This line we actually constructing mongoclient;
			services.AddSingleton(serviceProvider =>
			{
				// it's already registered with asp net service. so we can gain access through the serviceProvider

				var configuration = serviceProvider.GetService<IConfiguration>();
				var serviceSettings = configuration.GetSection(nameof(ServiceSettings))

												   .Get<ServiceSettings>();
				var mongoDbSettings = configuration.GetSection(nameof(MongoDbSettings))
												   .Get<MongoDbSettings>();
				var mongoClient = new MongoClient(mongoDbSettings.ConnectionString);
				return mongoClient.GetDatabase(serviceSettings.ServiceName);
			});
			return services;
		}

		public static IServiceCollection AddMongoRepository<T>(this IServiceCollection services, string collectionName) where T : IEntity
		{
			services.AddSingleton<IRepository<T>>(serviceProvider =>
			{
				var database = serviceProvider.GetService<IMongoDatabase>();
				return new MongoRepository<T>(database, collectionName);
			});
			return services;
		}
	}
}
  