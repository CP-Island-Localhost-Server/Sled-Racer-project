using LitJson;

namespace Disney.MobileNetwork
{
	public class Json
	{
		public static object parse(string jsonInput)
		{
			//Discarded unreachable code: IL_000c, IL_001e
			try
			{
				return JsonMapperSimple.ToObjectSimple(jsonInput);
			}
			catch (JsonException inner_exception)
			{
				throw new JsonParseException("Internal JSON parse error:", inner_exception);
			}
		}

		public static string serialize(object input)
		{
			//Discarded unreachable code: IL_000c, IL_0019
			try
			{
				return JsonMapper.ToJson(input);
			}
			catch (JsonException)
			{
				return null;
			}
		}
	}
}
