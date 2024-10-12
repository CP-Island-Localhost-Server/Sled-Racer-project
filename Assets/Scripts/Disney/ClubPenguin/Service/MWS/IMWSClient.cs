using System;
using System.Collections.Generic;
using Disney.ClubPenguin.Service.MWS.Domain;
using Disney.ClubPenguin.Service.MWS.Domain.Likes;
using Disney.HTTP.Client;

namespace Disney.ClubPenguin.Service.MWS
{
	public interface IMWSClient
	{
		string AuthToken { get; }

		IGetAuthTokenResponse GetAuthToken(string appId, string appVersion, string username, string password, Action<IGetAuthTokenResponse> responseHandler = null);

		void ClearAuthToken(Action responseHandler = null);

		IGetAccountResponse GetAccount(Action<IGetAccountResponse> responseHandler = null);

		ICreateAccountResponse CreateAccount(string username, string password, string appVersion, string appId, string email, int colour, int language, Action<ICreateAccountResponse> responseHandler = null);

		IGetIdentityResponse GetIdentity(string id, int language, Action<IGetIdentityResponse> responseHandler = null);

		IHTTPResponse RequestActivationEmailResent(Action<IHTTPResponse> responseHandler = null);

		IGetPlayerCardDataResponse GetPlayerCardData(Action<IGetPlayerCardDataResponse> responseHandler = null, bool forceReload = false);

		IGetOutfitResponse GetOutfit(Action<IGetOutfitResponse> responseHandler = null);

		ISetOutfitResponse SetOutfit(Outfit outfit, Action<ISetOutfitResponse> responseHandler = null);

		IPurchaseItemResponse PurchaseItem(string itemType, long itemId, Action<IPurchaseItemResponse> responseHandler = null);

		ISetCoinsResponse SetCoins(int coins, string appId, Action<ISetCoinsResponse> responseHandler = null);

		ISetCoinsResponse IncrementCoins(int amount, string appId, Action<ISetCoinsResponse> responseHandler = null);

		IStartGameResponse StartGame(string gameType, Action<IStartGameResponse> responseHandler = null);

		ISetGameResultResponse SetGameResult(GameResult gameResult, Action<ISetGameResultResponse> responseHandler = null);

		IGetIglooInstanceResponse GetIglooInstance(string ownerId, string language, Action<IGetIglooInstanceResponse> responseHandler = null);

		IGetIgloosResponse GetIgloos(IglooListType type, string language, int limit, Action<IGetIgloosResponse> responseHandler = null);

		IGetPuffleResponse GetPuffle(long puffleId, Action<IGetPuffleResponse> responseHandler = null);

		IGetPufflesResponse GetPuffles(Action<IGetPufflesResponse> responseHandler = null);

		ICreatePuffleResponse CreatePuffle(int type, string name, int languageCode, Action<ICreatePuffleResponse> responseHandler = null);

		IHTTPResponse UpdatePuffle(int puffleId, int play, int rest, int clean, long headItemId, long location, Action<IHTTPResponse> responseHandler = null);

		IGetSoundStudioTrackDataResponse AddSoundStudioTrack(SoundStudioTrackData soundStudioTrackData, Action<IGetSoundStudioTrackDataResponse> responseHandler = null);

		IHTTPResponse DeleteSoundStudioTrack(long trackId, Action<IHTTPResponse> responseHandler = null);

		IGetSoundStudioTrackDataResponse GetSoundStudioTrack(long playerId, long trackId, Action<IGetSoundStudioTrackDataResponse> responseHandler = null);

		IGetSoundStudioTracksDataResponse GetSoundStudioTracks(Action<IGetSoundStudioTracksDataResponse> responseHandler = null);

		IGetSoundStudioTracksDataResponse GetSoundStudioTracksListing(Action<IGetSoundStudioTracksDataResponse> responseHandler = null);

		IGetSoundStudioRadioTracksDataResponse GetSharedSoundStudioTracksListing(List<string> swids, string language, Action<IGetSoundStudioRadioTracksDataResponse> responseHandler = null);

		IHTTPResponse RenameSoundStudioTrack(long trackId, string name, Action<IHTTPResponse> responseHandler = null);

		IHTTPResponse UpdateShareStateOfSoundStudioTrack(long trackId, TrackShareState shareState, Action<IHTTPResponse> responseHandler = null);

		IGetSoundStudioRadioTracksDataResponse GetNewSharedSoundStudioTracksListing(string language, int limit, long beforeTrackId, Action<IGetSoundStudioRadioTracksDataResponse> responseHandler = null);

		IGetSoundStudioRadioTracksDataResponse GetRandomSharedSoundStudioTracksListing(string language, int limit, Action<IGetSoundStudioRadioTracksDataResponse> responseHandler = null);

		ICanLikeResponse CanLike(ID actorID, ID objectID, Action<ICanLikeResponse> responseHandler = null);

		ILikeResponse Like(Activity activity, Action<ILikeResponse> responseHandler = null);

		IUnLikeResponse UnLike(Activity activity, Action<IUnLikeResponse> responseHandler = null);

		IGetLikedByCountsResponse GetLikedByCounts(ID objectID, Action<IGetLikedByCountsResponse> responseHandler = null);

		IGetLikeCountsResponse GetLikeCounts(ID objectID, Action<IGetLikeCountsResponse> responseHandler = null);

		IGetLikedBysResponse GetLikedBys(ID objectID, ID actorID, PaginationInfo pInfo, Action<IGetLikedBysResponse> responseHandler = null);

		IGetLikesResponse GetLikes(ID actorID, PaginationInfo pInfo, Action<IGetLikesResponse> responseHandler = null);

		IDeleteEntityResponse DeleteEntity(ID objectID, Action<IDeleteEntityResponse> responseHandler = null);

		IDeleteLikesResponse DeleteLikes(ID objectID, Action<IDeleteLikesResponse> responseHandler = null);

		IDeleteLikedBysResponse DeleteLikedBys(ID objectID, bool resetAll, Action<IDeleteLikedBysResponse> responseHandler = null);

		ISetEntityClassResponse SetEntityClass(EntityClass entityClass, Action<ISetEntityClassResponse> responseHandler = null);

		IGetEntityClassResponse GetEntityClass(string context, string type, Action<IGetEntityClassResponse> responseHandler = null);

		IMyHighScoreResponse GetMyHighScore(string gameType, Action<IMyHighScoreResponse> responseHandler = null);

		IGetLeaderBoardResponse GetLeaderBoard(string gameType, string language, string duration, bool friendsOnly, Action<IGetLeaderBoardResponse> responseHandler = null);

		IGetLeaderBoardRewardStatus GetLeaderBoardRewardStatus(string gameType, string duration, int periods, Action<IGetLeaderBoardRewardStatus> responseHandler = null);

		IGetIAPProductsResponse GetIAPProductsForApp(string appId, string appVersion, StoreType store, Action<IGetIAPProductsResponse> responseHandler = null);

		IGetIAPPurchasesResponse GetIAPPurchasesForPlayer(string[] productIds = null, Action<IGetIAPPurchasesResponse> responseHandler = null);

		IClaimProductsForMemberResponse ClaimProductsForMember(string deviceId, StoreType store, IList<string> productIds, Action<IClaimProductsForMemberResponse> responseHandler = null);

		IRegisterApplePurchaseResponse RegisterApplePurchase(string appId, string deviceId, ApplePurchaseData purchaseData, Action<IRegisterApplePurchaseResponse> responseHandler = null);

		IRegisterGooglePlayPurchaseResponse RegisterGooglePlayPurchase(string appId, string deviceId, GooglePlayPurchaseData purchaseData, Action<IRegisterGooglePlayPurchaseResponse> responseHandler = null);
	}
}
