using UnityEngine;

namespace Disney.ClubPenguin.Common.Utils
{
	public class AttachToLayer : MonoBehaviour
	{
		public float MaxDistance = 10f;

		public LayerMask Mask;

		public Vector3 RaycastDirection = Vector3.down;

		private void Update()
		{
			PhysicsUtil.AttachObjectToLayer(base.gameObject, Mask, MaxDistance, RaycastDirection);
			base.enabled = false;
		}
	}
}
