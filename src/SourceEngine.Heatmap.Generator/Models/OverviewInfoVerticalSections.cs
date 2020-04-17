using System.Runtime.Serialization;

namespace SourceEngine.Heatmap.Generator.Models
{
	[DataContract]
	public class OverviewInfoVerticalSections
	{
		private OverviewInfoVerticalSectionsSpecific upper;
		private OverviewInfoVerticalSectionsSpecific lower;

		[DataMember(Name = "upper")]
		public OverviewInfoVerticalSectionsSpecific Upper
		{
			get { return upper; }
			set { upper = value; }
		}

		[DataMember(Name = "layer1")]
		public OverviewInfoVerticalSectionsSpecific Layer1
		{
			get { return upper; }
			set { upper = value; }
		}

		[DataMember(Name = "default")]
		public OverviewInfoVerticalSectionsSpecific Default { get; set; }

		[DataMember(Name = "lower")]
		public OverviewInfoVerticalSectionsSpecific Lower
		{
			get { return lower; }
			set { lower = value; }
		}

		[DataMember(Name = "layer0")]
		public OverviewInfoVerticalSectionsSpecific Layer0
		{
			get { return lower; }
			set { lower = value; }
		}


		public OverviewInfoVerticalSections() { }
	}
}
