using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Web_API_ING.Model
{
	public class TempObj
	{


		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string? Id { get; set; }
		public double Temperature { get; set; }
		public double Humidity { get; set; }
		public string PC { get; set; }
		private DateTime? MaxOfDate_;
		[BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
		public DateTime? Creation
		{
			get { return MaxOfDate_; }
			set
			{
				MaxOfDate_ = DateTime.SpecifyKind(value.GetValueOrDefault(), DateTimeKind.Utc);
			}
		}

	}
}
