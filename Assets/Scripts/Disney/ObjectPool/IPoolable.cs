using UnityEngine;

namespace Disney.ObjectPool
{
	public interface IPoolable
	{
		IPool SourcePool
		{
			get;
			set;
		}

		GameObject GameObject
		{
			get;
		}

		void Spawn(Transform newParent, Vector3 position, Quaternion rotation);

		void Spawn(Vector3 position, Quaternion rotation);

		void Spawn(Vector3 position, Quaternion rotation, Vector3 trans);

		void Despawn();

		void ReparentTo(Transform newParent);
	}
}
