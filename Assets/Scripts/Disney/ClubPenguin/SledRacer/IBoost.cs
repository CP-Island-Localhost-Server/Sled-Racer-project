using UnityEngine;

namespace Disney.ClubPenguin.SledRacer
{
	public interface IBoost
	{
		BoostType BoostPhase
		{
			get;
		}

		bool Active
		{
			get;
		}

		bool Ending
		{
			get;
		}

		bool Used
		{
			get;
		}

		void Execute();

		Vector3 FixedUpdate();

		void Update();

		void DrawGizmos();

		void Trigger();

		void Complete();

		void Abort();

		void Destroy();
	}
}
