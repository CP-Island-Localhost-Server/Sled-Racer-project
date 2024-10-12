using UnityEngine;

namespace ErrorPopup
{
	public class SetErrorArrow : MonoBehaviour
	{
		public GameObject leftArrow;

		public GameObject upArrow;

		public GameObject rightArrow;

		public GameObject downArrow;

		private void OnEnable()
		{
			leftArrow.SetActive(value: false);
			upArrow.SetActive(value: false);
			rightArrow.SetActive(value: false);
			downArrow.SetActive(value: false);
		}

		public void SetArrowByDirection(ErrorPosition errorPosition)
		{
			switch (errorPosition)
			{
			case ErrorPosition.DOWN:
				upArrow.SetActive(value: true);
				break;
			case ErrorPosition.LEFT:
				rightArrow.SetActive(value: true);
				break;
			case ErrorPosition.RIGHT:
				leftArrow.SetActive(value: true);
				break;
			case ErrorPosition.UP:
				downArrow.SetActive(value: true);
				break;
			}
		}
	}
}
