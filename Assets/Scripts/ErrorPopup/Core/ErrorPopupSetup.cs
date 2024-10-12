using System.Collections.Generic;
using UnityEngine;

namespace ErrorPopup.Core
{
	public class ErrorPopupSetup : MonoBehaviour
	{
		public List<string> errorJsonLocations = new List<string>
		{
			"Assets/Framework/ErrorPopup/Resources/errors.json"
		};

		private void Start()
		{
			errorJsonLocations.ForEach(delegate(string path)
			{
				ErrorsMap.Instance.LoadErrorsLocally(path);
			});
		}
	}
}
