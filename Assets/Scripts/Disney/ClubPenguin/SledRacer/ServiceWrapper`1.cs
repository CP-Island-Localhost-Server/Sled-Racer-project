namespace Disney.ClubPenguin.SledRacer
{
	internal class ServiceWrapper<T> : IServiceWrapper
	{
		public static T instance = default(T);

		public void Unset()
		{
			instance = default(T);
		}
	}
}
