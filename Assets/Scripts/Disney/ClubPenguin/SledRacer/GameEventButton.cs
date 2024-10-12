using UnityEngine;
using UnityEngine.UI;

namespace Disney.ClubPenguin.SledRacer
{
	public class GameEventButton : MonoBehaviour
	{
		public Button Button;

		public GameEvent.Type PressedEvent;

		public GameEvent.Type ReleasedEvent;

		private Sprite pressedSprite;

		private Sprite releasedSprite;

		private void Start()
		{
			PlayerController.OnGameEvent += GameEventHandler;
			pressedSprite = Button.spriteState.pressedSprite;
			releasedSprite = Button.image.sprite;
		}

		private void OnDestroy()
		{
			PlayerController.OnGameEvent -= GameEventHandler;
		}

		private void GameEventHandler(object sender, GameEvent e)
		{
			if (e.type == PressedEvent)
			{
				Button.image.sprite = pressedSprite;
			}
			else if (e.type == ReleasedEvent)
			{
				Button.image.sprite = releasedSprite;
			}
		}
	}
}
