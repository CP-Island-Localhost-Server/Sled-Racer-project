namespace Disney.ClubPenguin.Service.MWS.Domain.Likes
{
	public class PaginationInfo
	{
		private int? start = 1;

		private int? limit = 100;

		public int? Start
		{
			get
			{
				return start;
			}
			set
			{
				start = value;
			}
		}

		public int? Limit
		{
			get
			{
				return limit;
			}
			set
			{
				limit = value;
			}
		}

		public Orderby Orderby { get; set; }

		public Groupby Groupby { get; set; }

		public PaginationInfo()
		{
			Orderby = Orderby.none;
			Groupby = Groupby.none;
		}
	}
}
