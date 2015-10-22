using ContainersDemo.Model;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace ContainersDemo.StorageService
{
    public class Program
    {
        // TODO: Provide your own storage connection string here.
        private const string StorageConnectionString = "";
        private const string StorageQueueName = "registrations";
        private const string StorageTableName = "registrations";

        public async Task Main(string[] args)
        {
            var cloudStorageAccount = CloudStorageAccount.Parse(StorageConnectionString);
            var cloudQueueClient = cloudStorageAccount.CreateCloudQueueClient();
            var cloudTableClient = cloudStorageAccount.CreateCloudTableClient();
            var cloudQueue = cloudQueueClient.GetQueueReference(StorageQueueName);
            var cloudTable = cloudTableClient.GetTableReference(StorageTableName);

            await cloudQueue.CreateIfNotExistsAsync();
            await cloudTable.CreateIfNotExistsAsync();

            while (true)
            {
                var message = await cloudQueue.GetMessageAsync();

                if (message != null)
                {
                    var registration = JsonConvert.DeserializeObject<Registration>(message.AsString);
                    var registrationTableEntity = ToRegistrationTableEntity(registration);
                    var tableOperation = TableOperation.InsertOrReplace(registrationTableEntity);

                    await cloudTable.ExecuteAsync(tableOperation);
                    await cloudQueue.DeleteMessageAsync(message);

                    Console.WriteLine($"Added registration {message.AsString} to table storage.");
                    Console.WriteLine();
                }
            }
        }

        private RegistrationTableEntity ToRegistrationTableEntity(Registration registration)
        {
            return new RegistrationTableEntity
            {
                RowKey = registration.EmailAddress,
                PartitionKey = registration.State,
                FirstName = registration.FirstName,
                LastName = registration.LastName
            };
        }
    }
}
