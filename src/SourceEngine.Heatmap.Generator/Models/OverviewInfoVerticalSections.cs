using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SourceEngine.Heatmap.Generator.Models
{
	[DataContract]
	public class OverviewInfoVerticalSections
	{
		[DataMember(Name = "upper")]
		public OverviewInfoVerticalSectionsSpecific Upper { get; set; }

		[DataMember(Name = "default")]
		public OverviewInfoVerticalSectionsSpecific Default { get; set; }

		[DataMember(Name = "lower")]
		public OverviewInfoVerticalSectionsSpecific Lower { get; set; }


		public OverviewInfoVerticalSections() { }
	}
}
