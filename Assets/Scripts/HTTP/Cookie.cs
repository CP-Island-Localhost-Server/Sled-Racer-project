using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace HTTP
{

	public class Cookie
	{
		public string name = null;
		public string value = null;
		public DateTime expirationDate = DateTime.MaxValue;
		public string path = null;
		public string domain = null;
		public bool secure = false;
		public bool scriptAccessible = true;

		private static string cookiePattern = "\\s*([^=]+)(?:=((?:.|\\n)*))?";

		public Cookie( string cookieString )
		{
			string[] parts = cookieString.Split( ';' );
			foreach ( string part in parts )
			{

				Match match = Regex.Match( part, cookiePattern );

				if ( !match.Success )
				{
					throw new Exception( "Could not parse cookie string: " + cookieString );
				}

				if ( this.name == null )
				{
					this.name = match.Groups[ 1 ].Value;
					this.value = match.Groups[ 2 ].Value;
					continue;
				}

				switch( match.Groups[ 1 ].Value.ToLower() )
				{
				case "httponly":
					this.scriptAccessible = false;
					break;
				case "expires":
					this.expirationDate = DateTime.Parse( match.Groups[ 2 ].Value );
					break;
				case "path":
					this.path = match.Groups[ 2 ].Value;
					break;
				case "domain":
					this.domain = match.Groups[ 2 ].Value;
					break;
				case "secure":
					this.secure = true;
					break;
				default:
					// TODO: warn of unknown cookie setting?
					break;
				}
			}
		}

		public bool Matches( CookieAccessInfo accessInfo )
		{
			if (    this.secure != accessInfo.secure
				|| !this.CollidesWith( accessInfo ) )
			{
				return false;
			}

			return true;
		}

		public bool CollidesWith( CookieAccessInfo accessInfo )
		{
			if ( ( this.path != null && accessInfo.path == null ) || ( this.domain != null && accessInfo.domain == null ) )
			{
				return false;
			}

			if ( this.path != null && accessInfo.path != null && accessInfo.path.IndexOf( this.path ) != 0 )
			{
				return false;
			}

			if ( this.domain == accessInfo.domain )
			{
				return true;
			}
			else if ( this.domain != null && this.domain.Length >= 1 && this.domain[ 0 ] == '.' )
			{
				int wildcard = accessInfo.domain.IndexOf( this.domain.Substring( 1 ) );
				if( wildcard == -1 || wildcard != accessInfo.domain.Length - this.domain.Length + 1 )
				{
					return false;
				}
			}
			else if ( this.domain != null )
			{
				return false;
			}

			return true;
		}

		public string ToValueString()
		{
			return this.name + "=" + this.value;
		}

		public override string ToString()
		{
			List< string > elements = new List< string >();
			elements.Add( this.name + "=" + this.value );

			if( this.expirationDate != DateTime.MaxValue )
			{
				elements.Add( "expires=" + this.expirationDate.ToString() );
			}

			if( this.domain != null )
			{
				elements.Add( "domain=" + this.domain );
			}

			if( this.path != null )
			{
				elements.Add( "path=" + this.path );
			}

			if( this.secure )
			{
				elements.Add( "secure" );
			}

			if( this.scriptAccessible == false )
			{
				elements.Add( "httponly" );
			}

			return String.Join( "; ", elements.ToArray() );
		}
	}
}
