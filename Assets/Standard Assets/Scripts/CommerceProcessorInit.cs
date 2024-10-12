using UnityEngine;

[RequireComponent(typeof(CommerceProcessor))]
public class CommerceProcessorInit : MonoBehaviour
{
	public static CommerceProcessor commerceProcessor;

	public CommerceProcessor GetCommerceProcessor(int testMode = 0, bool forceMock = false)
	{
		if (forceMock)
		{
			commerceProcessor = GetComponent<CommerceProcessorMock>();
			commerceProcessor.SetTestMode(testMode);
			return commerceProcessor;
		}
		switch (Application.platform)
		{
		case RuntimePlatform.Android:
			commerceProcessor = GetComponent<CommerceProcessorGooglePlay>();
			break;
		case RuntimePlatform.WindowsEditor:
			commerceProcessor = GetComponent<CommerceProcessorMock>();
			commerceProcessor.SetTestMode(testMode);
			break;
		case RuntimePlatform.OSXEditor:
			commerceProcessor = GetComponent<CommerceProcessorMock>();
			commerceProcessor.SetTestMode(testMode);
			break;
		case RuntimePlatform.IPhonePlayer:
			commerceProcessor = GetComponent<CommerceProcessorApple>();
			break;
		default:
			UnityEngine.Debug.LogWarning("Application Platform " + Application.platform + " does not have a processor specified");
			break;
		}
		if (commerceProcessor == null)
		{
			commerceProcessor = GetComponent<CommerceProcessorMock>();
			commerceProcessor.SetTestMode(testMode);
		}
		return commerceProcessor;
	}
}
