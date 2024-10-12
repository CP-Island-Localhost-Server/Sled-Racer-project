namespace Disney.ClubPenguin.CPModuleUtils
{
	public class InputFieldStringUtils
	{
		public static string ToTitleCase(string inputTextString)
		{
			string text = string.Empty;
			for (int i = 0; i < inputTextString.Length; i++)
			{
				text = ((i != 0 && (i <= 0 || inputTextString[i - 1] != ' ')) ? (text + inputTextString[i]) : (text + inputTextString[i].ToString().ToUpper()));
			}
			return text;
		}

		public static string StripQuoteSlashes(string input)
		{
			return input.Replace("\\\"", "\"");
		}
	}
}
