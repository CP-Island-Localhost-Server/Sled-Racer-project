using Disney.ClubPenguin.Service.MWS.Domain;
using System.Collections.Generic;
using System.Diagnostics;

namespace Disney.ClubPenguin.SledRacer
{
	public class GameEventLogger : IGameEventLogger
	{
		private List<Disney.ClubPenguin.Service.MWS.Domain.GameEvent> gameLog;

		private Stopwatch timer;

		private SledRacerGameManager manager;

		private bool timerSuspended;

		public GameEventLogger()
		{
			manager = GameManager.GetInstanceAs<SledRacerGameManager>();
			timer = new Stopwatch();
			timer.Start();
			gameLog = new List<Disney.ClubPenguin.Service.MWS.Domain.GameEvent>();
		}

		~GameEventLogger()
		{
			timer.Stop();
		}

		public void SuspendGameTimer()
		{
			timerSuspended = true;
			timer.Stop();
		}

		public void UnsuspendGameTimer()
		{
			timerSuspended = false;
			timer.Start();
		}

		public void log(Event gameEvent, string data = null)
		{
			if (gameEvent == Event.PAUSE_START)
			{
				timer.Stop();
			}
			if (gameEvent == Event.PAUSE_END && !timerSuspended)
			{
				timer.Start();
			}
			Disney.ClubPenguin.Service.MWS.Domain.GameEvent gameEvent2 = new Disney.ClubPenguin.Service.MWS.Domain.GameEvent();
			gameEvent2.eventType = gameEvent;
			gameEvent2.score = manager.getCurrentScore();
			gameEvent2.value = data;
			gameEvent2.time = (float)timer.ElapsedMilliseconds / 1000f;
			gameLog.Add(gameEvent2);
		}

		public List<Disney.ClubPenguin.Service.MWS.Domain.GameEvent> getGameLog()
		{
			return gameLog;
		}
	}
}
