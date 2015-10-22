using Microsoft.WindowsAzure.Storage.Table;

namespace ContainersDemo.StorageService
{
    public class RegistrationTableEntity : TableEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
