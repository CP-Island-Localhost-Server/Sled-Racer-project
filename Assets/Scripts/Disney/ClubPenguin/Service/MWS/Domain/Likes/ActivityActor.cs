using Newtonsoft.Json;

namespace Disney.ClubPenguin.Service.MWS.Domain.Likes
{
	public class ActivityActor
	{
		public class Builder
		{
			private ActivityActor activityActor;

			public Builder()
			{
				activityActor = new ActivityActor();
			}

			public Builder Id(string id)
			{
				activityActor.Id = id;
				return this;
			}

			public Builder ObjectType(string objectType)
			{
				activityActor.ObjectType = objectType;
				return this;
			}

			public Builder DisplayName(string displayName)
			{
				activityActor.DisplayName = displayName;
				return this;
			}

			public ActivityActor Build()
			{
				return activityActor;
			}
		}

		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("objectType")]
		public string ObjectType { get; set; }

		[JsonProperty("displayName")]
		public string DisplayName { get; set; }
	}
}
