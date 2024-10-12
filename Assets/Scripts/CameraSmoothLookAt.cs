using UnityEngine;

[AddComponentMenu("Camera-Control/Smooth Look At")]
public class CameraSmoothLookAt : CameraBaseRig
{
	public Transform defaultTarget;

	private Transform currTarget;

	private Transform lastTarget;

	private bool targetHasChanged;

	private Vector3 targetOrigin;

	public bool smoothLockOn = true;

	public float lockOnSpeed = 1f;

	public bool smoothFollow = true;

	public float followSpeed = 5f;

	private float elapsed;

	private Quaternion WantedRotation;

	private Quaternion PreLookRotation;

	private Transform WantedTransform;

	private Vector3 PreLookPosition;

	public Transform target
	{
		get
		{
			return currTarget;
		}
		set
		{
			if (currTarget != value)
			{
				currTarget = value;
			}
			if (lastTarget != currTarget)
			{
				PreLookPosition = base.transform.position;
				PreLookRotation = base.transform.rotation;
				targetOrigin = currTarget.position;
				targetHasChanged = true;
			}
		}
	}

	protected override void OnAwake()
	{
		base.OnAwake();
		target = defaultTarget;
	}

	private void LateUpdate()
	{
		CheckForChanges();
		if (!currTarget)
		{
			return;
		}
		WantedRotation = Quaternion.LookRotation(currTarget.position - base.transform.position);
		if (targetHasChanged || mountHasChanged)
		{
			UnityEngine.Debug.Log("[CameraSmoothLook] " + ((!targetHasChanged) ? "MOUNT" : "TARGET") + " has changed");
			WantedRotation = Quaternion.LookRotation(targetOrigin - PreLookPosition);
			targetHasChanged = false;
			mountHasChanged = false;
			elapsed = 0f;
		}
		float deltaTime = Time.deltaTime;
		WantedRotation.y = 0f;
		if (smoothLockOn && elapsed < 1f)
		{
			elapsed += deltaTime / lockOnSpeed;
			base.transform.rotation = Quaternion.Slerp(PreLookRotation, WantedRotation, elapsed);
			return;
		}
		lastTarget = currTarget;
		if (smoothFollow)
		{
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, WantedRotation, followSpeed * deltaTime);
		}
		else
		{
			base.transform.rotation = WantedRotation;
		}
	}

	private void CheckForChanges()
	{
		if (defaultMountPoint != currMount)
		{
			base.MountPoint = defaultMountPoint;
		}
		if (defaultTarget != currTarget)
		{
			target = defaultTarget;
		}
	}
}
