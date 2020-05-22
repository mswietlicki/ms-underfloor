using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace MS.Underfloor.Api.Data
{
    public class StorageTempsRepository : ITempsRepository
    {
        private CloudTable _table;
        public StorageTempsRepository(string connectionString)
        {
            var account = CloudStorageAccount.Parse(connectionString);
            var tableClient = account.CreateCloudTableClient();
            _table = tableClient.GetTableReference("Temps");
            _table.CreateIfNotExistsAsync().Wait();
        }

        public async Task InsertTemp(TempReport temp)
        {
            var entity = Map(temp);
            var insertOperation = TableOperation.Insert(entity);
            await _table.ExecuteAsync(insertOperation);
        }

        private TempReportEntity Map(TempReport temp)
        {
            return new TempReportEntity(temp.Timestamp)
            {
                Temps = JsonSerializer.Serialize(temp.Temps),
                HeaterOn = temp.State == "ON"
            };
        }

        public async Task<IReadOnlyCollection<TempReport>> GetTemps(DateTime from, DateTime? to)
        {
            throw new NotImplementedException();
        }
    }
}
