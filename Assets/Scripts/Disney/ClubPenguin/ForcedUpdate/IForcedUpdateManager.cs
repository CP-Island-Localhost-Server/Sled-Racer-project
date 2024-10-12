using Disney.ClubPenguin.Service.DirectoryService;
using UnityEngine;

namespace Disney.ClubPenguin.ForcedUpdate
{
	public interface IForcedUpdateManager
	{
		bool AlertOnUpdateRecommended
		{
			get;
			set;
		}

		IDirectoryServiceClient DirectoryServiceClient
		{
			get;
			set;
		}

		GameObject ParentView
		{
			get;
			set;
		}

		long? PlayerId
		{
			get;
			set;
		}

		void Init(GameObject parentView, string clientId, string clientVersion, string appStoreUrl);

		void Init(GameObject parentView, string clientId, string clientVersion, string appStoreUrl, int versionCheckInterval);

		void CheckVersion();
	}
}
