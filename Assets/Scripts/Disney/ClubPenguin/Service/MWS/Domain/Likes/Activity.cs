using Newtonsoft.Json;

namespace Disney.ClubPenguin.Service.MWS.Domain.Likes
{
	public class Activity
	{
		public class Builder
		{
			private Activity activity;

			public Builder()
			{
				activity = new Activity();
			}

			public Builder Title(string title)
			{
				activity.Title = title;
				return this;
			}

			public Builder Namespace(string aNamespace)
			{
				activity.Namespace = aNamespace;
				return this;
			}

			public Builder Actor(ActivityActor actor)
			{
				activity.Actor = actor;
				return this;
			}

			public Builder Object(ActivityObject activityObject)
			{
				activity.Object = activityObject;
				return this;
			}

			public Activity Build()
			{
				return activity;
			}
		}

		[JsonProperty("title")]
		public string Title { get; set; }

		[JsonProperty("namespace")]
		public string Namespace { get; set; }

		[JsonProperty("actor")]
		public ActivityActor Actor { get; set; }

		[JsonProperty("object")]
		public ActivityObject Object { get; set; }
	}
}
