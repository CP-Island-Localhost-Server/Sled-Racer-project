using System;
using System.Collections.Generic;
using UnityEngine;

namespace DevonLocalization.Core
{
	public class Localizer
	{
		public delegate void TokensUpdatedDelegate();

		private static Localizer _Instance;

		public TokensUpdatedDelegate TokensUpdated;

		public Language Language = Language.en_US;

		public Dictionary<string, string> tokens = new Dictionary<string, string>();

		public static Localizer Instance
		{
			get
			{
				if (_Instance == null)
				{
					_Instance = new Localizer();
				}
				return _Instance;
			}
		}

		private Localizer()
		{
		}

		public void ResetTokens()
		{
			tokens = new Dictionary<string, string>();
		}

		public string GetTokenTranslation(string token)
		{
			if (tokens.Count == 0)
			{
				Debug.Log("Tokens do not exist, has a loczalized file been loaded?");
				return string.Empty;
			}
			foreach (KeyValuePair<string, string> token2 in tokens)
			{
				if (token == token2.Key)
				{
					return token2.Value;
				}
			}
			return token;
		}

		public void LoadTokensFromLocalJSON(ILocalizedTokenFilePath path, Action<bool> responseHandler = null)
		{
			string empty = string.Empty;
			string empty2 = string.Empty;
			empty = path.GetResourceFilePathForLanguage(LocalizationLanguage.GetLanguageString(Language));
			TextAsset textAsset = Resources.Load<TextAsset>(empty);
			if (textAsset == null)
			{
				Debug.LogWarning("Could not load JSON resource: " + empty);
				responseHandler(false);
			}
			else
			{
				empty2 = textAsset.text;
				UpdateTokensFromJSONText(empty2, responseHandler);
			}
		}

		public void UpdateTokensFromJSONText(string jsonText, Action<bool> responseHandler)
		{
			JSONObject jSONObject = new JSONObject(jsonText);
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			for (int i = 0; i < jSONObject.list.Count; i++)
			{
				if (dictionary.ContainsKey(jSONObject.keys[i]))
				{
					dictionary[jSONObject.keys[i]] = jSONObject.list[i].str;
				}
				else
				{
					dictionary.Add(jSONObject.keys[i], jSONObject.list[i].str);
				}
			}
			if (dictionary == null || dictionary.Count == 0)
			{
				Debug.Log("Could not find any tokens in specified text.");
				return;
			}
			bool obj = false;
			foreach (KeyValuePair<string, string> item in dictionary)
			{
				if (tokens.ContainsKey(item.Key))
				{
					obj = true;
					tokens[item.Key] = item.Value;
				}
				else
				{
					obj = true;
					tokens.Add(item.Key, item.Value);
				}
			}
			if (responseHandler != null)
			{
				responseHandler(obj);
			}
			if (TokensUpdated != null)
			{
				TokensUpdated();
			}
		}
	}
}
