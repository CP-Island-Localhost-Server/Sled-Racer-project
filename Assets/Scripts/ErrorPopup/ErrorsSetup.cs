using ErrorPopup.Core;
using UnityEngine;

namespace ErrorPopup
{
	public class ErrorsSetup : MonoBehaviour
	{
		public string errorsJsonLocation = "Assets/Resources/ErrorPopup/errors.json.txt";

		private void Start()
		{
			ErrorsMap.Instance.LoadErrorsLocally(errorsJsonLocation);
		}
	}
}
