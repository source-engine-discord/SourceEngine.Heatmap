using System.Runtime.Serialization;

namespace SourceEngine.Heatmap.Generator.Models
{
	[DataContract]
	public class OverviewInfoVerticalSectionsSpecific
	{
		[DataMember(Name = "AltitudeMax")]
		public float AltitudeMax { get; set; }

		[DataMember(Name = "AltitudeMin")]
		public float AltitudeMin { get; set; }


		public OverviewInfoVerticalSectionsSpecific() { }
	}
}
