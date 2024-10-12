using UnityEngine;

namespace Disney.ObjectPool
{
	public class PoolableObjectLifespan : PoolableObject
	{
		public float Speed = 5f;

		public float Lifespan = 2f;

		protected float _lifespan;

		private void Start()
		{
		}

		private void Update()
		{
			if (base.gameObject.activeSelf)
			{
				float deltaTime = Time.deltaTime;
				_lifespan -= deltaTime;
				if (_lifespan <= 0f)
				{
					Despawn();
				}
				base.transform.Translate(Vector3.right * Speed * deltaTime);
			}
		}
	}
}
