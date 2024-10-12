using System;

namespace Disney.ClubPenguin.SledRacer
{
	public class GameEvent : EventArgs
	{
		public enum Type
		{
			Invalid,
			End,
			LeftStart,
			LeftEnd,
			RightStart,
			RightEnd,
			Both,
			Paused,
			TutorialProgress,
			Unpaused
		}

		private readonly Type eventType;

		private readonly int dataInt = -1;

		public Type type => eventType;

		public int intData => dataInt;

		public GameEvent(Type _type)
		{
			eventType = _type;
		}

		public GameEvent(Type _type, int _dataInt)
		{
			eventType = _type;
			dataInt = _dataInt;
		}
	}
}
