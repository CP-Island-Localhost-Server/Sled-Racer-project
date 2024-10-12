using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using Disney.ClubPenguin.Service.DirectoryService;
using Disney.ClubPenguin.Service.MWS.Domain;
using Disney.ClubPenguin.Service.MWS.Domain.Likes;
using Disney.HTTP.Client;
using Disney.HTTP.Client.HTTPUnity;
using HTTP;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using strange.extensions.injector.api;
using UnityEngine;

namespace Disney.ClubPenguin.Service.MWS
{
	[Implements(typeof(IMWSClient), InjectionBindingScope.CROSS_CONTEXT)]
	public class MWSClient : IMWSClient
	{
		public class GetAuthTokenResponse : HTTPResponseDelegate, IHTTPResponse, IGetAuthTokenResponse
		{
			private AuthData authData;

			private ResponseError responseError;

			public AuthData AuthData
			{
				get
				{
					return authData;
				}
			}

			public ResponseError ResponseError
			{
				get
				{
					return responseError;
				}
			}

			public GetAuthTokenResponse(IHTTPResponse httpResponse)
				: base(httpResponse)
			{
				try
				{
					if (httpResponse.IsError)
					{
						if (httpResponse.Text != null)
						{
							try
							{
								JObject.Parse(httpResponse.Text);
								responseError = JsonConvert.DeserializeObject<ResponseError>(httpResponse.Text, new JsonSerializerSettings
								{
									NullValueHandling = NullValueHandling.Ignore
								});
								return;
							}
							catch (Exception)
							{
								Debug.Log("Could not parse Json Response: " + httpResponse.Text);
								return;
							}
						}
					}
					else
					{
						authData = JsonConvert.DeserializeObject<AuthData>(httpResponse.Text);
					}
				}
				catch (Exception exception)
				{
					Debug.LogException(exception);
				}
			}
		}

		public class GetAccountResponse : HTTPResponseDelegate, IHTTPResponse, IGetAccountResponse
		{
			private Account account;

			public Account Account
			{
				get
				{
					return account;
				}
			}

			public GetAccountResponse(IHTTPResponse httpResponse)
				: base(httpResponse)
			{
				if (!httpResponse.IsError)
				{
					Debug.Log("Deserializing " + httpResponse.Text);
					account = JsonConvert.DeserializeObject<Account>(httpResponse.Text, new JsonSerializerSettings
					{
						NullValueHandling = NullValueHandling.Ignore
					});
					Debug.Log("Deserialized to " + account);
				}
			}
		}

		public class CreateAccountResponse : HTTPResponseDelegate, IHTTPResponse, ICreateAccountResponse
		{
			private Account account;

			private ResponseError responseError;

			public Account Account
			{
				get
				{
					return account;
				}
			}

			public ResponseError ResponseError
			{
				get
				{
					return responseError;
				}
			}

			public CreateAccountResponse(IHTTPResponse httpResponse)
				: base(httpResponse)
			{
				Debug.Log("Deserializing " + httpResponse.Text);
				if (httpResponse.IsError)
				{
					if (httpResponse.Text != null)
					{
						responseError = JsonConvert.DeserializeObject<ResponseError>(httpResponse.Text, new JsonSerializerSettings
						{
							NullValueHandling = NullValueHandling.Ignore
						});
						Debug.Log("Deserialized to " + responseError);
					}
				}
				else
				{
					account = JsonConvert.DeserializeObject<Account>(httpResponse.Text, new JsonSerializerSettings
					{
						NullValueHandling = NullValueHandling.Ignore
					});
					Debug.Log("Deserialized to " + account);
				}
			}
		}

		public class GetIdentityResponse : HTTPResponseDelegate, IHTTPResponse, IGetIdentityResponse
		{
			private Identity identity;

			public Identity Identity
			{
				get
				{
					return identity;
				}
			}

			public GetIdentityResponse(IHTTPResponse httpResponse)
				: base(httpResponse)
			{
				Debug.Log("Deserializing " + httpResponse.Text);
				identity = JsonConvert.DeserializeObject<Identity>(httpResponse.Text);
				Debug.Log("Deserialized to " + identity);
			}
		}

		public class GetPlayerCardDataResponse : HTTPResponseDelegate, IHTTPResponse, IGetPlayerCardDataResponse
		{
			private PlayerCardData playerCardData;

			public PlayerCardData PlayerCardData
			{
				get
				{
					return playerCardData;
				}
			}

			public GetPlayerCardDataResponse(IHTTPResponse httpResponse)
				: base(httpResponse)
			{
				Debug.Log("Deserializing " + httpResponse.Text);
				playerCardData = JsonConvert.DeserializeObject<PlayerCardData>(httpResponse.Text);
				Debug.Log("Deserialized to " + playerCardData);
			}
		}

		public class GetOutfitResponse : HTTPResponseDelegate, IHTTPResponse, IGetOutfitResponse
		{
			private Outfit outfit;

			public Outfit Outfit
			{
				get
				{
					return outfit;
				}
			}

			public GetOutfitResponse(IHTTPResponse httpResponse)
				: base(httpResponse)
			{
				Debug.Log("Deserializing " + httpResponse.Text);
				outfit = Outfit.FromDelimitedString(httpResponse.Text);
				Debug.Log("Deserialized to " + outfit);
			}
		}

		public class SetOutfitResponse : HTTPResponseDelegate, IHTTPResponse, ISetOutfitResponse
		{
			private Outfit outfit;

			public Outfit Outfit
			{
				get
				{
					return outfit;
				}
			}

			public SetOutfitResponse(IHTTPResponse httpResponse)
				: base(httpResponse)
			{
				Debug.Log("Deserializing " + httpResponse.Text);
				outfit = Outfit.FromDelimitedString(httpResponse.Text);
				Debug.Log("Deserialized to " + outfit);
			}
		}

		public class PurchaseItemResponse : HTTPResponseDelegate, IHTTPResponse, IPurchaseItemResponse
		{
			private Purchase purchase;

			public Purchase Purchase
			{
				get
				{
					return purchase;
				}
			}

			public PurchaseItemResponse(IHTTPResponse httpResponse)
				: base(httpResponse)
			{
				Debug.Log("Deserializing " + httpResponse.Text);
				purchase = JsonConvert.DeserializeObject<Purchase>(httpResponse.Text);
				Debug.Log("Deserialized to " + purchase);
			}
		}

		public class SetCoinsResponse : HTTPResponseDelegate, IHTTPResponse, ISetCoinsResponse
		{
			private int coins;

			public int Coins
			{
				get
				{
					return coins;
				}
			}

			public SetCoinsResponse(IHTTPResponse httpResponse)
				: base(httpResponse)
			{
				Debug.Log("Deserializing " + httpResponse.Text);
				var anonymousTypeObject = new
				{
					coins = 0
				};
				var anon = JsonConvert.DeserializeAnonymousType(httpResponse.Text, anonymousTypeObject);
				coins = anon.coins;
				Debug.Log("Deserialized to " + coins);
			}
		}

		public class StartGameResponse : HTTPResponseDelegate, IHTTPResponse, IStartGameResponse
		{
			private Game game;

			public Game Game
			{
				get
				{
					return game;
				}
			}

			public StartGameResponse(IHTTPResponse httpResponse)
				: base(httpResponse)
			{
				Debug.Log("Deserializing " + httpResponse.Text);
				try
				{
					game = JsonConvert.DeserializeObject<Game>(httpResponse.Text);
				}
				catch (JsonSerializationException)
				{
					Debug.LogWarning("Unable to parse game" + httpResponse.Text);
				}
				Debug.Log("Deserialized to " + game);
			}
		}

		public class SetGameResultResponse : HTTPResponseDelegate, IHTTPResponse, ISetGameResultResponse
		{
			private GameResult gameResult;

			public GameResult GameResult
			{
				get
				{
					return gameResult;
				}
			}

			public SetGameResultResponse(IHTTPResponse httpResponse)
				: base(httpResponse)
			{
				Debug.Log("Deserializing " + httpResponse.Text);
				try
				{
					gameResult = JsonConvert.DeserializeObject<GameResult>(httpResponse.Text, converters);
				}
				catch (JsonSerializationException)
				{
					Debug.LogWarning("Unable to parse gameResult" + httpResponse.Text);
				}
				Debug.Log("Deserialized to " + gameResult);
			}
		}

		public class GetIglooInstanceResponse : HTTPResponseDelegate, IHTTPResponse, IGetIglooInstanceResponse
		{
			private string iglooInstanceId;

			public string IglooInstanceId
			{
				get
				{
					return iglooInstanceId;
				}
			}

			public GetIglooInstanceResponse(IHTTPResponse httpResponse)
				: base(httpResponse)
			{
				iglooInstanceId = httpResponse.Text;
			}
		}

		public class GetIgloosResponse : HTTPResponseDelegate, IHTTPResponse, IGetIgloosResponse
		{
			private List<IglooVillageEntry> igloos;

			public List<IglooVillageEntry> Igloos
			{
				get
				{
					return igloos;
				}
			}

			public GetIgloosResponse(IglooListType type, IHTTPResponse httpResponse)
				: base(httpResponse)
			{
				Dictionary<string, List<IglooVillageEntry>> dictionary = JsonConvert.DeserializeObject<Dictionary<string, List<IglooVillageEntry>>>(httpResponse.Text);
				igloos = dictionary[type.ToString()];
			}
		}

		public class GetPuffleResponse : HTTPResponseDelegate, IHTTPResponse, IGetPuffleResponse
		{
			private Puffle puffle;

			public Puffle Puffle
			{
				get
				{
					return puffle;
				}
			}

			public GetPuffleResponse(IHTTPResponse httpResponse)
				: base(httpResponse)
			{
				puffle = JsonConvert.DeserializeObject<Puffle>(httpResponse.Text);
			}
		}

		public class GetPufflesResponse : HTTPResponseDelegate, IHTTPResponse, IGetPufflesResponse
		{
			private List<Puffle> puffles;

			public List<Puffle> Puffles
			{
				get
				{
					return puffles;
				}
			}

			public GetPufflesResponse(IHTTPResponse httpResponse)
				: base(httpResponse)
			{
				puffles = JsonConvert.DeserializeObject<List<Puffle>>(httpResponse.Text);
			}
		}

		public class CreatePuffleResponse : HTTPResponseDelegate, IHTTPResponse, ICreatePuffleResponse
		{
			private Puffle puffle;

			public Puffle Puffle
			{
				get
				{
					return puffle;
				}
			}

			public CreatePuffleResponse(IHTTPResponse httpResponse)
				: base(httpResponse)
			{
				Debug.Log("Deserializing " + httpResponse.Text);
				puffle = JsonConvert.DeserializeObject<Puffle>(httpResponse.Text);
				Debug.Log("Deserialized to " + puffle);
			}
		}

		public class GetSoundStudioTracksDataResponse : HTTPResponseDelegate, IHTTPResponse, IGetSoundStudioTracksDataResponse
		{
			private List<SoundStudioTrackData> tracks;

			public List<SoundStudioTrackData> Tracks
			{
				get
				{
					if (tracks == null)
					{
						tracks = new List<SoundStudioTrackData>();
					}
					return tracks;
				}
			}

			public GetSoundStudioTracksDataResponse(IHTTPResponse httpResponse)
				: base(httpResponse)
			{
				if (!httpResponse.IsError)
				{
					tracks = JsonConvert.DeserializeObject<List<SoundStudioTrackData>>(httpResponse.Text);
				}
			}
		}

		public class GetSoundStudioRadioTracksDataResponse : HTTPResponseDelegate, IHTTPResponse, IGetSoundStudioRadioTracksDataResponse
		{
			private List<SoundStudioRadioTrackData> tracks;

			public List<SoundStudioRadioTrackData> Tracks
			{
				get
				{
					if (tracks == null)
					{
						tracks = new List<SoundStudioRadioTrackData>();
					}
					return tracks;
				}
			}

			public GetSoundStudioRadioTracksDataResponse(IHTTPResponse httpResponse)
				: base(httpResponse)
			{
				if (!httpResponse.IsError)
				{
					tracks = JsonConvert.DeserializeObject<List<SoundStudioRadioTrackData>>(httpResponse.Text);
				}
			}
		}

		public class GetSoundStudioTrackDataResponse : HTTPResponseDelegate, IHTTPResponse, IGetSoundStudioTrackDataResponse
		{
			private SoundStudioTrackData track;

			public SoundStudioTrackData SoundStudioTrackData
			{
				get
				{
					if (track == null)
					{
						track = new SoundStudioTrackData();
					}
					return track;
				}
			}

			public GetSoundStudioTrackDataResponse(IHTTPResponse httpResponse)
				: base(httpResponse)
			{
				if (!httpResponse.IsError)
				{
					track = JsonConvert.DeserializeObject<SoundStudioTrackData>(httpResponse.Text);
				}
			}
		}

		public class DeleteSoundStudioTrackResponse : HTTPResponseDelegate, IHTTPResponse
		{
			public DeleteSoundStudioTrackResponse(IHTTPResponse httpResponse)
				: base(httpResponse)
			{
			}
		}

		public class ListPlayerIDs
		{
			private List<long> players;

			[JsonProperty("players")]
			public List<long> Players
			{
				get
				{
					if (players == null)
					{
						players = new List<long>();
					}
					return players;
				}
				set
				{
					players = value;
				}
			}
		}

		public class CanLikeResponse : HTTPResponseDelegate, IHTTPResponse, ICanLikeResponse
		{
			private CanLike canLike;

			public CanLike CanLike
			{
				get
				{
					return canLike;
				}
			}

			public CanLikeResponse(IHTTPResponse httpResponse)
				: base(httpResponse)
			{
				Debug.Log("Deserializing " + httpResponse.Text);
				canLike = JsonConvert.DeserializeObject<CanLike>(httpResponse.Text);
				Debug.Log("Deserialized to " + canLike);
			}
		}

		public class LikeResponse : HTTPResponseDelegate, IHTTPResponse, ILikeResponse
		{
			private Entity entity;

			public Entity Entity
			{
				get
				{
					return entity;
				}
			}

			public LikeResponse(IHTTPResponse httpResponse)
				: base(httpResponse)
			{
				Debug.Log("Deserializing " + httpResponse.Text);
				entity = JsonConvert.DeserializeObject<Entity>(httpResponse.Text);
				Debug.Log("Deserialized to " + entity);
			}
		}

		public class UnLikeResponse : HTTPResponseDelegate, IHTTPResponse, IUnLikeResponse
		{
			private Entity entity;

			public Entity Entity
			{
				get
				{
					return entity;
				}
			}

			public UnLikeResponse(IHTTPResponse httpResponse)
				: base(httpResponse)
			{
				Debug.Log("Deserializing " + httpResponse.Text);
				entity = JsonConvert.DeserializeObject<Entity>(httpResponse.Text);
				Debug.Log("Deserialized to " + entity);
			}
		}

		public class GetLikedByCountsResponse : HTTPResponseDelegate, IHTTPResponse, IGetLikedByCountsResponse
		{
			private Entity entity;

			public Entity Entity
			{
				get
				{
					return entity;
				}
			}

			public GetLikedByCountsResponse(IHTTPResponse httpResponse)
				: base(httpResponse)
			{
				Debug.Log("Deserializing " + httpResponse.Text);
				entity = JsonConvert.DeserializeObject<Entity>(httpResponse.Text);
				Debug.Log("Deserialized to " + entity);
			}
		}

		public class GetLikeCountsResponse : HTTPResponseDelegate, IHTTPResponse, IGetLikeCountsResponse
		{
			private Entity entity;

			public Entity Entity
			{
				get
				{
					return entity;
				}
			}

			public GetLikeCountsResponse(IHTTPResponse httpResponse)
				: base(httpResponse)
			{
				Debug.Log("Deserializing " + httpResponse.Text);
				entity = JsonConvert.DeserializeObject<Entity>(httpResponse.Text);
				Debug.Log("Deserialized to " + entity);
			}
		}

		public class GetLikedBysResponse : HTTPResponseDelegate, IHTTPResponse, IGetLikedBysResponse
		{
			private Entity entity;

			public Entity Entity
			{
				get
				{
					return entity;
				}
			}

			public GetLikedBysResponse(IHTTPResponse httpResponse)
				: base(httpResponse)
			{
				Debug.Log("Deserializing " + httpResponse.Text);
				entity = JsonConvert.DeserializeObject<Entity>(httpResponse.Text);
				Debug.Log("Deserialized to " + entity);
			}
		}

		public class GetLikesResponse : HTTPResponseDelegate, IHTTPResponse, IGetLikesResponse
		{
			private Entity entity;

			public Entity Entity
			{
				get
				{
					return entity;
				}
			}

			public GetLikesResponse(IHTTPResponse httpResponse)
				: base(httpResponse)
			{
				Debug.Log("Deserializing " + httpResponse.Text);
				entity = JsonConvert.DeserializeObject<Entity>(httpResponse.Text);
				Debug.Log("Deserialized to " + entity);
			}
		}

		public class DeleteEntityResponse : HTTPResponseDelegate, IHTTPResponse, IDeleteEntityResponse
		{
			public DeleteEntityResponse(IHTTPResponse httpResponse)
				: base(httpResponse)
			{
			}
		}

		public class DeleteLikesResponse : HTTPResponseDelegate, IHTTPResponse, IDeleteLikesResponse
		{
			private Entity entity;

			public Entity Entity
			{
				get
				{
					return entity;
				}
			}

			public DeleteLikesResponse(IHTTPResponse httpResponse)
				: base(httpResponse)
			{
				Debug.Log("Deserializing " + httpResponse.Text);
				entity = JsonConvert.DeserializeObject<Entity>(httpResponse.Text);
				Debug.Log("Deserialized to " + entity);
			}
		}

		public class DeleteLikedBysResponse : HTTPResponseDelegate, IHTTPResponse, IDeleteLikedBysResponse
		{
			public DeleteLikedBysResponse(IHTTPResponse httpResponse)
				: base(httpResponse)
			{
			}
		}

		public class RegisterEntityResponse : HTTPResponseDelegate, IHTTPResponse, IRegisterEntityResponse
		{
			private Entity entity;

			public Entity Entity
			{
				get
				{
					return entity;
				}
			}

			public RegisterEntityResponse(IHTTPResponse httpResponse)
				: base(httpResponse)
			{
				Debug.Log("Deserializing " + httpResponse.Text);
				entity = JsonConvert.DeserializeObject<Entity>(httpResponse.Text);
				Debug.Log("Deserialized to " + entity);
			}
		}

		public class SetEntityClassResponse : HTTPResponseDelegate, IHTTPResponse, ISetEntityClassResponse
		{
			public SetEntityClassResponse(IHTTPResponse httpResponse)
				: base(httpResponse)
			{
			}
		}

		public class GetEntityClassResponse : HTTPResponseDelegate, IHTTPResponse, IGetEntityClassResponse
		{
			private EntityClass entityClass;

			public EntityClass EntityClass
			{
				get
				{
					return entityClass;
				}
			}

			public GetEntityClassResponse(IHTTPResponse httpResponse)
				: base(httpResponse)
			{
				Debug.Log("Deserializing " + httpResponse.Text);
				entityClass = JsonConvert.DeserializeObject<EntityClass>(httpResponse.Text);
				Debug.Log("Deserialized to " + entityClass);
			}
		}

		public class MyHighScoreResponse : HTTPResponseDelegate, IHTTPResponse, IMyHighScoreResponse
		{
			private int score;

			public int Score
			{
				get
				{
					return score;
				}
			}

			public MyHighScoreResponse(IHTTPResponse httpResponse)
				: base(httpResponse)
			{
				Debug.Log("Deserializing " + httpResponse.Text);
				if (!int.TryParse(httpResponse.Text, out score))
				{
					score = 0;
				}
				Debug.Log("Deserialized to " + score);
			}
		}

		public class MyGetLeaderBoardResponse : HTTPResponseDelegate, IHTTPResponse, IGetLeaderBoardResponse
		{
			private LeaderBoardResponse leaderBoard;

			public LeaderBoardResponse LeaderBoard
			{
				get
				{
					return leaderBoard;
				}
			}

			public MyGetLeaderBoardResponse(IHTTPResponse httpResponse)
				: base(httpResponse)
			{
				if (httpResponse.IsError)
				{
					Debug.LogError("Error fetching leaderboard: " + httpResponse.StatusCode + "\n" + httpResponse.Text);
					leaderBoard = createEmptyResponse();
					return;
				}
				try
				{
					Debug.Log("Deserializing " + httpResponse.Text);
					leaderBoard = JsonConvert.DeserializeObject<LeaderBoardResponse>(httpResponse.Text);
					Debug.Log("Deserialized to " + leaderBoard);
				}
				catch (JsonSerializationException ex)
				{
					Debug.LogError("Invalid leaderboard response: " + httpResponse.Text + "\n" + ex.ToString());
					leaderBoard = createEmptyResponse();
				}
			}

			private LeaderBoardResponse createEmptyResponse()
			{
				LeaderBoardResponse leaderBoardResponse = new LeaderBoardResponse();
				leaderBoardResponse.Countdown = 0;
				leaderBoardResponse.Players = new List<LeaderBoardHighScore>();
				return leaderBoardResponse;
			}
		}

		public class GetLeaderBoardRewardStatusResponse : HTTPResponseDelegate, IHTTPResponse, IGetLeaderBoardRewardStatus
		{
			private LeaderBoardRewardStatus rewardStatus;

			public LeaderBoardRewardStatus RewardStatus
			{
				get
				{
					return rewardStatus;
				}
			}

			public GetLeaderBoardRewardStatusResponse(IHTTPResponse httpResponse)
				: base(httpResponse)
			{
				if (httpResponse.IsError)
				{
					Debug.LogError("Error fetching leaderboard reward status: " + httpResponse.StatusCode + "\n" + httpResponse.Text);
					rewardStatus = createEmptyResponse();
					return;
				}
				try
				{
					Debug.Log("Deserializing " + httpResponse.Text);
					rewardStatus = JsonConvert.DeserializeObject<LeaderBoardRewardStatus>(httpResponse.Text);
					Debug.Log("Deserialized to " + rewardStatus);
				}
				catch (JsonSerializationException ex)
				{
					Debug.LogError("Invalid leaderboard response: " + httpResponse.Text + "\n" + ex.ToString());
					rewardStatus = createEmptyResponse();
				}
			}

			private LeaderBoardRewardStatus createEmptyResponse()
			{
				LeaderBoardRewardStatus leaderBoardRewardStatus = new LeaderBoardRewardStatus();
				leaderBoardRewardStatus.Status = LeaderBoardRewardStatus.RewardStatus.NOT_THE_LEADER;
				leaderBoardRewardStatus.SecondsTillNextCheck = 1;
				return leaderBoardRewardStatus;
			}
		}

		public class GetIAPProductsResponse : HTTPResponseDelegate, IHTTPResponse, IGetIAPProductsResponse
		{
			private GetProductsResponse products;

			public GetProductsResponse Products
			{
				get
				{
					return products;
				}
			}

			public GetIAPProductsResponse(IHTTPResponse httpResponse)
				: base(httpResponse)
			{
				Debug.Log("Deserializing " + httpResponse.Text);
				products = JsonConvert.DeserializeObject<GetProductsResponse>(httpResponse.Text);
				if (products != null)
				{
					Debug.Log("Deserialized to " + products.ToString());
				}
			}
		}

		public class GetIAPPurchasesResponse : HTTPResponseDelegate, IHTTPResponse, IGetIAPPurchasesResponse
		{
			private IList<ProductPurchase> products;

			public IList<ProductPurchase> Products
			{
				get
				{
					return products;
				}
			}

			public GetIAPPurchasesResponse(IHTTPResponse httpResponse)
				: base(httpResponse)
			{
				Debug.Log("Deserializing " + httpResponse.Text);
				products = JsonConvert.DeserializeObject<IList<ProductPurchase>>(httpResponse.Text);
				if (products != null)
				{
					Debug.Log("Deserialized to " + products.ToString());
				}
			}
		}

		public class ClaimProductsForMemberResponse : HTTPResponseDelegate, IHTTPResponse, IClaimProductsForMemberResponse
		{
			public ClaimProductsForMemberResponse(IHTTPResponse httpResponse)
				: base(httpResponse)
			{
			}
		}

		public class RegisterApplePurchaseResponse : HTTPResponseDelegate, IHTTPResponse, IRegisterApplePurchaseResponse
		{
			private IList<ProductPurchase> result;

			public IList<ProductPurchase> Result
			{
				get
				{
					return result;
				}
			}

			public RegisterApplePurchaseResponse(IHTTPResponse httpResponse)
				: base(httpResponse)
			{
				Debug.Log("Deserializing " + httpResponse.Text);
				result = JsonConvert.DeserializeObject<IList<ProductPurchase>>(httpResponse.Text);
				if (result != null)
				{
					Debug.Log("Deserialized to " + result.ToString());
				}
			}
		}

		public class RegisterGooglePlayPurchaseResponse : HTTPResponseDelegate, IHTTPResponse, IRegisterGooglePlayPurchaseResponse
		{
			private IList<ProductPurchase> result;

			public IList<ProductPurchase> Result
			{
				get
				{
					return result;
				}
			}

			public RegisterGooglePlayPurchaseResponse(IHTTPResponse httpResponse)
				: base(httpResponse)
			{
				Debug.Log("Deserializing " + httpResponse.Text);
				result = JsonConvert.DeserializeObject<IList<ProductPurchase>>(httpResponse.Text);
				if (result != null)
				{
					Debug.Log("Deserialized to " + result.ToString());
				}
			}
		}

		private static IMWSClient instance = new MWSClient();

		private static readonly JsonConverter[] converters = new JsonConverter[1]
		{
			new StringEnumConverter()
		};

		private IDirectoryServiceClient _directoryServiceClient;

		private IHTTPRequestFactory _httpRequestFactory;

		private string username;

		private string password;

		private string authToken;

		private GetPlayerCardDataResponse cachedPlayerCardData;

		private Queue<Action<IGetPlayerCardDataResponse>> playerCardDataResponseHandlers = new Queue<Action<IGetPlayerCardDataResponse>>();

		public static IMWSClient Instance
		{
			get
			{
				return instance;
			}
		}

		[Inject]
		public IDirectoryServiceClient directoryServiceClient
		{
			get
			{
				if (_directoryServiceClient == null)
				{
					return DirectoryServiceClient.Instance;
				}
				return _directoryServiceClient;
			}
			set
			{
				_directoryServiceClient = value;
			}
		}

		[Inject]
		public IHTTPRequestFactory httpRequestFactory
		{
			get
			{
				if (_httpRequestFactory == null)
				{
					return HTTPUnityRequestFactory.Instance;
				}
				return _httpRequestFactory;
			}
			set
			{
				_httpRequestFactory = value;
			}
		}

		public string Username
		{
			get
			{
				return username;
			}
		}

		public string Password
		{
			get
			{
				return password;
			}
		}

		public string AuthToken
		{
			get
			{
				return authToken;
			}
		}

		public string CellophaneToken
		{
			get
			{
				if (directoryServiceClient.IsDev || directoryServiceClient.IsInt || directoryServiceClient.IsQA)
				{
					return "0869F69C-56CC-4EEB-9D5E-D3887F971033:AF5DC9E77FB4FE15F8C051ABC544435A410B46B725CE94B5";
				}
				return "9B407F96-418B-4E0B-93DB-3AD33CC7D72E:205EF7823B24EE5277E318E061E5557F4648F1BF4CCFB457";
			}
		}

		public MWSClient()
		{
			Request.LogAllRequests = true;
			Request.VerboseLogging = true;
		}

		public IGetAuthTokenResponse GetAuthToken(string appId, string appVersion, string username, string password, Action<IGetAuthTokenResponse> responseHandler = null)
		{
			string previousAuthToken = authToken;
			ClearAuthToken();
			this.username = username;
			this.password = password;
			string endpointPath = "/authToken";
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection.Add("appId", appId);
			nameValueCollection.Add("appVersion", appVersion);
			MWSRequest mWSRequest = new MWSRequest(this, "GET", endpointPath, nameValueCollection);
			if (responseHandler == null)
			{
				IHTTPResponse httpResponse2 = mWSRequest.Send();
				GetAuthTokenResponse getAuthTokenResponse = new GetAuthTokenResponse(httpResponse2);
				if (!getAuthTokenResponse.IsError)
				{
					authToken = getAuthTokenResponse.AuthData.AuthToken;
				}
				else
				{
					authToken = previousAuthToken;
				}
				return getAuthTokenResponse;
			}
			mWSRequest.Send(delegate(IHTTPResponse httpResponse)
			{
				GetAuthTokenResponse getAuthTokenResponse2 = new GetAuthTokenResponse(httpResponse);
				if (!getAuthTokenResponse2.IsError)
				{
					authToken = getAuthTokenResponse2.AuthData.AuthToken;
				}
				else
				{
					authToken = previousAuthToken;
				}
				responseHandler(getAuthTokenResponse2);
			});
			return null;
		}

		public void ClearAuthToken(Action responseHandler = null)
		{
			cachedPlayerCardData = null;
			playerCardDataResponseHandlers.Clear();
			authToken = null;
			username = null;
			password = null;
			if (responseHandler != null)
			{
				responseHandler.Invoke();
			}
		}

		public IGetAccountResponse GetAccount(Action<IGetAccountResponse> responseHandler = null)
		{
			string endpointPath = "/account";
			MWSRequest mWSRequest = new MWSRequest(this, "GET", endpointPath);
			if (responseHandler == null)
			{
				IHTTPResponse httpResponse2 = mWSRequest.Send();
				return new GetAccountResponse(httpResponse2);
			}
			mWSRequest.Send(delegate(IHTTPResponse httpResponse)
			{
				responseHandler(new GetAccountResponse(httpResponse));
			});
			return null;
		}

		public ICreateAccountResponse CreateAccount(string username, string password, string appVersion, string appId, string email, int colour, int language, Action<ICreateAccountResponse> responseHandler = null)
		{
			string endpointPath = "/account";
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection.Add("appVersion", appVersion);
			nameValueCollection.Add("appId", appId);
			Hashtable hashtable = new Hashtable();
			hashtable.Add("username", username);
			hashtable.Add("password", password);
			hashtable.Add("email", email);
			hashtable.Add("color", Convert.ToString(colour));
			hashtable.Add("language", Convert.ToString(language));
			MWSRequest mWSRequest = new MWSRequest(this, "PUT", endpointPath, nameValueCollection, hashtable);
			if (responseHandler == null)
			{
				IHTTPResponse httpResponse2 = mWSRequest.Send();
				return new CreateAccountResponse(httpResponse2);
			}
			mWSRequest.Send(delegate(IHTTPResponse httpResponse)
			{
				responseHandler(new CreateAccountResponse(httpResponse));
			});
			return null;
		}

		public IGetIdentityResponse GetIdentity(string id, int language, Action<IGetIdentityResponse> responseHandler = null)
		{
			string endpointPath = "/identity/" + id;
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection.Add("language", Convert.ToString(language));
			MWSRequest mWSRequest = new MWSRequest(this, "GET", endpointPath, nameValueCollection);
			if (responseHandler == null)
			{
				IHTTPResponse httpResponse2 = mWSRequest.Send();
				return new GetIdentityResponse(httpResponse2);
			}
			mWSRequest.Send(delegate(IHTTPResponse httpResponse)
			{
				responseHandler(new GetIdentityResponse(httpResponse));
			});
			return null;
		}

		public IHTTPResponse RequestActivationEmailResent(Action<IHTTPResponse> responseHandler = null)
		{
			string endpointPath = "/resendActivationEmail";
			MWSRequest mWSRequest = new MWSRequest(this, "POST", endpointPath);
			if (responseHandler == null)
			{
				return mWSRequest.Send();
			}
			mWSRequest.Send(delegate(IHTTPResponse httpResponse)
			{
				responseHandler(httpResponse);
			});
			return null;
		}

		public IGetPlayerCardDataResponse GetPlayerCardData(Action<IGetPlayerCardDataResponse> responseHandler = null, bool forceReload = false)
		{
			string endpointPath = "/card-data";
			if (!forceReload && cachedPlayerCardData != null)
			{
				if (responseHandler == null)
				{
					return cachedPlayerCardData;
				}
				responseHandler(cachedPlayerCardData);
				return null;
			}
			MWSRequest mWSRequest = new MWSRequest(this, "GET", endpointPath);
			if (responseHandler == null)
			{
				IHTTPResponse httpResponse2 = mWSRequest.Send();
				cachedPlayerCardData = new GetPlayerCardDataResponse(httpResponse2);
				return cachedPlayerCardData;
			}
			playerCardDataResponseHandlers.Enqueue(responseHandler);
			if (playerCardDataResponseHandlers.Count == 1)
			{
				mWSRequest.Send(delegate(IHTTPResponse httpResponse)
				{
					cachedPlayerCardData = new GetPlayerCardDataResponse(httpResponse);
					foreach (Action<IGetPlayerCardDataResponse> playerCardDataResponseHandler in playerCardDataResponseHandlers)
					{
						playerCardDataResponseHandler(cachedPlayerCardData);
					}
					playerCardDataResponseHandlers.Clear();
				});
			}
			return null;
		}

		public IGetOutfitResponse GetOutfit(Action<IGetOutfitResponse> responseHandler = null)
		{
			string endpointPath = "/outfit";
			MWSRequest mWSRequest = new MWSRequest(this, "GET", endpointPath);
			if (responseHandler == null)
			{
				IHTTPResponse httpResponse2 = mWSRequest.Send();
				return new GetOutfitResponse(httpResponse2);
			}
			mWSRequest.Send(delegate(IHTTPResponse httpResponse)
			{
				responseHandler(new GetOutfitResponse(httpResponse));
			});
			return null;
		}

		public IGetOutfitResponse GetPlayerOutfit(long playerId, Action<IGetOutfitResponse> responseHandler = null)
		{
			string endpointPath = "/" + playerId + "/outfit";
			MWSRequest mWSRequest = new MWSRequest(this, "GET", endpointPath);
			if (responseHandler == null)
			{
				IHTTPResponse httpResponse2 = mWSRequest.Send();
				return new GetOutfitResponse(httpResponse2);
			}
			mWSRequest.Send(delegate(IHTTPResponse httpResponse)
			{
				responseHandler(new GetOutfitResponse(httpResponse));
			});
			return null;
		}

		public ISetOutfitResponse SetOutfit(Outfit outfit, Action<ISetOutfitResponse> responseHandler = null)
		{
			string endpointPath = "/outfit";
			Hashtable hashtable = new Hashtable();
			hashtable.Add("outfit", outfit.ToDelimitedString());
			MWSRequest mWSRequest = new MWSRequest(this, "PUT", endpointPath, null, hashtable);
			if (responseHandler == null)
			{
				IHTTPResponse httpResponse2 = mWSRequest.Send();
				return new SetOutfitResponse(httpResponse2);
			}
			mWSRequest.Send(delegate(IHTTPResponse httpResponse)
			{
				responseHandler(new SetOutfitResponse(httpResponse));
			});
			return null;
		}

		public IPurchaseItemResponse PurchaseItem(string itemType, long itemId, Action<IPurchaseItemResponse> responseHandler = null)
		{
			long num = 0L;
			string endpointPath = "/purchase";
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection.Add("catalogId", Convert.ToString(num));
			nameValueCollection.Add("itemType", itemType);
			nameValueCollection.Add("itemId", Convert.ToString(itemId));
			MWSRequest mWSRequest = new MWSRequest(this, "POST", endpointPath, nameValueCollection);
			if (responseHandler == null)
			{
				IHTTPResponse httpResponse2 = mWSRequest.Send();
				return new PurchaseItemResponse(httpResponse2);
			}
			mWSRequest.Send(delegate(IHTTPResponse httpResponse)
			{
				responseHandler(new PurchaseItemResponse(httpResponse));
			});
			return null;
		}

		public ISetCoinsResponse SetCoins(int coins, string appId, Action<ISetCoinsResponse> responseHandler = null)
		{
			string endpointPath = "/coins";
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection.Add("appId", appId);
			Hashtable hashtable = new Hashtable();
			hashtable.Add("coins", coins);
			MWSRequest mWSRequest = new MWSRequest(this, "PUT", endpointPath, nameValueCollection, hashtable);
			if (responseHandler == null)
			{
				IHTTPResponse httpResponse2 = mWSRequest.Send();
				return new SetCoinsResponse(httpResponse2);
			}
			mWSRequest.Send(delegate(IHTTPResponse httpResponse)
			{
				responseHandler(new SetCoinsResponse(httpResponse));
			});
			return null;
		}

		public ISetCoinsResponse IncrementCoins(int amount, string appId, Action<ISetCoinsResponse> responseHandler = null)
		{
			string endpointPath = "/coins";
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection.Add("appId", appId);
			Hashtable hashtable = new Hashtable();
			Hashtable hashtable2 = new Hashtable();
			hashtable2.Add("coins", amount);
			hashtable.Add("$inc", hashtable2);
			MWSRequest mWSRequest = new MWSRequest(this, "PUT", endpointPath, nameValueCollection, hashtable);
			if (responseHandler == null)
			{
				IHTTPResponse httpResponse2 = mWSRequest.Send();
				return new SetCoinsResponse(httpResponse2);
			}
			mWSRequest.Send(delegate(IHTTPResponse httpResponse)
			{
				responseHandler(new SetCoinsResponse(httpResponse));
			});
			return null;
		}

		public IStartGameResponse StartGame(string gameType, Action<IStartGameResponse> responseHandler = null)
		{
			string endpointPath = "/game";
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection.Add("type", gameType);
			MWSRequest mWSRequest = new MWSRequest(this, "POST", endpointPath, nameValueCollection);
			if (responseHandler == null)
			{
				IHTTPResponse httpResponse2 = mWSRequest.Send();
				return new StartGameResponse(httpResponse2);
			}
			mWSRequest.Send(delegate(IHTTPResponse httpResponse)
			{
				responseHandler(new StartGameResponse(httpResponse));
			});
			return null;
		}

		public ISetGameResultResponse SetGameResult(GameResult gameResult, Action<ISetGameResultResponse> responseHandler = null)
		{
			string endpointPath = "/game/" + gameResult.gameId + "/result";
			MWSRequest mWSRequest = new MWSRequest(this, "PUT", endpointPath, null, gameResult, converters);
			if (responseHandler == null)
			{
				IHTTPResponse httpResponse2 = mWSRequest.Send();
				return new SetGameResultResponse(httpResponse2);
			}
			mWSRequest.Send(delegate(IHTTPResponse httpResponse)
			{
				responseHandler(new SetGameResultResponse(httpResponse));
			});
			return null;
		}

		public IGetIglooInstanceResponse GetIglooInstance(string ownerId, string language, Action<IGetIglooInstanceResponse> responseHandler = null)
		{
			string endpointPath = "/igloo/instance/" + ownerId + "/" + language;
			MWSRequest mWSRequest = new MWSRequest(this, "GET", endpointPath);
			if (responseHandler == null)
			{
				IHTTPResponse httpResponse2 = mWSRequest.Send();
				return new GetIglooInstanceResponse(httpResponse2);
			}
			mWSRequest.Send(delegate(IHTTPResponse httpResponse)
			{
				responseHandler(new GetIglooInstanceResponse(httpResponse));
			});
			return null;
		}

		public IGetIgloosResponse GetIgloos(IglooListType type, string language, int limit, Action<IGetIgloosResponse> responseHandler = null)
		{
			string endpointPath = string.Concat("/igloo/village/", type, "/", language);
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection.Add("limit", Convert.ToString(limit));
			MWSRequest mWSRequest = new MWSRequest(this, "GET", endpointPath, nameValueCollection);
			if (responseHandler == null)
			{
				IHTTPResponse httpResponse2 = mWSRequest.Send();
				return new GetIgloosResponse(type, httpResponse2);
			}
			mWSRequest.Send(delegate(IHTTPResponse httpResponse)
			{
				responseHandler(new GetIgloosResponse(type, httpResponse));
			});
			return null;
		}

		public IGetPuffleResponse GetPuffle(long puffleId, Action<IGetPuffleResponse> responseHandler = null)
		{
			string endpointPath = "/puffle/" + puffleId;
			MWSRequest mWSRequest = new MWSRequest(this, "GET", endpointPath);
			if (responseHandler == null)
			{
				IHTTPResponse httpResponse2 = mWSRequest.Send();
				return new GetPuffleResponse(httpResponse2);
			}
			mWSRequest.Send(delegate(IHTTPResponse httpResponse)
			{
				responseHandler(new GetPuffleResponse(httpResponse));
			});
			return null;
		}

		public IGetPufflesResponse GetPuffles(Action<IGetPufflesResponse> responseHandler = null)
		{
			string endpointPath = "/puffle";
			MWSRequest mWSRequest = new MWSRequest(this, "GET", endpointPath);
			if (responseHandler == null)
			{
				IHTTPResponse httpResponse2 = mWSRequest.Send();
				return new GetPufflesResponse(httpResponse2);
			}
			mWSRequest.Send(delegate(IHTTPResponse httpResponse)
			{
				responseHandler(new GetPufflesResponse(httpResponse));
			});
			return null;
		}

		public ICreatePuffleResponse CreatePuffle(int type, string name, int languageCode, Action<ICreatePuffleResponse> responseHandler = null)
		{
			string endpointPath = "/puffle";
			Hashtable hashtable = new Hashtable();
			hashtable.Add("type", type);
			hashtable.Add("name", name);
			hashtable.Add("language", languageCode);
			MWSRequest mWSRequest = new MWSRequest(this, "POST", endpointPath, null, hashtable);
			if (responseHandler == null)
			{
				IHTTPResponse httpResponse2 = mWSRequest.Send();
				return new CreatePuffleResponse(httpResponse2);
			}
			mWSRequest.Send(delegate(IHTTPResponse httpResponse)
			{
				responseHandler(new CreatePuffleResponse(httpResponse));
			});
			return null;
		}

		public IHTTPResponse UpdatePuffle(int puffleId, int play, int rest, int clean, long headItemId, long location, Action<IHTTPResponse> responseHandler = null)
		{
			string endpointPath = "/puffle/" + puffleId;
			Hashtable hashtable = new Hashtable();
			hashtable.Add("play", play);
			hashtable.Add("rest", rest);
			hashtable.Add("clean", clean);
			hashtable.Add("headItemId", headItemId);
			hashtable.Add("location", location);
			MWSRequest mWSRequest = new MWSRequest(this, "PUT", endpointPath, null, hashtable);
			if (responseHandler == null)
			{
				return mWSRequest.Send();
			}
			mWSRequest.Send(delegate(IHTTPResponse httpResponse)
			{
				responseHandler(httpResponse);
			});
			return null;
		}

		public IHTTPResponse DeleteSoundStudioTrack(long trackId, Action<IHTTPResponse> responseHandler = null)
		{
			MWSRequest mWSRequest = new MWSRequest(this, "DELETE", "inventory/soundstudio/track/" + trackId);
			if (responseHandler == null)
			{
				IHTTPResponse httpResponse2 = mWSRequest.Send();
				return new DeleteSoundStudioTrackResponse(httpResponse2);
			}
			mWSRequest.Send(delegate(IHTTPResponse httpResponse)
			{
				responseHandler(new DeleteSoundStudioTrackResponse(httpResponse));
			});
			return null;
		}

		public IGetSoundStudioTrackDataResponse AddSoundStudioTrack(SoundStudioTrackData soundStudioTrackData, Action<IGetSoundStudioTrackDataResponse> responseHandler = null)
		{
			string body = JsonConvert.SerializeObject(soundStudioTrackData);
			MWSRequest mWSRequest = new MWSRequest(this, "POST", "/inventory/soundstudio/track", null, body);
			if (responseHandler == null)
			{
				IHTTPResponse httpResponse2 = mWSRequest.Send();
				return new GetSoundStudioTrackDataResponse(httpResponse2);
			}
			mWSRequest.Send(delegate(IHTTPResponse httpResponse)
			{
				responseHandler(new GetSoundStudioTrackDataResponse(httpResponse));
			});
			return null;
		}

		public IGetSoundStudioTracksDataResponse GetSoundStudioTracksListing(Action<IGetSoundStudioTracksDataResponse> responseHandler = null)
		{
			MWSRequest mWSRequest = new MWSRequest(this, "GET", "/inventory/soundstudio/trackslisting");
			if (responseHandler == null)
			{
				IHTTPResponse httpResponse2 = mWSRequest.Send();
				return new GetSoundStudioTracksDataResponse(httpResponse2);
			}
			mWSRequest.Send(delegate(IHTTPResponse httpResponse)
			{
				responseHandler(new GetSoundStudioTracksDataResponse(httpResponse));
			});
			return null;
		}

		public IGetSoundStudioTrackDataResponse GetSoundStudioTrack(long playerId, long trackId, Action<IGetSoundStudioTrackDataResponse> responseHandler = null)
		{
			MWSRequest mWSRequest = new MWSRequest(this, "GET", "/public/inventory/soundstudio/track/" + playerId + "/" + trackId);
			if (responseHandler == null)
			{
				IHTTPResponse httpResponse2 = mWSRequest.Send();
				return new GetSoundStudioTrackDataResponse(httpResponse2);
			}
			mWSRequest.Send(delegate(IHTTPResponse httpResponse)
			{
				responseHandler(new GetSoundStudioTrackDataResponse(httpResponse));
			});
			return null;
		}

		public IGetSoundStudioRadioTracksDataResponse GetSharedSoundStudioTracksListing(List<string> swids, string language, Action<IGetSoundStudioRadioTracksDataResponse> responseHandler = null)
		{
			ListPlayerSwids listPlayerSwids = new ListPlayerSwids();
			listPlayerSwids.Swids = swids;
			string text = JsonConvert.SerializeObject(listPlayerSwids);
			Debug.Log("============== Hey there swids as string: " + text);
			MWSRequest request = new MWSRequest(this, "POST", "/inventory/soundstudio/sharedtrackslisting/" + language, null, text);
			return CreateGetSoundStudioRadioTracksDataResponse(responseHandler, request);
		}

		public IGetSoundStudioRadioTracksDataResponse GetNewSharedSoundStudioTracksListing(string language, int limit, long beforeTrackId, Action<IGetSoundStudioRadioTracksDataResponse> responseHandler = null)
		{
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection.Add("limit", limit.ToString());
			nameValueCollection.Add("beforeTrackId", beforeTrackId.ToString());
			MWSRequest request = new MWSRequest(this, "GET", string.Format("/public/inventory/soundstudio/{0}/newsharedtracks", language), nameValueCollection);
			return CreateGetSoundStudioRadioTracksDataResponse(responseHandler, request);
		}

		public IGetSoundStudioRadioTracksDataResponse GetRandomSharedSoundStudioTracksListing(string language, int limit, Action<IGetSoundStudioRadioTracksDataResponse> responseHandler = null)
		{
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection.Add("limit", limit.ToString());
			MWSRequest request = new MWSRequest(this, "GET", string.Format("/public/inventory/soundstudio/{0}/randomsharedtracks", language), nameValueCollection);
			return CreateGetSoundStudioRadioTracksDataResponse(responseHandler, request);
		}

		private static IGetSoundStudioRadioTracksDataResponse CreateGetSoundStudioRadioTracksDataResponse(Action<IGetSoundStudioRadioTracksDataResponse> responseHandler, MWSRequest request)
		{
			if (responseHandler == null)
			{
				IHTTPResponse httpResponse2 = request.Send();
				return new GetSoundStudioRadioTracksDataResponse(httpResponse2);
			}
			request.Send(delegate(IHTTPResponse httpResponse)
			{
				responseHandler(new GetSoundStudioRadioTracksDataResponse(httpResponse));
			});
			return null;
		}

		public IGetSoundStudioTracksDataResponse GetSoundStudioTracks(Action<IGetSoundStudioTracksDataResponse> responseHandler = null)
		{
			MWSRequest mWSRequest = new MWSRequest(this, "GET", "/inventory/soundstudio/tracks");
			if (responseHandler == null)
			{
				IHTTPResponse httpResponse2 = mWSRequest.Send();
				return new GetSoundStudioTracksDataResponse(httpResponse2);
			}
			mWSRequest.Send(delegate(IHTTPResponse httpResponse)
			{
				responseHandler(new GetSoundStudioTracksDataResponse(httpResponse));
			});
			return null;
		}

		public IHTTPResponse RenameSoundStudioTrack(long trackId, string name, Action<IHTTPResponse> responseHandler = null)
		{
			MWSRequest mWSRequest = new MWSRequest(this, "PUT", "/inventory/soundstudio/renametrack/" + trackId + "/" + name);
			if (responseHandler == null)
			{
				return mWSRequest.Send();
			}
			mWSRequest.Send(delegate(IHTTPResponse httpResponse)
			{
				responseHandler(httpResponse);
			});
			return null;
		}

		public IHTTPResponse UpdateShareStateOfSoundStudioTrack(long trackId, TrackShareState trackShareState, Action<IHTTPResponse> responseHandler = null)
		{
			MWSRequest mWSRequest = new MWSRequest(this, "PUT", "/inventory/soundstudio/sharestatetrack/" + trackId + "/" + trackShareState.ToString());
			if (responseHandler == null)
			{
				return mWSRequest.Send();
			}
			mWSRequest.Send(delegate(IHTTPResponse httpResponse)
			{
				responseHandler(httpResponse);
			});
			return null;
		}

		public ICanLikeResponse CanLike(ID actorID, ID objectID, Action<ICanLikeResponse> responseHandler = null)
		{
			string endpointPath = "/likes/can_like";
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection.Add("ownerID", actorID.Id);
			nameValueCollection.Add("objectID", objectID.Id);
			MWSRequest mWSRequest = new MWSRequest(this, "GET", endpointPath, nameValueCollection);
			if (responseHandler == null)
			{
				IHTTPResponse httpResponse2 = mWSRequest.Send();
				return new CanLikeResponse(httpResponse2);
			}
			mWSRequest.Send(delegate(IHTTPResponse httpResponse)
			{
				responseHandler(new CanLikeResponse(httpResponse));
			});
			return null;
		}

		public ILikeResponse Like(Activity activity, Action<ILikeResponse> responseHandler = null)
		{
			string endpointPath = "/likes/like";
			NameValueCollection parameters = null;
			MWSRequest mWSRequest = new MWSRequest(this, "POST", endpointPath, parameters, activity);
			if (responseHandler == null)
			{
				IHTTPResponse httpResponse2 = mWSRequest.Send();
				return new LikeResponse(httpResponse2);
			}
			mWSRequest.Send(delegate(IHTTPResponse httpResponse)
			{
				responseHandler(new LikeResponse(httpResponse));
			});
			return null;
		}

		public IUnLikeResponse UnLike(Activity activity, Action<IUnLikeResponse> responseHandler = null)
		{
			string endpointPath = "/likes/unlike";
			MWSRequest mWSRequest = new MWSRequest(this, "POST", endpointPath, null, activity);
			if (responseHandler == null)
			{
				IHTTPResponse httpResponse2 = mWSRequest.Send();
				return new UnLikeResponse(httpResponse2);
			}
			mWSRequest.Send(delegate(IHTTPResponse httpResponse)
			{
				responseHandler(new UnLikeResponse(httpResponse));
			});
			return null;
		}

		public IGetLikedByCountsResponse GetLikedByCounts(ID objectID, Action<IGetLikedByCountsResponse> responseHandler = null)
		{
			string endpointPath = "/likes/likedby_counts";
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection.Add("objectID", objectID.Id);
			MWSRequest mWSRequest = new MWSRequest(this, "GET", endpointPath, nameValueCollection);
			if (responseHandler == null)
			{
				IHTTPResponse httpResponse2 = mWSRequest.Send();
				return new GetLikedByCountsResponse(httpResponse2);
			}
			mWSRequest.Send(delegate(IHTTPResponse httpResponse)
			{
				responseHandler(new GetLikedByCountsResponse(httpResponse));
			});
			return null;
		}

		public IGetLikeCountsResponse GetLikeCounts(ID objectID, Action<IGetLikeCountsResponse> responseHandler = null)
		{
			string endpointPath = "/likes/like_counts";
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection.Add("objectID", objectID.Id);
			MWSRequest mWSRequest = new MWSRequest(this, "GET", endpointPath, nameValueCollection);
			if (responseHandler == null)
			{
				IHTTPResponse httpResponse2 = mWSRequest.Send();
				return new GetLikeCountsResponse(httpResponse2);
			}
			mWSRequest.Send(delegate(IHTTPResponse httpResponse)
			{
				responseHandler(new GetLikeCountsResponse(httpResponse));
			});
			return null;
		}

		public IGetLikedBysResponse GetLikedBys(ID objectID, ID actorID, PaginationInfo pInfo, Action<IGetLikedBysResponse> responseHandler = null)
		{
			string endpointPath = "/likes/likedby";
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection.Add("objectID", objectID.Id);
			if (actorID != null)
			{
				nameValueCollection.Add("actorID", actorID.Id);
			}
			if (pInfo != null)
			{
				if (pInfo.Start.HasValue)
				{
					nameValueCollection.Add("start", Convert.ToString(pInfo.Start));
				}
				if (pInfo.Limit.HasValue)
				{
					nameValueCollection.Add("limit", Convert.ToString(pInfo.Limit));
				}
				nameValueCollection.Add("groupby", pInfo.Groupby.ToString());
				nameValueCollection.Add("orderby", pInfo.Orderby.ToString());
			}
			MWSRequest mWSRequest = new MWSRequest(this, "GET", endpointPath, nameValueCollection);
			if (responseHandler == null)
			{
				IHTTPResponse httpResponse2 = mWSRequest.Send();
				return new GetLikedBysResponse(httpResponse2);
			}
			mWSRequest.Send(delegate(IHTTPResponse httpResponse)
			{
				responseHandler(new GetLikedBysResponse(httpResponse));
			});
			return null;
		}

		public IGetLikesResponse GetLikes(ID actorID, PaginationInfo pInfo, Action<IGetLikesResponse> responseHandler = null)
		{
			string endpointPath = "/likes/likes";
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection.Add("objectID", actorID.Id);
			if (pInfo != null)
			{
				if (pInfo.Start.HasValue)
				{
					nameValueCollection.Add("start", Convert.ToString(pInfo.Start));
				}
				if (pInfo.Limit.HasValue)
				{
					nameValueCollection.Add("limit", Convert.ToString(pInfo.Limit));
				}
				nameValueCollection.Add("groupby", pInfo.Groupby.ToString());
				nameValueCollection.Add("orderby", pInfo.Orderby.ToString());
			}
			MWSRequest mWSRequest = new MWSRequest(this, "GET", endpointPath, nameValueCollection);
			if (responseHandler == null)
			{
				IHTTPResponse httpResponse2 = mWSRequest.Send();
				return new GetLikesResponse(httpResponse2);
			}
			mWSRequest.Send(delegate(IHTTPResponse httpResponse)
			{
				responseHandler(new GetLikesResponse(httpResponse));
			});
			return null;
		}

		public IDeleteEntityResponse DeleteEntity(ID objectID, Action<IDeleteEntityResponse> responseHandler = null)
		{
			string endpointPath = "/likes/entity";
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection.Add("objectID", objectID.Id);
			MWSRequest mWSRequest = new MWSRequest(this, "DELETE", endpointPath, nameValueCollection);
			if (responseHandler == null)
			{
				IHTTPResponse httpResponse2 = mWSRequest.Send();
				return new DeleteEntityResponse(httpResponse2);
			}
			mWSRequest.Send(delegate(IHTTPResponse httpResponse)
			{
				responseHandler(new DeleteEntityResponse(httpResponse));
			});
			return null;
		}

		public IDeleteLikesResponse DeleteLikes(ID objectID, Action<IDeleteLikesResponse> responseHandler = null)
		{
			string endpointPath = "/likes/likes";
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection.Add("objectID", objectID.Id);
			MWSRequest mWSRequest = new MWSRequest(this, "DELETE", endpointPath, nameValueCollection);
			if (responseHandler == null)
			{
				IHTTPResponse httpResponse2 = mWSRequest.Send();
				return new DeleteLikesResponse(httpResponse2);
			}
			mWSRequest.Send(delegate(IHTTPResponse httpResponse)
			{
				responseHandler(new DeleteLikesResponse(httpResponse));
			});
			return null;
		}

		public IDeleteLikedBysResponse DeleteLikedBys(ID objectID, bool resetAll, Action<IDeleteLikedBysResponse> responseHandler = null)
		{
			string endpointPath = "/likes/likedby";
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection.Add("objectID", objectID.Id);
			nameValueCollection.Add("resetAll", Convert.ToString(resetAll));
			MWSRequest mWSRequest = new MWSRequest(this, "DELETE", endpointPath, nameValueCollection);
			if (responseHandler == null)
			{
				IHTTPResponse httpResponse2 = mWSRequest.Send();
				return new DeleteLikedBysResponse(httpResponse2);
			}
			mWSRequest.Send(delegate(IHTTPResponse httpResponse)
			{
				responseHandler(new DeleteLikedBysResponse(httpResponse));
			});
			return null;
		}

		public IRegisterEntityResponse RegisterEntity(EntityClass entityClass, Action<IRegisterEntityResponse> responseHandler = null)
		{
			string endpointPath = "/likes/register_entity";
			NameValueCollection parameters = null;
			MWSRequest mWSRequest = new MWSRequest(this, "POST", endpointPath, parameters);
			if (responseHandler == null)
			{
				IHTTPResponse httpResponse2 = mWSRequest.Send();
				return new RegisterEntityResponse(httpResponse2);
			}
			mWSRequest.Send(delegate(IHTTPResponse httpResponse)
			{
				responseHandler(new RegisterEntityResponse(httpResponse));
			});
			return null;
		}

		public ISetEntityClassResponse SetEntityClass(EntityClass entityClass, Action<ISetEntityClassResponse> responseHandler = null)
		{
			string endpointPath = "/likes/entity_class";
			NameValueCollection parameters = null;
			MWSRequest mWSRequest = new MWSRequest(this, "POST", endpointPath, parameters, entityClass);
			if (responseHandler == null)
			{
				IHTTPResponse httpResponse2 = mWSRequest.Send();
				return new SetEntityClassResponse(httpResponse2);
			}
			mWSRequest.Send(delegate(IHTTPResponse httpResponse)
			{
				responseHandler(new SetEntityClassResponse(httpResponse));
			});
			return null;
		}

		public IGetEntityClassResponse GetEntityClass(string context, string type, Action<IGetEntityClassResponse> responseHandler = null)
		{
			string endpointPath = "/likes/entity_class";
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection.Add("context", context);
			nameValueCollection.Add("type", type);
			MWSRequest mWSRequest = new MWSRequest(this, "GET", endpointPath, nameValueCollection);
			if (responseHandler == null)
			{
				IHTTPResponse httpResponse2 = mWSRequest.Send();
				return new GetEntityClassResponse(httpResponse2);
			}
			mWSRequest.Send(delegate(IHTTPResponse httpResponse)
			{
				responseHandler(new GetEntityClassResponse(httpResponse));
			});
			return null;
		}

		public IMyHighScoreResponse GetMyHighScore(string gameType, Action<IMyHighScoreResponse> responseHandler = null)
		{
			string endpointPath = "/leaderboard/highscore";
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection.Add("game", gameType);
			MWSRequest mWSRequest = new MWSRequest(this, "GET", endpointPath, nameValueCollection);
			if (responseHandler == null)
			{
				IHTTPResponse httpResponse2 = mWSRequest.Send();
				return new MyHighScoreResponse(httpResponse2);
			}
			mWSRequest.Send(delegate(IHTTPResponse httpResponse)
			{
				responseHandler(new MyHighScoreResponse(httpResponse));
			});
			return null;
		}

		public IGetLeaderBoardResponse GetLeaderBoard(string gameType, string language, string duration, bool friendsOnly, Action<IGetLeaderBoardResponse> responseHandler = null)
		{
			string text = "/leaderboard/" + language + "/" + duration + "/";
			if (AuthToken == null)
			{
				text = "/public" + text;
			}
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection.Add("game", gameType);
			if (AuthToken != null)
			{
				nameValueCollection.Add("friendsOnly", friendsOnly.ToString());
			}
			MWSRequest mWSRequest = new MWSRequest(this, "GET", text, nameValueCollection);
			if (responseHandler == null)
			{
				IHTTPResponse httpResponse2 = mWSRequest.Send();
				return new MyGetLeaderBoardResponse(httpResponse2);
			}
			mWSRequest.Send(delegate(IHTTPResponse httpResponse)
			{
				responseHandler(new MyGetLeaderBoardResponse(httpResponse));
			});
			return null;
		}

		public IGetLeaderBoardRewardStatus GetLeaderBoardRewardStatus(string gameType, string duration, int periods, Action<IGetLeaderBoardRewardStatus> responseHandler = null)
		{
			string endpointPath = string.Format("/leaderboard/historical/friends/leader/reward/{0}/{1}/", duration, periods);
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection.Add("game", gameType);
			MWSRequest mWSRequest = new MWSRequest(this, "GET", endpointPath, nameValueCollection);
			if (responseHandler == null)
			{
				IHTTPResponse httpResponse2 = mWSRequest.Send();
				return new GetLeaderBoardRewardStatusResponse(httpResponse2);
			}
			mWSRequest.Send(delegate(IHTTPResponse httpResponse)
			{
				responseHandler(new GetLeaderBoardRewardStatusResponse(httpResponse));
			});
			return null;
		}

		public IGetIAPProductsResponse GetIAPProductsForApp(string appId, string appVersion, StoreType store, Action<IGetIAPProductsResponse> responseHandler = null)
		{
			string endpointPath = "/productIdentifiers";
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection.Add("appId", appId);
			nameValueCollection.Add("appVersion", appVersion);
			nameValueCollection.Add("store", store.ToString().ToLower());
			MWSRequest mWSRequest = new MWSRequest(this, "GET", endpointPath, nameValueCollection);
			if (responseHandler == null)
			{
				IHTTPResponse httpResponse2 = mWSRequest.Send();
				return new GetIAPProductsResponse(httpResponse2);
			}
			mWSRequest.Send(delegate(IHTTPResponse httpResponse)
			{
				responseHandler(new GetIAPProductsResponse(httpResponse));
			});
			return null;
		}

		public IGetIAPPurchasesResponse GetIAPPurchasesForPlayer(string[] productIds = null, Action<IGetIAPPurchasesResponse> responseHandler = null)
		{
			string endpointPath = "/purchases";
			NameValueCollection nameValueCollection = new NameValueCollection();
			if (productIds != null)
			{
				nameValueCollection.Add("products", string.Join(",", productIds));
			}
			MWSRequest mWSRequest = new MWSRequest(this, "GET", endpointPath, nameValueCollection);
			if (responseHandler == null)
			{
				IHTTPResponse httpResponse2 = mWSRequest.Send();
				return new GetIAPPurchasesResponse(httpResponse2);
			}
			mWSRequest.Send(delegate(IHTTPResponse httpResponse)
			{
				responseHandler(new GetIAPPurchasesResponse(httpResponse));
			});
			return null;
		}

		public IClaimProductsForMemberResponse ClaimProductsForMember(string deviceId, StoreType store, IList<string> productIds, Action<IClaimProductsForMemberResponse> responseHandler = null)
		{
			string endpointPath = "/claim";
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection.Add("deviceId", deviceId);
			nameValueCollection.Add("store", store.ToString().ToLower());
			MWSRequest mWSRequest = new MWSRequest(this, "POST", endpointPath, nameValueCollection, productIds);
			if (responseHandler == null)
			{
				IHTTPResponse httpResponse2 = mWSRequest.Send();
				return new ClaimProductsForMemberResponse(httpResponse2);
			}
			mWSRequest.Send(delegate(IHTTPResponse httpResponse)
			{
				responseHandler(new ClaimProductsForMemberResponse(httpResponse));
			});
			return null;
		}

		public IRegisterApplePurchaseResponse RegisterApplePurchase(string appId, string deviceId, ApplePurchaseData purchaseData, Action<IRegisterApplePurchaseResponse> responseHandler = null)
		{
			string endpointPath = "/apple/receipt";
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection.Add("deviceId", deviceId);
			MWSRequest mWSRequest = new MWSRequest(this, "POST", endpointPath, nameValueCollection, purchaseData);
			if (responseHandler == null)
			{
				IHTTPResponse httpResponse2 = mWSRequest.Send();
				return new RegisterApplePurchaseResponse(httpResponse2);
			}
			mWSRequest.Send(delegate(IHTTPResponse httpResponse)
			{
				responseHandler(new RegisterApplePurchaseResponse(httpResponse));
			});
			return null;
		}

		public IRegisterGooglePlayPurchaseResponse RegisterGooglePlayPurchase(string appId, string deviceId, GooglePlayPurchaseData purchaseData, Action<IRegisterGooglePlayPurchaseResponse> responseHandler = null)
		{
			string endpointPath = "/google_play/receipt";
			NameValueCollection nameValueCollection = new NameValueCollection();
			nameValueCollection.Add("deviceId", deviceId);
			MWSRequest mWSRequest = new MWSRequest(this, "POST", endpointPath, nameValueCollection, purchaseData);
			if (responseHandler == null)
			{
				IHTTPResponse httpResponse2 = mWSRequest.Send();
				return new RegisterGooglePlayPurchaseResponse(httpResponse2);
			}
			mWSRequest.Send(delegate(IHTTPResponse httpResponse)
			{
				responseHandler(new RegisterGooglePlayPurchaseResponse(httpResponse));
			});
			return null;
		}
	}
}
