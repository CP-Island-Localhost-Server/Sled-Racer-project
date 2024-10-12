namespace Disney.ClubPenguin.Service.MWS.Domain.Likes
{
	public class EntityClass
	{
		public string Context { get; set; }

		public string Type { get; set; }

		public Periodicity Periodicity { get; set; }

		public int Period { get; set; }

		public int Offset { get; set; }

		public bool Unlikable { get; set; }
	}
}
