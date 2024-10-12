namespace Disney.ObjectPool
{
	public interface IPool
	{
		bool Ready
		{
			get;
		}

		void CheckIn(IPoolable poolable);

		IPoolable CheckOut();

		void Shrink();
	}
}
