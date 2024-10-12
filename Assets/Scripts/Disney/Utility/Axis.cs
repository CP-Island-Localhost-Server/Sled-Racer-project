using UnityEngine;

namespace Disney.Utility
{
	public class Axis : MonoBehaviour
	{
		public float Min = -1f;

		public float Center;

		public float Max = 1f;

		public float AxisSpeed = 0.33f;

		public float counterForceStrength = 1f;

		protected Actuator minActuator;

		protected Actuator maxActuator;

		private float previousValue;

		public Axis()
		{
			DefaultAxis();
		}

		public Axis(float _counterforce)
		{
			counterForceStrength = _counterforce;
			DefaultAxis();
		}

		private void DefaultAxis()
		{
			minActuator = new Actuator(Center, Min, AxisSpeed / 2f);
			maxActuator = new Actuator(Center, Max, AxisSpeed / 2f);
			minActuator.Engage();
			maxActuator.Engage();
		}

		public virtual float GetValue(int energy)
		{
			float num = 0f;
			float deltaTime = Time.deltaTime;
			num = ((energy < 0) ? ((maxActuator.CurrentValue != Center) ? maxActuator.GetValue(deltaTime * counterForceStrength, energy) : ((minActuator.CurrentValue == Min) ? Min : minActuator.GetValue(deltaTime, -energy))) : ((energy > 0) ? ((minActuator.CurrentValue != Center) ? minActuator.GetValue(deltaTime * counterForceStrength, -energy) : ((maxActuator.CurrentValue == Max) ? Max : maxActuator.GetValue(deltaTime, energy))) : ((maxActuator.CurrentValue != Center) ? maxActuator.GetValue(deltaTime, -1) : ((minActuator.CurrentValue == Center) ? Center : minActuator.GetValue(deltaTime, -1)))));
			if (num != previousValue)
			{
				previousValue = num;
			}
			return num;
		}

		public virtual float GetValueAt(float time)
		{
			float num = 0f;
			if (time < 0f)
			{
				return minActuator.GetValueAt(0f - time);
			}
			if (time > 0f)
			{
				return maxActuator.GetValueAt(time);
			}
			return Center;
		}

		public void Reset()
		{
		}
	}
}
