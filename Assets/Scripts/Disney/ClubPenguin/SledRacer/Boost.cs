using UnityEngine;

namespace Disney.ClubPenguin.SledRacer
{
	public abstract class Boost : IBoost
	{
		protected PlayerController player;

		protected BoostType myPhase;

		protected bool active;

		protected bool ending;

		protected bool used;

		public BoostType BoostPhase => myPhase;

		public bool Active => active;

		public bool Ending => ending;

		public virtual bool Used => used;

		public Boost(PlayerController _player)
		{
			player = _player;
		}

		public virtual void Execute()
		{
		}

		public virtual Vector3 FixedUpdate()
		{
			return player.AppliedForces;
		}

		public virtual void Update()
		{
		}

		public virtual void DrawGizmos()
		{
		}

		public virtual void Trigger()
		{
		}

		public virtual void Complete()
		{
		}

		protected void DevTrace(string msg)
		{
			Debug.Log (msg);
		}

		public virtual void Abort()
		{
		}

		public virtual void Destroy()
		{
		}
	}
}
