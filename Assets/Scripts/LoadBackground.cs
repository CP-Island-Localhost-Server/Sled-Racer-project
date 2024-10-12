using UnityEngine;

public class LoadBackground : MonoBehaviour
{
	public GameObject defaultBackground;

	public Transform backgroundContainer;

	public Camera backgroundCamera;

	private GameObject backgroundInstance;

	public void UpdateBackground(string assetString)
	{
		LoadBackgroundFromResources(assetString);
	}

	private void LoadBackgroundFromResources(string backgroundPath)
	{
		GameObject gameObject = Resources.Load(backgroundPath) as GameObject;
		if (gameObject != null)
		{
			CreateBackground(gameObject);
		}
		else
		{
			CreateBackground(defaultBackground);
		}
		BackgroundCameraMovement component = GetComponent<BackgroundCameraMovement>();
		if (component != null)
		{
			component.ResetPostion();
		}
	}

	private void CreateBackground(GameObject artAssetGameObject)
	{
		if (backgroundInstance != null)
		{
			UnityEngine.Object.Destroy(backgroundInstance);
			backgroundInstance = null;
		}
		backgroundInstance = (UnityEngine.Object.Instantiate(artAssetGameObject) as GameObject);
		backgroundInstance.transform.SetParent(backgroundContainer, worldPositionStays: false);
		if (backgroundCamera == null)
		{
			UnityEngine.Debug.LogError("Missing background Camera.");
			return;
		}
		BackgroundColor component = backgroundInstance.GetComponent<BackgroundColor>();
		if (component != null)
		{
			backgroundCamera.backgroundColor = component.backgroundColor;
		}
		else
		{
			UnityEngine.Debug.Log("Background did not have a background color.");
		}
	}
}
