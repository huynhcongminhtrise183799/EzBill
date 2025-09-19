using EzBill.Domain.Entity;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Infrastructure
{
	public interface IMongoDbContext
	{
		IMongoCollection<Messages> Messages { get; }
		IMongoCollection<MessagesReadStatus> MessagesReadStatuses { get; }
	}
	public class MongoDbContext : IMongoDbContext
	{
		private readonly IMongoDatabase _database;

		public MongoDbContext(string connectionString, string dbName)
		{
			BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));

			var clientSettings = MongoClientSettings.FromConnectionString(connectionString);
			var client = new MongoClient(clientSettings);

			_database = client.GetDatabase(dbName);
		}


		public IMongoCollection<Messages> Messages
			=> _database.GetCollection<Messages>("Messages");

		public IMongoCollection<MessagesReadStatus> MessagesReadStatuses
			=> _database.GetCollection<MessagesReadStatus>("MessagesReadStatus");
	}
}
