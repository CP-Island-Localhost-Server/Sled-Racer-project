using UnityEngine;
using UnityEngine.UI;

namespace Disney.ClubPenguin.Login.Creation
{
	public class ActivatePenguinController : MonoBehaviour
	{
		public delegate void AboutMembershipClickedDelegate();

		public AboutMembershipClickedDelegate AboutMembershipClicked;

		public RectTransform MemberBenefitsPanel;

		public Button PreviousBenefitButton;

		public Button NextBenefitButton;

		public RectTransform[] MemberBenefitViewPrefabs;

		public Text ParentEmailText;

		public Text PenguinName;

		public Button MembershipButton;

		private RectTransform[] MemberBenefitViewInstances;

		private int currentBenefitIndex;

		private RectTransform currentBenefitViewInstance;

		private bool showMembershipButton;

		private bool benefitsAdded;

		private void Start()
		{
			AddBenefitViewsToPanel();
			PreviousBenefitButton.onClick.AddListener(delegate
			{
				showPreviousBenefitInstance();
			});
			NextBenefitButton.onClick.AddListener(delegate
			{
				showNextBenefitInstance();
			});
			MembershipButton.gameObject.SetActive(showMembershipButton);
			MembershipButton.onClick.AddListener(delegate
			{
				if (AboutMembershipClicked != null)
				{
					AboutMembershipClicked();
				}
			});
		}

		public void SetParentEmail(string parentEmail)
		{
			ParentEmailText.text = parentEmail;
		}

		public void SetPenguinName(string penguinName)
		{
			PenguinName.text = penguinName;
		}

		public void SetMembershipButtonVisibility(bool isVisible)
		{
			MembershipButton.gameObject.SetActive(isVisible);
			showMembershipButton = isVisible;
		}

		private void showPreviousBenefitInstance()
		{
			if (MemberBenefitViewInstances != null)
			{
				currentBenefitViewInstance.gameObject.SetActive(value: false);
				currentBenefitIndex--;
				if (currentBenefitIndex < 0)
				{
					currentBenefitIndex = MemberBenefitViewInstances.Length - 1;
				}
				currentBenefitViewInstance = MemberBenefitViewInstances[currentBenefitIndex];
				currentBenefitViewInstance.gameObject.SetActive(value: true);
			}
		}

		private void showNextBenefitInstance()
		{
			if (MemberBenefitViewInstances != null)
			{
				currentBenefitViewInstance.gameObject.SetActive(value: false);
				currentBenefitIndex++;
				if (currentBenefitIndex == MemberBenefitViewInstances.Length)
				{
					currentBenefitIndex = 0;
				}
				currentBenefitViewInstance = MemberBenefitViewInstances[currentBenefitIndex];
				currentBenefitViewInstance.gameObject.SetActive(value: true);
			}
		}

		public void SetMembershipBenefitViewsPrefabs(RectTransform[] memberBenefitViewsPrefabs)
		{
			if (MemberBenefitViewInstances != null)
			{
				RectTransform[] memberBenefitViewInstances = MemberBenefitViewInstances;
				foreach (RectTransform rectTransform in memberBenefitViewInstances)
				{
					rectTransform.gameObject.SetActive(value: false);
					UnityEngine.Object.Destroy(rectTransform.gameObject);
				}
			}
			benefitsAdded = false;
			MemberBenefitViewPrefabs = memberBenefitViewsPrefabs;
			AddBenefitViewsToPanel();
		}

		private void AddBenefitViewsToPanel()
		{
			if (benefitsAdded)
			{
				return;
			}
			if (MemberBenefitViewPrefabs == null || MemberBenefitViewPrefabs.Length == 0)
			{
				UnityEngine.Debug.Log("NO BENEFIT PREFABS");
				return;
			}
			int num = 0;
			MemberBenefitViewInstances = new RectTransform[MemberBenefitViewPrefabs.Length];
			RectTransform[] memberBenefitViewPrefabs = MemberBenefitViewPrefabs;
			foreach (RectTransform original in memberBenefitViewPrefabs)
			{
				RectTransform rectTransform = UnityEngine.Object.Instantiate(original) as RectTransform;
				rectTransform.SetParent(MemberBenefitsPanel, worldPositionStays: false);
				rectTransform.gameObject.SetActive(value: false);
				MemberBenefitViewInstances[num] = rectTransform;
				num++;
			}
			MemberBenefitViewInstances[0].gameObject.SetActive(value: true);
			currentBenefitIndex = 0;
			currentBenefitViewInstance = MemberBenefitViewInstances[0];
			benefitsAdded = true;
		}
	}
}
