using System;
using System.Collections.Generic;

namespace Disney.ClubPenguin.SledRacer
{
	public static class Service
	{
		[ThreadStatic]
		private static List<IServiceWrapper> serviceWrapperList;

		public static void Set<T>(T instance)
		{
			if (ServiceWrapper<T>.instance != null)
			{
				throw new Exception("An instance of this service class has already been set!");
			}
			ServiceWrapper<T>.instance = instance;
			if (serviceWrapperList == null)
			{
				serviceWrapperList = new List<IServiceWrapper>();
			}
			serviceWrapperList.Add(new ServiceWrapper<T>());
		}

		public static T Get<T>()
		{
			return ServiceWrapper<T>.instance;
		}

		public static bool IsSet<T>()
		{
			return ServiceWrapper<T>.instance != null;
		}

		public static void ResetAll()
		{
			if (serviceWrapperList != null)
			{
				for (int num = serviceWrapperList.Count - 1; num >= 0; num--)
				{
					serviceWrapperList[num].Unset();
				}
				serviceWrapperList = null;
			}
		}
	}
}
