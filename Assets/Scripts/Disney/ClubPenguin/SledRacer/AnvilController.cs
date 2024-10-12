using Disney.Utility;
using System;
using System.Collections;
using UnityEngine;

namespace Disney.ClubPenguin.SledRacer
{
	public class AnvilController : MonoBehaviour
	{
		private enum AnvilState
		{
			Idle,
			Moving
		}

		private Vector3 LoPoint;

		private Vector3 HiPoint;

		private int Energy;

		private ConfigController config;

		protected Actuator MotionActuator;

		private AnvilState state;

		private void Awake()
		{
			config = Service.Get<ConfigController>();
			LoPoint = base.transform.localPosition;
			HiPoint = LoPoint + config.AnvilMotion;
			MotionActuator = new Actuator(LoPoint, HiPoint, config.AnvilSlideDuration);
		}

		private void OnEnable()
		{
			Delay();
			switch (UnityEngine.Random.Range(1, 3))
			{
			case 1:
				base.transform.localPosition = LoPoint;
				Energy = -1;
				break;
			case 2:
				base.transform.localPosition = HiPoint;
				Energy = 1;
				MotionActuator.CurrentTime = 1f;
				break;
			}
			MotionActuator.Engage();
		}

		private void FixedUpdate()
		{
			if (state == AnvilState.Moving)
			{
				Vector3 vector = MotionActuator.GetVector(Time.deltaTime, Energy);
				base.transform.localPosition = vector;
				if (MotionActuator.CurrentTime == 0f || MotionActuator.CurrentTime == 1f)
				{
					Delay();
				}
			}
		}

		private void Delay()
		{
			state = AnvilState.Idle;
			StartCoroutine(Idle(config.AnvilIdleTime, NextMotion));
		}

		private IEnumerator Idle(float _time, Action _callback)
		{
			yield return new WaitForSeconds(_time);
			_callback();
		}

		private void NextMotion()
		{
			Energy *= -1;
			state = AnvilState.Moving;
		}
	}
}
