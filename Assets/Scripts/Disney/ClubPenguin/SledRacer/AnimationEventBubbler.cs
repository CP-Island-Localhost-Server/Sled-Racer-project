using System.Reflection;
using UnityEngine;

namespace Disney.ClubPenguin.SledRacer
{
	public class AnimationEventBubbler : MonoBehaviour
	{
		private GameObject parent;

		public bool PropegateToAllAncenstors;

		private void Start()
		{
			Transform transform = GetComponent<Transform>().parent;
			if (transform != null)
			{
				parent = transform.gameObject;
			}
		}

		private void OnTumbleEnd(AnimationEvent other)
		{
			DevTrace(base.gameObject.name + " heard OnTumbleEnd");
			PropegateAnimationEvent("OnTumbleEnd", other);
		}

		private void OnWhoopeeEnd(AnimationEvent other)
		{
			DevTrace(base.gameObject.name + " heard OnWhoopeeEnd");
			PropegateAnimationEvent("OnWhoopeeEnd", other);
		}

		private void OnWhoopeeLaunch(AnimationEvent other)
		{
			DevTrace(base.gameObject.name + " heard OnWhoopeeLaunch");
			PropegateAnimationEvent("OnWhoopeeLaunch", other);
		}

		private void OnLandNormalStart(AnimationEvent other)
		{
			PropegateAnimationEvent("OnLandNormalStart", other);
		}

		private void OnHardLandStart(AnimationEvent other)
		{
			PropegateAnimationEvent("OnHardLandStart", other);
		}

		private void PropegateAnimationEvent(string message, AnimationEvent other)
		{
			if (parent != null)
			{
				if (PropegateToAllAncenstors)
				{
					SendMessageUpwards(message, other, SendMessageOptions.DontRequireReceiver);
				}
				else
				{
					parent.SendMessage(message, other);
				}
			}
		}

		private void DevTrace(string _msg)
		{
			UnityEngine.Debug.Log("[" + MethodBase.GetCurrentMethod().ReflectedType.Name + "] " + _msg);
		}
	}
}
