using Newtonsoft.Json;

namespace Disney.ClubPenguin.Service.MWS.Domain.Likes
{
	public class ActivityObject
	{
		public class Builder
		{
			private ActivityObject activityObject;

			public Builder()
			{
				activityObject = new ActivityObject();
			}

			public Builder Id(string id)
			{
				activityObject.Id = id;
				return this;
			}

			public Builder OwnerId(string ownerId)
			{
				activityObject.OwnerId = ownerId;
				return this;
			}

			public Builder ObjectType(string objectType)
			{
				activityObject.ObjectType = objectType;
				return this;
			}

			public Builder DisplayName(string displayName)
			{
				activityObject.DisplayName = displayName;
				return this;
			}

			public ActivityObject Build()
			{
				return activityObject;
			}
		}

		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("ownerID")]
		public string OwnerId { get; set; }

		[JsonProperty("objectType")]
		public string ObjectType { get; set; }

		[JsonProperty("displayName")]
		public string DisplayName { get; set; }
	}
}
