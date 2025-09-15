using EzBill.Domain.Entity;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EzBill.Infrastructure
{
	public class MongoDbContext
	{
		private readonly IMongoDatabase _database;

		public MongoDbContext(string connectionString, string dbName)
		{
			var client = new MongoClient(connectionString);
			_database = client.GetDatabase(dbName);
		}

		public IMongoCollection<Messages> Messages
			=> _database.GetCollection<Messages>("Messages");

		public IMongoCollection<MessagesReadStatus> MessagesReadStatuses
			=> _database.GetCollection<MessagesReadStatus>("MessagesReadStatus");
	}
}
