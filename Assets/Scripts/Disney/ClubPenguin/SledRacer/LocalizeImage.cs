using DevonLocalization.Core;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Disney.ClubPenguin.SledRacer
{
	[RequireComponent(typeof(Image))]
	public class LocalizeImage : MonoBehaviour
	{
		[Serializable]
		public struct LanguageImage
		{
			public Language Language;

			public Sprite Image;
		}

		public LanguageImage[] LanguageImageMap;

		private void Start()
		{
			LanguageImage[] languageImageMap = LanguageImageMap;
			int num = 0;
			LanguageImage languageImage;
			while (true)
			{
				if (num < languageImageMap.Length)
				{
					languageImage = languageImageMap[num];
					if (languageImage.Language == Localizer.Instance.Language)
					{
						break;
					}
					num++;
					continue;
				}
				return;
			}
			base.gameObject.GetComponent<Image>().sprite = languageImage.Image;
		}
	}
}
