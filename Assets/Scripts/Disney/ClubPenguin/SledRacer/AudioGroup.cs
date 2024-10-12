using Disney.ClubPenguin.CPModuleUtils;
using Fabric;
using System;
using UnityEngine;

namespace Disney.ClubPenguin.SledRacer
{
	public abstract class AudioGroup : IAudioGroup
	{
		protected FabricManager fabric;

		protected EventManager events;

		private string mutePrefKey;

		public GroupComponent Group
		{
			get;
			private set;
		}

		public string Name
		{
			get;
			private set;
		}

		public float Volume
		{
			get
			{
				if (Group != null)
				{
					return Group.Volume;
				}
				return 0f;
			}
			set
			{
				if (Group != null)
				{
					Group.SetVolume(value);
				}
			}
		}

		private AudioGroup()
		{
			fabric = FabricManager.Instance;
			events = EventManager.Instance;
		}

		public AudioGroup(string component)
			: this()
		{
			if (fabric == null)
			{
				UnityEngine.Debug.LogError("Fabric Manager does not exists in the current scene.");
				return;
			}
			Group = (fabric.GetComponentByName(component) as GroupComponent);
			if (Group == null)
			{
				UnityEngine.Debug.LogError("Component named '" + component + " could not be found.");
				return;
			}
			InitMutePref();
			Name = Group.Name;
		}

		public AudioGroup(GroupComponent group)
			: this()
		{
			Name = group.Name;
			Group = group;
			if (Group == null)
			{
				throw new ArgumentNullException("group");
			}
			InitMutePref();
		}

		private void InitMutePref()
		{
			mutePrefKey = Group.gameObject.GetPath().Remove(0, 1).Replace("/", ".") + ".Mute";
			Group.Mute = ((PlayerPrefs.GetInt(mutePrefKey, 0) == 1) ? true : false);
		}

		public bool IsMuted()
		{
			return Group.Mute;
		}

		public void Mute()
		{
			Group.Mute = true;
			SavePref();
		}

		public void UnMute()
		{
			Group.Mute = false;
			SavePref();
		}

		private void SavePref()
		{
			PlayerPrefs.SetInt(mutePrefKey, Group.Mute ? 1 : 0);
			PlayerPrefs.Save();
		}
	}
}
