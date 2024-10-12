using System;
using System.Runtime.InteropServices;

namespace Disney.ClubPenguin.Security
{
	public class XRT2
	{
		private static bool initialized;

		[DllImport("xrt2")]
		private static extern void XRT2CPSecurityInit();

		public static void CPSecurityInit()
		{
			if (!initialized)
			{
				XRT2CPSecurityInit();
				initialized = true;
			}
		}

		[DllImport("xrt2")]
		private static extern IntPtr XRT2GetRequestTokenHeader();

		public static string GetRequestTokenHeader()
		{
			return Marshal.PtrToStringAuto(XRT2GetRequestTokenHeader());
		}

		[DllImport("xrt2")]
		private static extern IntPtr XRT2GetRequestToken(string urlSansCellophane, string body, string time);

		public static string GetRequestToken(string urlSansCellophane, string body, string time)
		{
			CPSecurityInit();
			if (body == null)
			{
				body = string.Empty;
			}
			return Marshal.PtrToStringAuto(XRT2GetRequestToken(urlSansCellophane, body, time));
		}
	}
}
