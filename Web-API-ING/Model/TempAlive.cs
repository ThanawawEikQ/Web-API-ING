using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Web_API_ING.Model
{
	public class TempAlive
	{

		[BsonId]
		[BsonRepresentation(BsonType.ObjectId)]
		public string? Id { get; set; }

		public string? PC { get; set; }

		private DateTime? MaxOfDate_;
		[BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
		public DateTime? LastUpdate
		{
			get { return MaxOfDate_; }
			set
			{
				MaxOfDate_ = DateTime.SpecifyKind(value.GetValueOrDefault(), DateTimeKind.Utc);
			}
		}
	}
}
