using DevonLocalization.Core;
using Disney.ClubPenguin.CPModuleUtils;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Disney.ClubPenguin.ParentPermission
{
	[RequireComponent(typeof(AudioSource))]
	public class ParentPermissionController : MonoBehaviour
	{
		public const string MODULE_ID = "PARENTPERMISSION";

		private const int YEAR_CHARACTER_LIMIT = 4;

		private const int MONTH_CHARACTER_LIMIT = 2;

		private const int DAY_CHARACTER_LIMIT = 2;

		private const float CHECKMARK_SUCCESS_DELAY_SEC = 0.8f;

		public string PathToTokens = "Assets/Framework/ParentPermission/Resources/Translations";

		public int MinAgeInYears = 13;

		public AudioClip OnOKButtonAudioClip;

		public AudioClip OnCloseButtonAudioClip;

		public ScrollRectSnapTo DayScroll;

		public ScrollRectSnapTo MonthScroll;

		public ScrollRectSnapTo YearScroll;

		public RectTransform DefaultBackground;

		public Button CloseButton;

		public Text TitleText;

		public Button OkButton;

		private int day;

		private int month;

		private int year;

		private Image modalImage;

		private int ageEntered;

		private bool isDaySelected;

		private bool isMonthSelected;

		private bool isYearSelected;

		public event Action<int> onSuccess;

		public event Action<int> onFailClose;

		private void Awake()
		{
			modalImage = GetComponent<Image>();
		}

		private void Start()
		{
			LoadTokenTranslations();
			HardwareBackButtonDispatcher.SetTargetClickHandler(CloseButton);
			CloseButton.onClick.AddListener(delegate
			{
				OnCloseButtonpressed();
			});
			OkButton.onClick.AddListener(delegate
			{
				OnOkButtonClicked();
			});
			DayScroll.ElementSelected += OnDayChanged;
			MonthScroll.ElementSelected += OnMonthChanged;
			YearScroll.ElementSelected += OnYearChanged;
			DayScroll.StartedScrolling += OnDayScrollStarted;
			MonthScroll.StartedScrolling += OnMonthScrollStarted;
			YearScroll.StartedScrolling += OnYearScrollStarted;
			StartCoroutine(SnapToCurrentYear());
		}

		private void OnDestroy()
		{
			Image[] componentsInChildren = GetComponentsInChildren<Image>();
			Image[] array = componentsInChildren;
			foreach (Image image in array)
			{
				image.sprite = null;
			}
			DayScroll.ElementSelected -= OnDayChanged;
			MonthScroll.ElementSelected -= OnMonthChanged;
			YearScroll.ElementSelected -= OnYearChanged;
			DayScroll.StartedScrolling -= OnDayScrollStarted;
			MonthScroll.StartedScrolling -= OnMonthScrollStarted;
			YearScroll.StartedScrolling -= OnYearScrollStarted;
			OnOKButtonAudioClip = null;
			OnCloseButtonAudioClip = null;
			DayScroll = null;
			MonthScroll = null;
			YearScroll = null;
			DefaultBackground = null;
			CloseButton = null;
			TitleText = null;
			OkButton = null;
		}

		private void OnDayScrollStarted()
		{
			isDaySelected = false;
			OkButton.interactable = false;
		}

		private void OnMonthScrollStarted()
		{
			isMonthSelected = false;
			OkButton.interactable = false;
		}

		private void OnYearScrollStarted()
		{
			isYearSelected = false;
			OkButton.interactable = false;
		}

		private IEnumerator SnapToCurrentYear()
		{
			yield return new WaitForSeconds(0.5f);
			YearScroll.TweenToContentChildWithIndex(0);
			MonthScroll.TweenToContentChildWithIndex(0);
			DayScroll.TweenToContentChildWithIndex(0);
		}

		public void LoadTokenTranslations()
		{
			ILocalizedTokenFilePath path = new ModuleTokensFilePath(PathToTokens, "PARENTPERMISSION", Platform.global);
			Localizer.Instance.LoadTokensFromLocalJSON(path);
		}

		private void OnDayChanged(GameObject dayObject)
		{
			day = int.Parse(dayObject.GetComponent<Text>().text);
			isDaySelected = true;
			CheckOkButton();
		}

		private void OnMonthChanged(GameObject monthObject)
		{
			month = int.Parse(monthObject.GetComponent<Text>().text);
			isMonthSelected = true;
			CheckOkButton();
		}

		private void OnYearChanged(GameObject yearObject)
		{
			year = int.Parse(yearObject.GetComponent<Text>().text);
			isYearSelected = true;
			CheckOkButton();
		}

		private void CheckOkButton()
		{
			OkButton.interactable = (isDaySelected && isMonthSelected && isYearSelected);
		}

		private void OnOkButtonClicked()
		{
			if (CheckDateOfBirth())
			{
				StartCoroutine(WaitForSound(this.onSuccess, OnOKButtonAudioClip));
			}
			else
			{
				StartCoroutine(WaitForSound(this.onFailClose, OnOKButtonAudioClip));
			}
		}

		private IEnumerator WaitForSound(Action<int> callback, AudioClip sound)
		{
			if (sound != null)
			{
				GetComponent<AudioSource>().PlayOneShot(sound);
			}
			yield return null;
			callback?.Invoke(ageEntered);
		}

		private void OnCloseButtonpressed()
		{
			StartCoroutine(WaitForSound(this.onFailClose, OnCloseButtonAudioClip));
		}

		public void EnableDefaultBackground()
		{
			if (!DefaultBackground.gameObject.activeSelf)
			{
				DefaultBackground.gameObject.SetActive(value: true);
				CloseButton.gameObject.SetActive(value: true);
				TitleText.gameObject.SetActive(value: true);
				modalImage.enabled = true;
			}
		}

		public void DisableDefaultBackground()
		{
			if (DefaultBackground.gameObject.activeSelf)
			{
				DefaultBackground.gameObject.SetActive(value: false);
				CloseButton.gameObject.SetActive(value: false);
				TitleText.gameObject.SetActive(value: false);
				modalImage.enabled = false;
			}
		}

		private bool CheckDateOfBirth()
		{
			bool result = false;
			if (year > 1700 && month > 0 && month < 13 && day > 0 && day < 32 && day <= DateTime.DaysInMonth(year, month))
			{
				DateTime birthDate = new DateTime(year, month, day);
				DateTime now = DateTime.Now;
				int num = now.Year - year;
				if (num > 13)
				{
					result = true;
				}
				else if (num == MinAgeInYears)
				{
					if (birthDate.Month < month)
					{
						result = true;
					}
					else if (birthDate.Month == month)
					{
						result = (birthDate.Day <= day);
					}
				}
				int num2;
				try
				{
					num2 = GetAge(birthDate, now);
				}
				catch (ArgumentOutOfRangeException)
				{
					num2 = -1;
				}
				ageEntered = num2;
				DateTime value = new DateTime(year, month, day).AddYears(MinAgeInYears);
				result = (now.CompareTo(value) >= 0);
			}
			return result;
		}

		private int GetAge(DateTime birthDate, DateTime todayDate)
		{
			DateTime d = new DateTime(birthDate.Year, birthDate.Month, birthDate.Day);
			DateTime d2 = new DateTime(todayDate.Year, todayDate.Month, todayDate.Day);
			TimeSpan value = d2 - d;
			return DateTime.MinValue.Add(value).Year - 1;
		}
	}
}
