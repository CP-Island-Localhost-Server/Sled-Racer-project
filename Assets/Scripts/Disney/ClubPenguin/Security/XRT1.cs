using System;
using System.Security.Cryptography;
using System.Text;

namespace Disney.ClubPenguin.Security
{
	public class XRT1
	{
		private static string requestTokenSalt = "bIMDmOdjg7bmKC1V4eKx";

		public static string GetRequestTokenHeader()
		{
			return "X-Request-Token";
		}

		public static string GetRequestToken(string url, string body, long timestamp)
		{
			string text = (body == null || body.Length <= 0) ? url : (url + " " + body);
			text = requestTokenSalt + ":" + timestamp + ":" + text;
			return sha512Hex(text);
		}

		private static string sha512Hex(string unhashedString)
		{
			byte[] bytes = Encoding.ASCII.GetBytes(unhashedString);
			SHA512 sHA = new SHA512Managed();
			byte[] value = sHA.ComputeHash(bytes);
			string text = BitConverter.ToString(value);
			return text.Replace("-", string.Empty).ToLower();
		}
	}
}
