using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Disney.ClubPenguin.Service.MWS.Domain
{
	public class SoundStudioTrackData
	{
		private class SortLastModifiedHelper : IComparer<SoundStudioTrackData>
		{
			public int Compare(SoundStudioTrackData track1, SoundStudioTrackData track2)
			{
				if (track1.LastModified > track2.LastModified)
				{
					return 1;
				}
				if (track1.LastModified < track2.LastModified)
				{
					return -1;
				}
				return 0;
			}
		}

		private class SortByNameAscending : IComparer<SoundStudioTrackData>
		{
			public int Compare(SoundStudioTrackData track1, SoundStudioTrackData track2)
			{
				return track1.Name.CompareTo(track2.Name);
			}
		}

		private static readonly DateTime unixDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

		public static IComparer<SoundStudioTrackData> SortLastModfiedAscending = new SortLastModifiedHelper();

		public static IComparer<SoundStudioTrackData> SortNameAscending = new SortByNameAscending();

		private TrackShareState trackShareState;

		[JsonProperty("data")]
		public string Data { get; set; }

		[JsonProperty("lastModified")]
		public long LastModified { get; set; }

		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("playerId")]
		public long PlayerId { get; set; }

		[JsonProperty("shareState")]
		public int JSONShareState
		{
			get
			{
				return (int)trackShareState;
			}
			set
			{
				trackShareState = (TrackShareState)value;
			}
		}

		[JsonIgnore]
		public TrackShareState TrackShareState
		{
			get
			{
				return trackShareState;
			}
			set
			{
				trackShareState = value;
			}
		}

		[JsonProperty("trackId")]
		public long TrackId { get; set; }

		public DateTime GetLastModifieDateTime()
		{
			return unixDateTime.AddSeconds((double)LastModified / 1000.0).ToLocalTime();
		}

		protected bool Equals(SoundStudioTrackData other)
		{
			return LastModified == other.LastModified && string.Equals(Name, other.Name) && PlayerId == other.PlayerId && TrackId == other.TrackId && trackShareState == other.trackShareState;
		}

		public override bool Equals(object obj)
		{
			if (object.ReferenceEquals(null, obj))
			{
				return false;
			}
			if (object.ReferenceEquals(this, obj))
			{
				return true;
			}
			if (obj.GetType() != GetType())
			{
				return false;
			}
			return Equals((SoundStudioTrackData)obj);
		}

		public override int GetHashCode()
		{
			int hashCode = LastModified.GetHashCode();
			hashCode = (hashCode * 397) ^ ((Name != null) ? Name.GetHashCode() : 0);
			hashCode = (hashCode * 397) ^ PlayerId.GetHashCode();
			hashCode = (hashCode * 397) ^ TrackId.GetHashCode();
			return (hashCode * 397) ^ (int)trackShareState;
		}

		public override string ToString()
		{
			return string.Format("Name: {0}, PlayerId: {1}, TrackShareState: {2}, TrackId: {3}, LastModified: {4}", Name, PlayerId, trackShareState, TrackId, GetLastModifieDateTime());
		}
	}
}
