using Eonet.Core.Models;
using System;

namespace NaturalEventsViewer.Domain.Tests.Data
{
	static class EonetApiResponses
	{
		public static EonetEventsResponse EonetOpenEventsResponse => new EonetEventsResponse
		{
			Title = "EONET Events",
			Description = "Natural events from EONET.",
			Link = "https://eonet.sci.gsfc.nasa.gov/api/v3/events",
			Events = new EonetEvent[] {
				new EonetEvent {
					Id = "EONET_4979",
					Title = "Wildfire - E of National City, California - United States",
					Description = null,
					Link = "https://eonet.sci.gsfc.nasa.gov/api/v3/events/EONET_4979",
					Closed = null,
					Categories = new[] {
						new EonetCategory {
							Id = "wildfires",
							Title = "Wildfires"
						}
					},
					Sources = new[] {
						new EonetSource {
							Id = "PDC",
							Url = "http://emops.pdc.org/emops/?hazard_id=113987"
						}
					},
					Geometry = new[] {
						new EonetGeometryPoint {
							MagnitudeValue = null,
							MagnitudeUnit = null,
							Date = DateTime.Parse("2020-09-06T09:39:00Z"),
							Type = EonetGeometryType.Point,
							Coordinates = new double[] { -116.721916532, 32.738763631 }
						}
					}
				},
				new EonetEvent {
					Id = "EONET_4976",
					Title = "Wildfire - NE of Fresno (Creek Fire), California - United States",
					Description = null,
					Link = "https://eonet.sci.gsfc.nasa.gov/api/v3/events/EONET_4976",
					Closed = null,
					Categories = new[] {
						new EonetCategory {
							Id = "wildfires",
							Title = "Wildfires"
						}
					},
					Sources = new[] {
						new EonetSource {
							Id = "PDC",
							Url = "http://emops.pdc.org/emops/?hazard_id=113962"
						}
					},
					Geometry = new[] {
						new EonetGeometryPoint {
							MagnitudeValue = null,
							MagnitudeUnit = null,
							Date = DateTime.Parse("2020-09-05T21:39:00Z"),
							Type = EonetGeometryType.Point,
							Coordinates = new double[] { -119.30186, 37.22187 }
						}
					}
				},
				new EonetEvent {
					Id = "EONET_4977",
					Title = "Wildfire - NE of Bozeman, Montana - United States",
					Description = null,
					Link = "https://eonet.sci.gsfc.nasa.gov/api/v3/events/EONET_4977",
					Closed = null,
					Categories = new[] {
						new EonetCategory {
							Id = "hard_wildfires",
							Title = "Hard Wildfires"
						}
					},
					Sources = new[] {
						new EonetSource {
							Id = "PDC",
							Url = "http://emops.pdc.org/emops/?hazard_id=113959"
						}
					},
					Geometry = new[] {
						new EonetGeometryPoint {
							MagnitudeValue = null,
							MagnitudeUnit = null,
							Date = DateTime.Parse("2020-09-04T21:38:00Z"),
							Type = EonetGeometryType.Point,
							Coordinates = new double[] { -110.92528361, 45.732003266 }
						}
					}
				},
				new EonetEvent {
					Id = "EONET_4978",
					Title = "Wildfire - E of San Bernardino, California - United States",
					Description = null,
					Link = "https://eonet.sci.gsfc.nasa.gov/api/v3/events/EONET_4978",
					Closed = null,
					Categories = new[] { new EonetCategory {
							Id = "light_wildfires",
							Title = "Light Wildfires"
						}
					},
					Sources = new[] {
						new EonetSource {
							Id = "PDC",
							Url = "http://emops.pdc.org/emops/?hazard_id=113950"
						}
					},
					Geometry = new[] { new EonetGeometryPoint
					{
							MagnitudeValue = null,
							MagnitudeUnit = null,
							Date = DateTime.Parse("2020-09-03T18:37:00Z"),
							Type = EonetGeometryType.Point,
							Coordinates = new double[] { -116.981639623, 34.068932369 }
						}
					}
				}
			}
		};

		public static EonetEventsResponse EonetClosedEventsResponse => new EonetEventsResponse
		{
			Title = "EONET Events",
			Description = "Natural events from EONET.",
			Link = "https://eonet.sci.gsfc.nasa.gov/api/v3/events",
			Events = new EonetEvent[] {
				new EonetEvent {
					Id = "EONET_4959",
					Title = "Ferguson Fire",
					Description = null,
					Link = "https://eonet.sci.gsfc.nasa.gov/api/v3/events/EONET_4959",
					Closed =  DateTime.Parse("2020-09-06T00:00:00Z"),
					Categories = new[] { new EonetCategory
						{
							Id = "hard_wildfires",
							Title = "Hard Wildfires"
						}
					},
					Sources = new[] { new EonetSource
						{
							Id = "InciWeb",
							Url = "http://inciweb.nwcg.gov/incident/7123/"
						}
					},
					Geometry = new[] { new EonetGeometryPoint
						{
							MagnitudeValue = null,
							MagnitudeUnit = null,
							Date =  DateTime.Parse("2020-09-05T09:39:00Z"),
							Type = EonetGeometryType.Point,
							Coordinates = new double[] { -101.376, 31.119 }
						}
					}
				},
				new EonetEvent {
					Id = "EONET_4957",
					Title = "1853 Fire",
					Description = null,
					Link = "https://eonet.sci.gsfc.nasa.gov/api/v3/events/EONET_4957",
					Closed =  DateTime.Parse("2020-09-05T00:00:00Z"),
					Categories = new[] { new EonetCategory
						{
							Id = "wildfires",
							Title = "Wildfires"
						}
					},
					Sources = new[] { new EonetSource
						{
							Id = "InciWeb",
							Url = "http://inciweb.nwcg.gov/incident/7110/"
						}
					},
					Geometry = new[] { new EonetGeometryPoint
						{
							MagnitudeValue = null,
							MagnitudeUnit = null,
							Date =  DateTime.Parse("2020-09-04T08:45:00Z"),
							Type = EonetGeometryType.Point,
							Coordinates = new double[] { -99.055999999999997, 32.606000000000002 }
						}
					}
				},
				new EonetEvent {
					Id = "EONET_4954",
					Title = "Deep Creek Fire",
					Description = null,
					Link = "https://eonet.sci.gsfc.nasa.gov/api/v3/events/EONET_4954",
					Closed =  DateTime.Parse("2020-09-05T00:00:00Z"),
					Categories = new[] { new EonetCategory
						{
							Id = "wildfires",
							Title = "Wildfires"
						}
					},
					Sources = new[] { new EonetSource
						{
							Id = "InciWeb",
							Url = "http://inciweb.nwcg.gov/incident/7112/"
						}
					},
					Geometry = new[] { new EonetGeometryPoint
						{
							MagnitudeValue = null,
							MagnitudeUnit = null,
							Date =  DateTime.Parse("2020-09-03T08:43:00Z"),
							Type = EonetGeometryType.Point,
							Coordinates = new double[] { -99.150000000000006, 32.686999999999998 }
						}
					}
				},
				new EonetEvent {
					Id = "EONET_4946",
					Title = "Sands Fire",
					Description = null,
					Link = "https://eonet.sci.gsfc.nasa.gov/api/v3/events/EONET_4946",
					Closed =  DateTime.Parse("2020-09-03T00:00:00Z"),
					Categories = new[] { new EonetCategory
						{
							Id = "wildfires",
							Title = "Wildfires"
						}
					},
					Sources = new[] { new EonetSource
						{
							Id = "LovelySource",
							Url = "http://inciweb.nwcg.gov/incident/7090/"
						}
					},
					Geometry = new[] { new EonetGeometryPoint
						{
							MagnitudeValue = null,
							MagnitudeUnit = null,
							Date =  DateTime.Parse("2020-09-02T15:45:00Z"),
							Type = EonetGeometryType.Point,
							Coordinates = new double[] { -103.119, 31.657 }
						}
					}
				}
			}
		};
	}
}
