using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using AutoDungeoners.Web.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace AutoDungeoners.Web.DataAccess.Repositories
{
    public class GenericRepository : IGenericRepository
    {
        private readonly IMongoClient client;
        private readonly MongoCollectionSettings settings;
        private readonly string databaseName;

        public GenericRepository(IConfiguration configuration, IMongoClient client)
        {
            this.client = client;
            this.settings = new MongoCollectionSettings() { AssignIdOnInsert = true };
            this.databaseName = configuration.GetSection("MongoDb:Database").Value;
        }

        public T SingleOrDefault<T>(Expression<Func<T, bool>> predicate)
        {
            var collection = this.GetCollection<T>();
            return collection.Find(predicate).SingleOrDefault();
        }

        public void Insert<T>(T obj)
        {
            var collection = this.GetCollection<T>();
            collection.InsertOne(obj);
        }

        public IEnumerable<T> All<T>()
        {
            var collection = this.GetCollection<T>();
            // TL;DR we're probably consuming lots of memory here returning all, but we need it.
            // Returning AsQueryable causes this weird case; update gold (gold = 1) returns,
            // save user executes, and gold is back to 0; probably because it's re-evaluated
            // from Mongo directly instead of preserving the in-memory changes thus far.
            return collection.AsQueryable().ToList();
        }

        public void Update<T>(T target) where T : HasId
        {
            // Kinda sorta like assumes target has an ObjectId field named Id. Also assumes that reflection
            // is sufficiently fast, even if we're doing this for a darth of users every 1s.
            var collection = this.GetCollection<T>();
            //var filter = Builders<T>.Filter.Eq(o => (o as HasId).Id, id);
            var filter = Builders<T>.Filter.Eq("Id", target.Id);
            collection.ReplaceOne(filter, target, new ReplaceOptions());
        }

        private IMongoCollection<T> GetCollection<T>()
        {
            var nameParts = typeof(T).Name.Split('.');
            var repositoryName = nameParts[nameParts.Length - 1];
            var collection = this.client.GetDatabase(this.databaseName).GetCollection<T>(repositoryName, this.settings);
            return collection;
        }
    }
}