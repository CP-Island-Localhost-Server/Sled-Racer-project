using Fabric;
using UnityEngine;

public class ExamplesAPI : MonoBehaviour
{
	public int eventID;

	private void Start()
	{
	}

	private void Update()
	{
		if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha1))
		{
			EventManager.Instance.PostEvent("Simple");
		}
		else if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha2))
		{
			EventManager.Instance.PostEvent("Simple", base.gameObject);
		}
		else if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha3))
		{
			EventManager.Instance.PostEvent("Simple", EventAction.PlaySound, null, base.gameObject);
		}
		else if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha4))
		{
			EventManager.Instance.PostEvent("Simple", EventAction.StopSound, null, base.gameObject);
		}
		else if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha5))
		{
			EventManager.Instance.SetParameter("Timeline", "Parameter", 0.5f, base.gameObject);
		}
		else if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha6))
		{
			EventManager.Instance.SetDSPParameter("Event", DSPType.LowPass, "CutoffFrequency", 5000f, 5f, 0.5f, base.gameObject);
		}
		else if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha7))
		{
			EventManager.Instance.PostEvent("DynamicMixer", EventAction.AddPreset, "MuteAll", null);
		}
		else if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha8))
		{
			EventManager.Instance.PostEvent("DynamicMixer", EventAction.RemovePreset, "MuteAll", null);
		}
		else if (UnityEngine.Input.GetKeyDown(KeyCode.Alpha9))
		{
			Fabric.Component componentByName = FabricManager.Instance.GetComponentByName("Audio_Fabric_SFX_Test");
			if (componentByName != null)
			{
				componentByName.Volume = 0.5f;
				if (!componentByName.IsPlaying())
				{
					componentByName.Play(base.gameObject);
				}
			}
		}
		else if (UnityEngine.Input.GetKeyDown(KeyCode.A))
		{
			FabricManager.Instance.LoadAsset("NameOfPrefab", "Audio_SFX");
		}
		else if (UnityEngine.Input.GetKeyDown(KeyCode.B))
		{
			FabricManager.Instance.UnloadAsset("Audio_SFX");
		}
		else if (UnityEngine.Input.GetKeyDown(KeyCode.C))
		{
			InitialiseParameters initialiseParameters = new InitialiseParameters();
			initialiseParameters._pitch.Value = 2f;
			EventManager.Instance.PostEvent("Simple", EventAction.PlaySound, null, base.gameObject, initialiseParameters);
		}
		else if (UnityEngine.Input.GetKeyDown(KeyCode.D))
		{
			if (EventManager.Instance.IsEventActive("Simple", base.gameObject))
			{
				UnityEngine.Debug.Log("Event Simple is Active");
			}
			else
			{
				UnityEngine.Debug.Log("Event Simple is Inactive");
			}
		}
		else if (UnityEngine.Input.GetKeyDown(KeyCode.E))
		{
			Fabric.Component[] componentsByName = FabricManager.Instance.GetComponentsByName("Audio_Simple", base.gameObject);
			if (componentsByName != null && componentsByName.Length > 0)
			{
				componentsByName[0].Volume = 0.5f;
				if (componentsByName[0].IsPlaying())
				{
					UnityEngine.Debug.Log("Component is playing");
				}
			}
		}
		if (UnityEngine.Input.GetKeyDown(KeyCode.F))
		{
			eventID = EventManager.GetIDFromEventName("Simple");
			EventManager.Instance.PostEvent(eventID, base.gameObject);
		}
	}
}
