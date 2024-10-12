using UnityEngine;

namespace Disney.ClubPenguin.SledRacer
{
	public class CameraTarget : MonoBehaviour
	{
		private PlayerController player;

		private ConfigController config;

		private Animator animTarget;

		private Vector3 localOrigin;

		private Vector3 worldOrigin;

		private float crashTime;

		private void Awake()
		{
			player = GetComponentInParent<PlayerController>();
			config = Service.Get<ConfigController>();
		}

		private void Start()
		{
			localOrigin = base.transform.localPosition;
		}

		private void Update()
		{
			worldOrigin = player.transform.position + localOrigin;
			if ((bool)player.RiderAnimator && !animTarget)
			{
				animTarget = player.RiderAnimator;
			}
			else if (!player.RiderAnimator)
			{
				animTarget = null;
			}
			if (player.currentLifeState != PlayerController.PlayerLifeState.Done && (bool)animTarget)
			{
				float num = animTarget.GetFloat("AnimY") * config.BoostWhoopieCushionAnimBounceHeight;
				if (player.currentLifeState == PlayerController.PlayerLifeState.Crashed || player.currentLifeState == PlayerController.PlayerLifeState.Done)
				{
					crashTime += Time.deltaTime;
					Vector3 a = worldOrigin;
					Vector3 up = Vector3.up;
					float num2 = num;
					Vector3 lossyScale = base.transform.lossyScale;
					Vector3 position = a + up * (num2 * lossyScale.y);
					base.transform.position = position;
				}
				else
				{
					crashTime = 0f;
					base.transform.localPosition = localOrigin;
				}
			}
		}
	}
}
