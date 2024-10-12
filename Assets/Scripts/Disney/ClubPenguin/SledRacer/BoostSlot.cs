using UnityEngine;
using UnityEngine.UI;

namespace Disney.ClubPenguin.SledRacer
{
	public class BoostSlot : MonoBehaviour
	{
		public Text TitleLable;

		public Image Image;

		public GameObject ImageContainer;

		public GameObject EmptyOption;

		public GameObject RemoveItemOption;

		public GameObject TitleBackGround;

		public BoostManager.AvailableBoosts? EquipedBoost
		{
			get;
			private set;
		}

		public void EquipeItem(BoostManager.AvailableBoosts boost, string title, Sprite image)
		{
			EquipedBoost = boost;
			TitleLable.text = title;
			Image.sprite = image;
			GetComponent<Button>().enabled = true;
			EmptyOption.SetActive(value: false);
			RemoveItemOption.SetActive(value: true);
			ImageContainer.gameObject.SetActive(value: true);
			TitleBackGround.SetActive(value: true);
			TitleLable.gameObject.SetActive(value: true);
		}

		public void ClearItem()
		{
			EquipedBoost = null;
			GetComponent<Button>().enabled = false;
			EmptyOption.SetActive(value: true);
			RemoveItemOption.SetActive(value: false);
			ImageContainer.gameObject.SetActive(value: false);
			TitleBackGround.SetActive(value: false);
			TitleLable.gameObject.SetActive(value: false);
		}

		public void RemoveItemClicked()
		{
			if (EquipedBoost.HasValue)
			{
				Service.Get<EventDataService>().SendUIEvent(this, new UIEvent(UIEvent.uiGameEvent.BoostUnequiped, EquipedBoost));
			}
		}
	}
}
