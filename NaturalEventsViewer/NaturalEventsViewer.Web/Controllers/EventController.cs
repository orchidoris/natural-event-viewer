using Eonet.Core;
using Eonet.Core.Models;
using NaturalEventsViewer.Domain;
using NaturalEventsViewer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace NaturalEventsViewer.Web.Controllers
{
    public class EventController : ApiController
    {
        private IEonetRepository _eonetRepository;

        public EventController(IEonetRepository eonetRepository) {
            _eonetRepository = eonetRepository;
        }

        [HttpPost]
        public async Task<EonetEventsResponse> GetEventList([FromBody]EonetEventsCustomRequest request)
        {
            var res = await _eonetRepository.GetEvents(request);
            return res;
        }

        [HttpGet]
        public async Task<EonetEvent> GetEvent(string id)
        {
            var res = await _eonetRepository.GetSingeEvent(id);
            return res;
        }
    }
}
