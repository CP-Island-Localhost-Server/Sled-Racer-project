using UnityEngine;

public class CameraBasePlate : MonoBehaviour
{
	private CameraBaseRig myCameraRig;

	private void Start()
	{
		myCameraRig = GetComponentInParent<CameraBaseRig>();
	}

	private void Update()
	{
	}
}
