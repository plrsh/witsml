﻿//----------------------------------------------------------------------- 
// PDS.Witsml.Server, 2016.1
//
// Copyright 2016 Petrotechnical Data Systems
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//   
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using PDS.Framework;
using PDS.Witsml.Server.Models;

namespace PDS.Witsml.Server.Data.Transactions
{
    /// <summary>
    /// Encapsulates transaction-like behavior on MongoDb
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class MongoTransaction : IDisposable
    {
        private static readonly string _idField = "_id";
        private static readonly string _uidWell = "UidWell";
        private static readonly string _uidWellbore = "UidWellbore";

        /// <summary>
        /// Gets or sets the transaction identifier.
        /// </summary>
        /// <value>The tid.</value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="MongoTransaction"/> is committed.
        /// </summary>
        /// <value><c>true</c> if committed; otherwise, <c>false</c>.</value>
        public bool Committed { get; set; }

        /// <summary>
        /// The list of transaction records. 
        /// </summary>
        public List<MongoDbTransaction> Transactions;
          
        public IDatabaseProvider DatabaseProvider { get; }

        public MongoDbTransactionAdapter Adapter { get; }

        [ImportingConstructor]
        public MongoTransaction(IDatabaseProvider databaseProvider, MongoDbTransactionAdapter adapter)
        {
            DatabaseProvider = databaseProvider;
            Adapter = adapter;
            Id = Guid.NewGuid().ToString();
            Transactions = new List<MongoDbTransaction>();
            Committed = false;
        }

        /// <summary>
        /// Commits the transaction in MongoDb.
        /// </summary>
        public void Commit()
        {
            var database = DatabaseProvider.GetDatabase();
            foreach (var transaction in Transactions.Where(t => t.Status == TransactionStatus.Pending && t.Action == MongoDbAction.Delete))
            {
                DeleteADocument(database, transaction);
            }

            ClearTransactions();
            Committed = true;
        }

        /// <summary>
        /// Rollbacks the transaction in MongoDb.
        /// </summary>
        public void Rollback()
        {
            var pending = Transactions.Where(t => t.Status == TransactionStatus.Pending).ToList();
            if (!pending.Any())
                return;

            var database = DatabaseProvider.GetDatabase();
            foreach (var transaction in pending)
            {
                var action = transaction.Action;

                if (action == MongoDbAction.Add)
                {
                    DeleteADocument(database, transaction);
                }
                else if (action == MongoDbAction.Update)
                {
                    UpdateADocument(database, transaction);
                }
            }

            ClearTransactions();
        }

        /// <summary>
        /// Creates a transaction record and add to the collection.
        /// </summary>
        /// <param name="action">The MongoDb operation, e.g. add.</param>
        /// <param name="collection">The MongoDb collection name.</param>
        /// <param name="document">The data obejct in BsonDocument format.</param>
        public void AddATransaction(MongoDbAction action, string collection, BsonDocument document)
        {
            var transaction = new MongoDbTransaction
            {
                TransactionId = Id,
                Collection = collection,
                Action = action,
                Status = TransactionStatus.Created
            };
            if (document != null)
                transaction.Value = document;

            Transactions.Add(transaction);
        }

        /// <summary>
        /// Creates the transaction records in MongoDb and change status to pending
        /// </summary>
        public void AddTransactions()
        {
            var created = Transactions.Where(t => t.Status == TransactionStatus.Created).ToList();
            if (!created.Any())
                return;

            foreach (var transaction in created)
                transaction.Status = TransactionStatus.Pending;

            Adapter.InsertEntities(created);
        }

        /// <summary>
        /// Rollbacks the MongoDb operations if the transaction is not commited when disposed.
        /// </summary>
        public void Dispose()
        {
            if (!Committed)
                Rollback();
        }

        private void ClearTransactions()
        {
            if (!Transactions.Any())
                return;

            Adapter.DeleteTransactions(Id);
            Transactions.Clear();
        }

        private void UpdateADocument(IMongoDatabase database, MongoDbTransaction transaction)
        {
            var collection = database.GetCollection<BsonDocument>(transaction.Collection);
            var filter = GetDocumentFilter(transaction.Value);
            collection.ReplaceOne(filter, transaction.Value);
        }

        private void DeleteADocument(IMongoDatabase database, MongoDbTransaction transaction)
        {
            var collection = database.GetCollection<BsonDocument>(transaction.Collection);
            var filter = GetDocumentFilter(transaction.Value);
            collection.DeleteOne(filter);
        }

        private FilterDefinition<BsonDocument> GetDocumentFilter(BsonDocument document)
        {
            var filters = new List<FilterDefinition<BsonDocument>>();
            if (document.Contains(ObjectTypes.Uid))
            {
                filters.Add(MongoDbUtility.BuildFilter<BsonDocument>(ObjectTypes.Uid, document[ObjectTypes.Uid].ToString()));
                if (document.Contains(_uidWell))
                {
                    filters.Add(MongoDbUtility.BuildFilter<BsonDocument>(_uidWell, document[_uidWell].ToString()));
                    if (document.Contains(_uidWellbore))
                    {
                        filters.Add(MongoDbUtility.BuildFilter<BsonDocument>(_uidWellbore, document[_uidWellbore].ToString()));
                    }
                }
            }
            else if (document.Contains(ObjectTypes.Uuid))
            {
                filters.Add(MongoDbUtility.BuildFilter<BsonDocument>(ObjectTypes.Uuid, document[ObjectTypes.Uuid]).ToString());
            }
            else if (document.Contains(ObjectTypes.Id))
            {
                filters.Add(MongoDbUtility.BuildFilter<BsonDocument>(ObjectTypes.Id, document[ObjectTypes.Id]));
            }
            else
            {
                filters.Add(MongoDbUtility.BuildFilter<BsonDocument>(_idField, document[_idField]));
            }

            return filters.Count > 0
                ? Builders<BsonDocument>.Filter.And(filters)
                : null;
        }
    }
}