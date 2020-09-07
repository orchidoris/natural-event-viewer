using NaturalEventsViewer.Domain;
using NaturalEventsViewer.Web.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace NaturalEventsViewer.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IEonetRepository _eonetRepository;
        private readonly int _maxDaysPrior;

        public HomeController(IEonetRepository eonetRepository, int maxDaysPrior)
        {
            _eonetRepository = eonetRepository;
            _maxDaysPrior = maxDaysPrior;
        }

        public async Task<ActionResult> Index()
        {
            ViewBag.Version = GetVersion();
            ViewBag.Title = "Natural Events on Earth";            
            return View("~/Views/Home/Index.cshtml", (object)await GetInitialReduxStateJsonString());
        }

        private async Task<string> GetInitialReduxStateJsonString()
        {
            var initialReduxState = new ReduxApplicationState
            {
                Global = new ReduxGlobalState
                {
                    Sources = await _eonetRepository.GetCurrentlyAvailableSourceIds(),
                    Categories = await _eonetRepository.GetCurrentlyAvailableCategories(),
                    MaxDaysPrior = 60 //_maxDaysPrior,
                }
            };

            string initialReduxStateJsonString = JsonConvert.SerializeObject(
                initialReduxState,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

            return initialReduxStateJsonString;
        }

        private string GetVersion()
        {
            var assembly = typeof(HomeController).Assembly;
            return assembly.GetName().Version.ToString();
        }
    }
}
