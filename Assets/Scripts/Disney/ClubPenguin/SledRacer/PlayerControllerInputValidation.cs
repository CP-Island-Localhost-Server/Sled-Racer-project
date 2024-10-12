using Fabric;
using UnityEngine;

namespace Disney.ClubPenguin.SledRacer
{
	public class PlayerControllerInputValidation : MonoBehaviour
	{
		private class MockAudio : IAudio, IAudioGroup
		{
			private MockSFX mockSFX;

			public IMusic Music => null;

			public ISFX SFX => mockSFX;

			public IAmbience Ambience => null;

			public LowPassFilter LowPassFilter => null;

			public GroupComponent Group => null;

			public string Name => "MockAudio";

			public float Volume
			{
				get
				{
					return 0f;
				}
				set
				{
				}
			}

			public MockAudio()
			{
				mockSFX = new MockSFX();
			}

			public bool IsMuted()
			{
				return false;
			}

			public void Mute()
			{
			}

			public void UnMute()
			{
			}
		}

		private class MockSFX : IAudioGroup, ISFX
		{
			public GroupComponent Group => null;

			public string Name => "MockSFX";

			public float Volume
			{
				get
				{
					return 0f;
				}
				set
				{
				}
			}

			public void Stop(SFXEvent sfx)
			{
			}

			public void Play(SFXEvent sfx)
			{
			}

			public void Play(SFXEvent sfx, GameObject go)
			{
			}

			public void Pause(SFXEvent sfx)
			{
			}

			public void Unpause(SFXEvent sfx)
			{
			}

			public void AdvanceSequence(SFXEvent sfx)
			{
			}

			public void ResetSequence(SFXEvent sfx)
			{
			}

			public void ButtonClickIn()
			{
			}

			public void ButtonClickOut()
			{
			}

			public void ToggleOff()
			{
			}

			public void ToggleOn()
			{
			}

			public void MuteRaceSFX(bool mute)
			{
			}

			public void StopRaceSFX()
			{
			}

			public bool IsMuted()
			{
				return false;
			}

			public void Mute()
			{
			}

			public void UnMute()
			{
			}
		}

		public PlayerController playerController;

		public MockInputManager inputManager;

		public MockAxis axisController;

		private void Awake()
		{
			Service.Set((IAudio)new MockAudio());
			base.gameObject.AddComponent<ConfigController>();
		}

		private void Start()
		{
			playerController.gameLogger = new NullGameEventLogger();
			playerController.enableUserInput();
			Vector3 input = playerController.getInput();
			IntegrationTest.Assert(Vector3.zero.Equals(input));
			Vector3 zero = Vector3.zero;
			zero.y = 1f;
			inputManager.mockJump = true;
			input = playerController.getInput();
			IntegrationTest.Assert(zero.Equals(input));
			inputManager.mockJump = false;
			Vector3 zero2 = Vector3.zero;
			zero2.x = 0.9f;
			inputManager.mockRight = true;
			input = playerController.getInput();
			UnityEngine.Debug.Log("input is " + input);
			IntegrationTest.Assert(zero2.Equals(input));
			inputManager.mockRight = false;
			Vector3 zero3 = Vector3.zero;
			zero3.x = -0.9f;
			inputManager.mockLeft = true;
			input = playerController.getInput();
			UnityEngine.Debug.Log("input is " + input);
			IntegrationTest.Assert(zero3.Equals(input));
			inputManager.mockLeft = false;
		}
	}
}
