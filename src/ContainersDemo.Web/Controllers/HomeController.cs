using ContainersDemo.Model;
using ContainersDemo.Web.Models;
using Microsoft.AspNet.Mvc;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ContainersDemo.Web.Controllers
{
    public class HomeController : Controller
    {
        // TODO: Provide your own storage connection string here.
        private const string StorageConnectionString = "";
        private const string StorageQueueName = "registrations";

        public IActionResult Index()
        {
            return View(new RegistrationViewModel());
        }
        
        [HttpPost]
        public async Task<IActionResult> Index(RegistrationViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var registration = ToRegistration(viewModel);
                var registrationJson = JsonConvert.SerializeObject(registration);
                var cloudStorageAccount = CloudStorageAccount.Parse(StorageConnectionString);
                var cloudQueueClient = cloudStorageAccount.CreateCloudQueueClient();
                var cloudQueue = cloudQueueClient.GetQueueReference(StorageQueueName);

                await cloudQueue.CreateIfNotExistsAsync();
                await cloudQueue.AddMessageAsync(new CloudQueueMessage(registrationJson));

                viewModel.SuccessMessage = "Thank you for registering!";
            }

            return View(viewModel);
        }

        private Registration ToRegistration(RegistrationViewModel viewModel)
        {
            return new Registration
            {
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                EmailAddress = viewModel.EmailAddress,
                State = viewModel.State
            };
        }
    }
}
