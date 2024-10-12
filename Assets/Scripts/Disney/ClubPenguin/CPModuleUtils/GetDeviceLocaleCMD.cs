using System;
using UnityEngine;

namespace Disney.ClubPenguin.CPModuleUtils
{
	public class GetDeviceLocaleCMD
	{
		public event Action<string> LocaleAsString;

		public void Execute()
		{
			this.LocaleAsString(Application.systemLanguage.ToString());
		}
	}
}
