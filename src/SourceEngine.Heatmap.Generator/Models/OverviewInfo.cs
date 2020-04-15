using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;

namespace SourceEngine.Heatmap.Generator.Models
{
	[DataContract]
	[KnownType(typeof(OverviewInfo))]
	public class OverviewInfo
	{
		[DataMember(Name = "material")]
		public string Material { get; set; }

		[DataMember(Name = "pos_x")]
		public float OffsetX { get; set; }

		[DataMember(Name = "pos_y")]
		public float OffsetY { get; set; }

		[DataMember(Name = "scale")]
		public float Scale { get; set; }

		[DataMember(Name = "rotate")]
		public float? Rotate { get; set; }

		[DataMember(Name = "zoom")]
		public float? Zoom { get; set; }


		[DataMember(Name = "verticalsections")]
		public OverviewInfoVerticalSections VerticalSections { get; set; }

		[DataMember(Name = "CTSpawn_x")]
		public float? CTSpawnX { get; set; }

		[DataMember(Name = "CTSpawn_y")]
		public float? CTSpawnY { get; set; }

		[DataMember(Name = "TSpawn_x")]
		public float? TSpawnX { get; set; }

		[DataMember(Name = "TSpawn_y")]
		public float? TSpawnY { get; set; }


		[DataMember(Name = "bombA_x")]
		public float? ABombsiteX { get; set; }

		[DataMember(Name = "bombA_y")]
		public float? ABombsiteY { get; set; }

		[DataMember(Name = "bombB_x")]
		public float? BBombsiteX { get; set; }

		[DataMember(Name = "bombB_y")]
		public float? BBombsiteY { get; set; }


		public OverviewInfo() { }
	}
}
