using UnityEngine;
using UnityEngine.UI;

namespace Disney.ClubPenguin.SledRacer
{
	public class BoostSelectItem : MonoBehaviour
	{
		private enum VisualState
		{
			BUY,
			ADD,
			NONE
		}

		public Text TitleLable;

		public Image Image;

		public GameObject AddItemOption;

		public GameObject BuyItemOption;

		public GameObject SelectedItemOption;

		public GameObject SelectedItemCheckBox;

		private VisualState currentState;

		public BoostManager.AvailableBoosts Boost
		{
			get;
			private set;
		}

		public void SetUp(BoostManager.AvailableBoosts boost, string title, Sprite image)
		{
			TitleLable.text = title;
			Image.sprite = image;
			Boost = boost;
		}

		public void Equiped()
		{
			currentState = VisualState.NONE;
			base.gameObject.GetComponent<Button>().targetGraphic = null;
			AddItemOption.SetActive(value: false);
			BuyItemOption.SetActive(value: false);
			SelectedItemOption.SetActive(value: true);
			SelectedItemCheckBox.SetActive(value: true);
			GetComponent<BILogButtonNavigation>().enabled = false;
		}

		public void Unavailable()
		{
			currentState = VisualState.BUY;
			base.gameObject.GetComponent<Button>().targetGraphic = BuyItemOption.GetComponent<Image>();
			AddItemOption.SetActive(value: false);
			BuyItemOption.SetActive(value: true);
			SelectedItemOption.SetActive(value: false);
			SelectedItemCheckBox.SetActive(value: false);
			GetComponent<BILogButtonNavigation>().enabled = true;
		}

		public void Owned()
		{
			currentState = VisualState.ADD;
			base.gameObject.GetComponent<Button>().targetGraphic = AddItemOption.GetComponent<Image>();
			AddItemOption.SetActive(value: true);
			BuyItemOption.SetActive(value: false);
			SelectedItemOption.SetActive(value: false);
			SelectedItemCheckBox.SetActive(value: false);
			GetComponent<BILogButtonNavigation>().enabled = false;
		}

		public void OnClicked()
		{
			switch (currentState)
			{
			case VisualState.ADD:
				Service.Get<IAudio>().SFX.Play(SFXEvent.UI_PowerUpEquip);
				Service.Get<EventDataService>().SendUIEvent(this, new UIEvent(UIEvent.uiGameEvent.BoostEquiped, Boost));
				break;
			case VisualState.BUY:
				Service.Get<EventDataService>().SendUIEvent(this, new UIEvent(UIEvent.uiGameEvent.IAPRequest));
				break;
			case VisualState.NONE:
				Service.Get<IAudio>().SFX.Play(SFXEvent.UI_PowerUpRemove);
				Service.Get<EventDataService>().SendUIEvent(this, new UIEvent(UIEvent.uiGameEvent.BoostUnequiped, Boost));
				break;
			}
		}
	}
}
