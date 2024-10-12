using DevonLocalization.Core;
using System;
using System.Collections.Generic;

namespace Disney.ClubPenguin.SledRacer
{
	public class BoostManager
	{
		public enum AvailableBoosts
		{
			[Boost("Boost.Label.Invulnerability", "Boost.Desc.Invulnerability", "BoostItem4", typeof(BoostInvulnerable))]
			INVULNERABLE,
			[Boost("Boost.Label.Jetpack", "Boost.Desc.Jetpack", "BoostItem2", typeof(BoostJetpack))]
			JETPACK,
			[Boost("Boost.Label.Parachute", "Boost.Desc.Parachute", "BoostItem5", typeof(BoostParachute))]
			PARACHUTE,
			[Boost("Boost.Label.Revive", "Boost.Desc.Revive", "BoostItem6", typeof(BoostRevive))]
			REVIVE,
			[Boost("Boost.Label.SkiWax", "Boost.Desc.SkiWax", "BoostItem3", typeof(BoostSkiwax))]
			SKIWAX,
			[Boost("Boost.Label.WhoopieCushion", "Boost.Desc.WhoopieCushion", "BoostItem1", typeof(BoostWhoopeeCushion))]
			WHOOPIECUSHION
		}

		internal class BoostAttribute : Attribute
		{
			public string Title
			{
				get;
				private set;
			}

			public string Description
			{
				get;
				private set;
			}

			public string ImageName
			{
				get;
				private set;
			}

			public Type ImplementationClass
			{
				get;
				private set;
			}

			internal BoostAttribute(string title, string description, string imageName, Type implementationClass)
			{
				Title = Localizer.Instance.GetTokenTranslation(title);
				Description = Localizer.Instance.GetTokenTranslation(description);
				ImageName = imageName;
				ImplementationClass = implementationClass;
			}
		}

		private Dictionary<AvailableBoosts, IBoost> EquipedBoostsMap = new Dictionary<AvailableBoosts, IBoost>();

		public IList<AvailableBoosts> EquipedBoosts
		{
			get;
			private set;
		}

		public BoostManager()
		{
			EquipedBoosts = new List<AvailableBoosts>();
		}

		public void EquipeBoost(AvailableBoosts boost)
		{
			EquipedBoosts.Add(boost);
		}

		public void UnEquipeBoost(AvailableBoosts boost)
		{
			EquipedBoosts.Remove(boost);
		}

		public void ClearEquipeBoosts()
		{
			EquipedBoosts.Clear();
		}

		public void AddBoostObject(AvailableBoosts type, IBoost boost)
		{
			EquipedBoostsMap.Add(type, boost);
		}

		public void ClearBoostObjects()
		{
			EquipedBoostsMap.Clear();
		}

		public IBoost GetBoostObject(AvailableBoosts boostType)
		{
			object result;
			if (EquipedBoostsMap.ContainsKey(boostType))
			{
				IBoost boost = EquipedBoostsMap[boostType];
				result = boost;
			}
			else
			{
				result = null;
			}
			return (IBoost)result;
		}
	}
}
