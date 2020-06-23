using System;
using System.Linq;
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
                HeaterLimitTemp = new AgentConfig().HeaterLimitTemp,
                HeaterOn = temp.State == "ON"
            };
        }
        private TempReport Map(TempReportEntity entity)
        {
            TimeZoneInfo easternZone;
            try
            {
                easternZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
            }
            catch (TimeZoneNotFoundException)
            {
                easternZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Warsaw");
            }

            return new TempReport
            {
                State = entity.HeaterOn ? "ON" : "OFF",
                HeaterLimitTemp = entity.HeaterLimitTemp,
                Temps = JsonSerializer.Deserialize<double[]>(entity.Temps),
                Timestamp = TimeZoneInfo.ConvertTimeFromUtc(entity.Timestamp.DateTime, easternZone)
            };
        }

        public async Task<IReadOnlyCollection<TempReport>> GetTemps(DateTime from, DateTime? to)
        {
            var query = new TableQuery<TempReportEntity>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, from.ToString("ddMMyyyy")));

            var tempEntities = new List<TempReportEntity>();
            TableContinuationToken continuationToken = null;
            do
            {
                var result = await _table.ExecuteQuerySegmentedAsync(query, continuationToken);
                tempEntities.AddRange(result);
                continuationToken = result.ContinuationToken;
            } while (continuationToken != null);
            return tempEntities.Select(Map).ToList();
        }
    }
}
