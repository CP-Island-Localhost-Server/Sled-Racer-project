using System;
using System.Reflection;
using UnityEngine;

namespace Disney.ClubPenguin.SledRacer
{
	public class PlayerStateObject : MonoBehaviour
	{
		protected PlayerController player;

		protected Animator stateAnimator;

		public event Action OnEnterStateEvent;

		public event Action OnExitStateEvent;

		private void Awake()
		{
			player = GetComponentInParent<PlayerController>();
			stateAnimator = GetComponentInChildren<Animator>();
			base.gameObject.SetActive(value: false);
			OnAwake();
		}

		protected virtual void OnAwake()
		{
		}

		protected virtual void DispatchEnterStateEvent()
		{
			if (this.OnEnterStateEvent != null)
			{
				this.OnEnterStateEvent();
			}
		}

		protected virtual void DispatchExitStateEvent()
		{
			if (this.OnExitStateEvent != null)
			{
				this.OnExitStateEvent();
			}
		}

		public virtual void EnterState()
		{
			base.gameObject.SetActive(value: true);
			DispatchEnterStateEvent();
		}

		protected virtual void ExitState()
		{
			base.gameObject.SetActive(value: false);
			DispatchExitStateEvent();
		}

		public virtual void AbortState()
		{
			base.gameObject.SetActive(value: false);
		}

		protected void DevTrace(string _msg)
		{
			UnityEngine.Debug.Log("[" + MethodBase.GetCurrentMethod().ReflectedType.Name + "] " + _msg);
		}
	}
}
