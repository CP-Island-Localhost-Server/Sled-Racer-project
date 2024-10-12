using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace HTTP
{
	public class CookieJar
	{
		private static string version = "v2";
		private object cookieJarLock = new object();

		private static CookieJar instance;
		public Dictionary< string, List< Cookie > > cookies;

		public ContentsChangedDelegate ContentsChanged;

		public static CookieJar Instance
		{
			get
			{
				if ( instance == null )
				{
					instance = new CookieJar();
				}
				return instance;
			}
		}

		public CookieJar ()
		{
			this.Clear();
		}

		public void Clear()
		{
			lock( cookieJarLock )
			{
				cookies = new Dictionary< string, List< Cookie > >();
				if ( ContentsChanged != null )
				{
					ContentsChanged();
				}
			}
		}

		public bool SetCookie( Cookie cookie )
		{
			lock( cookieJarLock )
			{
				bool expired = cookie.expirationDate < DateTime.Now;

				if ( cookies.ContainsKey( cookie.name ) )
				{
					for( int index = 0; index < cookies[ cookie.name ].Count; ++index )
					{
						Cookie collidableCookie = cookies[ cookie.name ][ index ];
						if ( collidableCookie.CollidesWith( new CookieAccessInfo( cookie ) ) )
						{
							if( expired )
							{
								cookies[ cookie.name ].RemoveAt( index );
								if ( cookies[ cookie.name ].Count == 0 )
								{
									cookies.Remove( cookie.name );
									if ( ContentsChanged != null )
									{
										ContentsChanged();
									}
								}

								return false;
							}
							else
							{
								cookies[ cookie.name ][ index ] = cookie;
								if ( ContentsChanged != null )
								{
									ContentsChanged();
								}
								return true;
							}
						}
					}

					if ( expired )
					{
						return false;
					}

					cookies[ cookie.name ].Add( cookie );
					if ( ContentsChanged != null )
					{
						ContentsChanged();
					}
					return true;
				}

				if ( expired )
				{
					return false;
				}

				cookies[ cookie.name ] = new List< Cookie >();
				cookies[ cookie.name ].Add( cookie );
				if ( ContentsChanged != null )
				{
					ContentsChanged();
				}
				return true;
			}
		}

		// TODO: figure out a way to respect the scriptAccessible flag and supress cookies being
		//       returned that should not be.  The issue is that at some point, within this
		//       library, we need to send all the correct cookies back in the request.  Right now
		//       there's no way to add all cookies (regardless of script accessibility) to the
		//       request without exposing cookies that should not be script accessible.

		public Cookie GetCookie( string name, CookieAccessInfo accessInfo )
		{
			if ( !cookies.ContainsKey( name ) )
			{
				return null;
			}

			for ( int index = 0; index < cookies[ name ].Count; ++index )
			{
				Cookie cookie = cookies[ name ][ index ];
				if ( cookie.expirationDate > DateTime.Now && cookie.Matches( accessInfo ) )
				{
					return cookie;
				}
			}

			return null;
		}

		public List< Cookie > GetCookies( CookieAccessInfo accessInfo )
		{
			List< Cookie > result = new List< Cookie >();
			foreach ( string cookieName in cookies.Keys )
			{
				Cookie cookie = this.GetCookie( cookieName, accessInfo );
				if ( cookie != null )
				{
					result.Add( cookie );
				}
			}

			return result;
		}

		public void SetCookies( Cookie[] cookieObjects )
		{
			for ( var index = 0; index < cookieObjects.Length; ++index )
			{
				this.SetCookie( cookieObjects[ index ] );
			}
		}

		private static string cookiesStringPattern = "[:](?=\\s*[a-zA-Z0-9_\\-]+\\s*[=])";

		public void SetCookies( string cookiesString )
		{

			Match match = Regex.Match( cookiesString, cookiesStringPattern );

			if ( !match.Success )
			{
				throw new Exception( "Could not parse cookies string: " + cookiesString );
			}

			for ( int index = 0; index < match.Groups.Count; ++index )
			{
				this.SetCookie( new Cookie( match.Groups[ index ].Value ) );
			}
		}

		private static string boundary = "\n!!::!!\n";

		public string Serialize()
		{
			string result = version + boundary;

			lock( cookieJarLock )
			{
				foreach ( string key in cookies.Keys )
				{
					for ( int index = 0; index < cookies[ key ].Count; ++index )
					{
						result += cookies[ key ][ index ].ToString() + boundary;
					}
				}
			}

			return result;
		}

		public void Deserialize( string cookieJarString, bool clear )
		{
			if ( clear )
			{
				this.Clear();
			}

			Regex regex = new Regex( boundary );
			string[] cookieStrings = regex.Split( cookieJarString );
			bool readVersion = false;
			foreach ( string cookieString in cookieStrings )
			{
				if ( !readVersion )
				{
					if ( cookieString.IndexOf( version ) != 0 )
					{
						return;
					}
					readVersion = true;
					continue;
				}

				if ( cookieString.Length > 0 )
				{
					this.SetCookie( new Cookie( cookieString ) );
				}
			}
		}
	}
}
