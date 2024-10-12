using Disney.ClubPenguin.Service.MWS.Domain;
using System.Collections.Generic;

namespace Disney.ClubPenguin.SledRacer
{
	public interface IGameEventLogger
	{
		void log(Event gameEvent, string data = null);

		List<Disney.ClubPenguin.Service.MWS.Domain.GameEvent> getGameLog();

		void SuspendGameTimer();

		void UnsuspendGameTimer();
	}
}
