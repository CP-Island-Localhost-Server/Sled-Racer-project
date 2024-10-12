using System.Collections.Generic;

namespace Disney.ClubPenguin.Service.MWS.Domain
{
	public class GameResult
	{
		public long gameId { get; set; }

		public string gameType { get; set; }

		public long playerId { get; set; }

		public List<PlayerResult> playerResults { get; set; }

		public List<TeamResult> teamResults { get; set; }

		public List<GameEvent> gameEvents { get; set; }

		public Dictionary<string, object> achievements { get; set; }

		public Dictionary<string, object> rewards { get; set; }

		public long start { get; set; }

		public long end { get; set; }
	}
}
