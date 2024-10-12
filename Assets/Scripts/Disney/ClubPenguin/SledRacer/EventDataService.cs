using System;

namespace Disney.ClubPenguin.SledRacer
{
	public sealed class EventDataService : IDataService
	{
		public event EventHandler<UIEvent> OnUIEvent;

		public void SendUIEvent(object sender, UIEvent e)
		{
			if (this.OnUIEvent != null)
			{
				this.OnUIEvent(sender, e);
			}
		}
	}
}
