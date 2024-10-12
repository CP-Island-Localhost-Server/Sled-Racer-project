using Disney.ClubPenguin.Service.MWS.Domain;
using System.Collections.Generic;

namespace Disney.ClubPenguin.SledRacer
{
	public class NullGameEventLogger : IGameEventLogger
	{
		public void log(Event gameEvent, string data = null)
		{
		}

		public List<Disney.ClubPenguin.Service.MWS.Domain.GameEvent> getGameLog()
		{
			return null;
		}

		public void SuspendGameTimer()
		{
		}

		public void UnsuspendGameTimer()
		{
		}
	}
}
