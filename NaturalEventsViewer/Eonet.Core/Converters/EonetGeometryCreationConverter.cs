using Eonet.Core.Models;
using Newtonsoft.Json.Linq;
using System;

namespace Eonet.Core.Converters
{
    public class EonetGeometryCreationConverter : JsonCreationConverter<EonetGeometry>
    {
        protected override EonetGeometry Create(Type objectType, JObject jObject)
        {
            if (jObject == null) throw new ArgumentNullException(nameof(jObject));

            JToken typeToken;
            if (!jObject.TryGetValue("type", out typeToken) && !jObject.TryGetValue("Type", out typeToken))
            {
                throw new Exception("No 'type' property found in the object.");
            }

            string typeName = typeToken.ToString();

            EonetGeometryType type;
            if (!Enum.TryParse(typeName, out type))
            {
                throw new Exception("Failed to parse 'type' property in the object.");
            }

            switch (type)
            {
                case EonetGeometryType.Point:
                    return new EonetGeometryPoint();
                case EonetGeometryType.Polygon:
                    return new EonetGeometryPolygon();
                default:
                    return null;
            }
        }
    }
}
