using Fabric;

namespace Disney.ClubPenguin.SledRacer
{
	public interface IAudioGroup
	{
		string Name
		{
			get;
		}

		GroupComponent Group
		{
			get;
		}

		float Volume
		{
			get;
			set;
		}

		bool IsMuted();

		void Mute();

		void UnMute();
	}
}
