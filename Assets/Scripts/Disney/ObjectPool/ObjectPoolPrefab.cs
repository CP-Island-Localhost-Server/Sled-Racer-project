using System;

namespace Disney.ObjectPool
{
	public class ObjectPoolPrefab : ObjectPool
	{
		public override bool Ready
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public override void CheckIn(IPoolable poolable)
		{
			throw new NotImplementedException();
		}

		public override IPoolable CheckOut()
		{
			throw new NotImplementedException();
		}

		public override void Shrink()
		{
			throw new NotImplementedException();
		}
	}
}
