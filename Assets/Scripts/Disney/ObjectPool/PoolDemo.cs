using UnityEngine;

namespace Disney.ObjectPool
{
	public class PoolDemo : MonoBehaviour
	{
		public ObjectPool Pool;

		public float TimerValue = 1f;

		private float _timer;

		private void Start()
		{
			_timer = TimerValue;
		}

		private void Update()
		{
			if (!(Pool == null) && Pool.Ready)
			{
				_timer -= Time.deltaTime;
				if (_timer <= 0f)
				{
					IPoolable poolable = Pool.CheckOut();
					poolable.Spawn(base.transform.position, base.transform.rotation);
					_timer = TimerValue;
				}
			}
		}
	}
}
