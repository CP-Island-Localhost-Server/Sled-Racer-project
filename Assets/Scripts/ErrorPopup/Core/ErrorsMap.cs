using System;
using System.Collections.Generic;
using UnityEngine;

namespace ErrorPopup.Core
{
	public class ErrorsMap
	{
		private const string RESOURCES_DIRECTORY = "resources";

		public Dictionary<string, string> errors = new Dictionary<string, string>();

		private static ErrorsMap instance;

		public static ErrorsMap Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new ErrorsMap();
				}
				return instance;
			}
		}

		private ErrorsMap()
		{
		}

		public string GetErrorMessage(string id)
		{
			if (errors.Count == 0)
			{
				UnityEngine.Debug.LogWarning("There are no errors.");
				return string.Empty;
			}
			foreach (KeyValuePair<string, string> error in errors)
			{
				if (id == error.Key)
				{
					return error.Value;
				}
			}
			return id;
		}

		public bool IsValidError(string key)
		{
			return errors.ContainsKey(key);
		}

		public void LoadErrorsLocally(string filePath)
		{
			string empty = string.Empty;
			string text = string.Empty;
			try
			{
				int num = filePath.ToLower().IndexOf("resources");
				num = num + "resources".Length + 1;
				text = filePath.Substring(num);
				if (text.Contains(".txt"))
				{
					text = text.Substring(0, text.Length - 4);
				}
				TextAsset textAsset = (TextAsset)Resources.Load(text, typeof(TextAsset));
				empty = textAsset.text;
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.Log("Could not load JSON resource " + text + " : " + ex.Message);
				return;
				IL_0099:;
			}
			UpdateErrorsFromJson(empty);
		}

		private void UpdateErrorsFromJson(string json)
		{
			if (!(json != string.Empty))
			{
				return;
			}
			JSONObject jSONObject = new JSONObject(json);
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
				UnityEngine.Debug.Log("Could not find any tokens in specified text.");
			}
			else
			{
				foreach (KeyValuePair<string, string> item in dictionary)
				{
					if (errors.ContainsKey(item.Key))
					{
						errors[item.Key] = item.Value;
					}
					else
					{
						errors.Add(item.Key, item.Value);
					}
				}
			}
		}

		private void PrintErrors()
		{
			foreach (KeyValuePair<string, string> error in errors)
			{
				UnityEngine.Debug.Log("Error - Key: " + error.Key + " Value: " + error.Value);
			}
		}
	}
}
