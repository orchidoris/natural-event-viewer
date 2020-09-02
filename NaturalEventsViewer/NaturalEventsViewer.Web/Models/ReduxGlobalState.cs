using Eonet.Core.Models;

namespace NaturalEventsViewer.Web.Models
{
    public class ReduxGlobalState
    {
        public string[] Sources { get; set; }
        public EonetCategory[] Categories { get; set; }
    }
}