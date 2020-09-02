using System;

namespace Eonet.Core.Models
{
    public class EonetGeometry
    {
        public DateTime Date { get; set; }
        public EonetGeometryType Type { get; set; }
        public double? MagnitudeValue { get; set; }
        public string MagnitudeUnit { get; set; }
    }
}
