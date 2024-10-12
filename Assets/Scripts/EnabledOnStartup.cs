using UnityEngine;

public class EnabledOnStartup : MonoBehaviour
{
	public MonoBehaviour obj;

	public bool enabledOnStartUp;

	private void Start()
	{
		IntegrationTest.Assert(obj.enabled == enabledOnStartUp);
	}
}
