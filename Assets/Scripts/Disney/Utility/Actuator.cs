using UnityEngine;

namespace Disney.Utility
{
	public class Actuator
	{
		public enum ActuatorState
		{
			OFF,
			ON,
			PAUSED
		}

		public enum ActuatorMode
		{
			STATIC,
			DYNAMIC
		}

		public Vector3 VMin = Vector3.zero;

		public Vector3 VMax = Vector3.one;

		public float Min;

		public float Max = 1f;

		public float ActuatorDuration = 1f;

		public static int OUTPUT_NORMAL = 1;

		public static int OUTPUT_INVERTED = -1;

		private ActuatorState state;

		private float currentTime;

		private float currentValue;

		private Vector3 currentVector;

		private float speedFactor;

		public float CurrentValue => currentValue;

		public Vector3 CurrentVector => currentVector;

		public float CurrentTime
		{
			get
			{
				return currentTime;
			}
			set
			{
				currentTime = ((value > 1f) ? 1f : ((!(value < 0f)) ? value : 0f));
			}
		}

		public Actuator(float _min, float _max, float _duration)
		{
			Min = _min;
			Max = _max;
			ActuatorDuration = _duration;
			state = ActuatorState.OFF;
			Reset();
		}

		public Actuator(Vector3 _min, Vector3 _max, float _duration)
		{
			VMin = _min;
			VMax = _max;
			ActuatorDuration = _duration;
			state = ActuatorState.OFF;
			Reset();
		}

		public Actuator clone(int _outputFormat)
		{
			return new Actuator(Min, Max, ActuatorDuration);
		}

		public float GetValue(float _deltaTime, int energy)
		{
			if (state == ActuatorState.ON)
			{
				float num = _deltaTime * speedFactor;
				currentTime = ((currentTime > 1f) ? 1f : ((!(currentTime < 0f)) ? Mathf.Min(1f, Mathf.Max(0f, currentTime + num * (float)energy)) : 0f));
				currentValue = Mathf.Lerp(Min, Max, currentTime);
			}
			return currentValue;
		}

		public float GetValueAt(float _deltaTime)
		{
			return Mathf.Lerp(Min, Max, _deltaTime);
		}

		public Vector3 GetVector(float _deltaTime, int energy)
		{
			if (state == ActuatorState.ON)
			{
				float num = _deltaTime * speedFactor;
				currentTime = ((currentTime > 1f) ? 1f : ((!(currentTime < 0f)) ? Mathf.Min(1f, Mathf.Max(0f, currentTime + num * (float)energy)) : 0f));
				currentVector = Vector3.Lerp(VMin, VMax, currentTime);
			}
			return currentVector;
		}

		public void Engage()
		{
			state = ActuatorState.ON;
		}

		public void Pause()
		{
			state = ActuatorState.PAUSED;
		}

		public void Reset()
		{
			Disengage();
			currentValue = Min;
			currentVector = VMin;
			currentTime = 0f;
			speedFactor = 1f / ActuatorDuration;
		}

		public void Disengage()
		{
			state = ActuatorState.OFF;
		}

		private void traceOut(string _msg)
		{
		}
	}
}
