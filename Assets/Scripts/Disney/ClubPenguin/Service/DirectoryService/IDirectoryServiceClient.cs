using System;
using Disney.HTTP.Client;

namespace Disney.ClubPenguin.Service.DirectoryService
{
	public interface IDirectoryServiceClient
	{
		CPEnvironment Environment { get; set; }

		bool IsDev { get; }

		bool IsInt { get; }

		bool IsQA { get; }

		bool IsStage { get; }

		bool IsProd { get; }

		string AppID { get; set; }

		string AppVersion { get; set; }

		int VersionCheckInterval { get; set; }

		string MWSCellophanePrefix { get; }

		event Action OnUpdateRecommended;

		event Action OnUpdateRequired;

		void CheckVersion();

		string GetServiceURL(string serviceName, Action<string> successHandler = null, Action<IHTTPResponse> failureHandler = null);

		IGetCompatibilityResponse GetCompatibility(string appId, string appVersion, Action<IGetCompatibilityResponse> responseHandler = null);
	}
}
