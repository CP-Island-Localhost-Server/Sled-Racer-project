namespace Disney.ClubPenguin.Service.MWS.Domain
{
	public class Outfit
	{
		public int colour;

		public int head;

		public int face;

		public int neck;

		public int body;

		public int hand;

		public int feet;

		public int flag;

		public int photo;

		public string ToDelimitedString(string delimiter = "|")
		{
			return string.Join(delimiter, new string[9]
			{
				colour.ToString(),
				head.ToString(),
				face.ToString(),
				neck.ToString(),
				body.ToString(),
				hand.ToString(),
				feet.ToString(),
				flag.ToString(),
				photo.ToString()
			});
		}

		public static Outfit FromDelimitedString(string outfitString, string delimiter = "|")
		{
			return new Outfit();
		}
	}
}
