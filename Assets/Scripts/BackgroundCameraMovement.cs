using Disney.ClubPenguin.SledRacer;
using UnityEngine;

public class BackgroundCameraMovement : MonoBehaviour
{
	public Camera backgroundCamera;

	private Vector3 cameraPositionTarget;

	public Transform CameraToFollow;

	private ConfigController config;

	private Vector3 previousFollowPos;

	private void Start()
	{
		config = Service.Get<ConfigController>();
		Service.Set(this);
		GetComponent<LoadBackground>().UpdateBackground(string.Empty);
	}

	private void LateUpdate()
	{
		Vector3 position = CameraToFollow.transform.position;
		float cameraHorizontal = position.y - previousFollowPos.y;
		SetCameraHorizontal(cameraHorizontal);
		previousFollowPos = CameraToFollow.transform.position;
		Vector3 localPosition = backgroundCamera.transform.localPosition;
		float num = Vector3.Distance(localPosition, cameraPositionTarget);
		if (num > config.BackgroundCameraDistanceCheck)
		{
			if (localPosition.y > cameraPositionTarget.y)
			{
				localPosition.y -= config.BackgroundCameraSpeed * Time.deltaTime;
			}
			else
			{
				localPosition.y += config.BackgroundCameraSpeed * Time.deltaTime;
			}
		}
		localPosition.y = Mathf.Clamp(localPosition.y, config.BackgroundCameraMinY, config.BackgroundCameraMaxY);
		backgroundCamera.transform.localPosition = localPosition;
	}

	private void SetCameraHorizontal(float value)
	{
		float value2 = value / config.BackgroundCameraMovementDampening;
		value2 = Mathf.Clamp(value2, config.BackgroundCameraMinHorizontal, config.BackgroundCameraMaxHorizontal);
		AdjustCameraHorizontal(value2);
	}

	private void AdjustCameraHorizontal(float value)
	{
		Vector3 localPosition = backgroundCamera.transform.localPosition;
		localPosition.y += value;
		backgroundCamera.transform.localPosition = localPosition;
	}

	public void ResetPostion()
	{
		cameraPositionTarget = backgroundCamera.transform.localPosition;
		cameraPositionTarget.y = config.BackgroundCameraTargetY;
		Vector3 localPosition = cameraPositionTarget;
		localPosition.y = config.BackgroundCameraStartY;
		backgroundCamera.transform.localPosition = localPosition;
		previousFollowPos = CameraToFollow.position;
	}
}
