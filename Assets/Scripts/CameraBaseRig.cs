using System;
using System.Reflection;
using UnityEngine;

public class CameraBaseRig : MonoBehaviour
{
	public enum CameraMountMode
	{
		Snap,
		Ease
	}

	public delegate void BasePlateChangeHandler(Transform _newBasePlate);

	public Transform defaultMountPoint;

	public CameraMountMode remountMode;

	public float remountTime;

	public Transform defaultBasePlate;

	public bool useVirtualBasePlate;

	public Transform[] AvailableBasePlates;

	protected Transform currMount;

	protected Transform prevMount;

	protected Transform basePlate;

	protected GameObject virtualBasePlate;

	protected CameraBaseRig camRig;

	protected bool mountHasChanged;

	protected bool baseHasChanged;

	protected Vector3 startPosition;

	protected Quaternion startRotation;

	protected Vector3 destPosition;

	protected Quaternion destRotation;

	protected float easeTime;

	public Transform MountPoint
	{
		get
		{
			return currMount;
		}
		set
		{
			if (!(currMount != value))
			{
				return;
			}
			prevMount = currMount;
			if (camRig != null && camRig.OnBasePlateChange != null)
			{
				CameraBaseRig cameraBaseRig = camRig;
				cameraBaseRig.OnBasePlateChange = (BasePlateChangeHandler)Delegate.Remove(cameraBaseRig.OnBasePlateChange, new BasePlateChangeHandler(HandleOnBasePlateChange));
			}
			camRig = value.GetComponent<CameraBaseRig>();
			if (camRig != null)
			{
				base.transform.parent = camRig.BasePlate;
				currMount = camRig.BasePlate;
			}
			else
			{
				camRig = value.transform.parent.GetComponent<CameraBaseRig>();
				base.transform.parent = value;
				currMount = value;
			}
			if (camRig != null)
			{
				CameraBaseRig cameraBaseRig2 = camRig;
				cameraBaseRig2.OnBasePlateChange = (BasePlateChangeHandler)Delegate.Combine(cameraBaseRig2.OnBasePlateChange, new BasePlateChangeHandler(HandleOnBasePlateChange));
			}
			switch (remountMode)
			{
			case CameraMountMode.Snap:
				lockToMount();
				break;
			case CameraMountMode.Ease:
				if (prevMount != null)
				{
					if (base.transform.position != currMount.transform.position || base.transform.rotation != currMount.transform.rotation)
					{
						startPosition = base.transform.position;
						startRotation = base.transform.rotation;
						base.transform.parent = currMount.transform;
						easeTime = remountTime;
					}
				}
				else
				{
					lockToMount();
				}
				break;
			}
			if (currMount != defaultMountPoint)
			{
				defaultMountPoint = currMount;
			}
			mountHasChanged = true;
		}
	}

	public Transform BasePlate
	{
		get
		{
			if (basePlate == null)
			{
				if (defaultBasePlate != null)
				{
					basePlate = defaultBasePlate;
				}
				else if (AvailableBasePlates.Length > 0)
				{
					basePlate = AvailableBasePlates[0];
				}
				else if (useVirtualBasePlate)
				{
					virtualBasePlate = new GameObject();
					virtualBasePlate.name = "BasePlate(virtual)";
					virtualBasePlate.transform.position = base.transform.position;
					virtualBasePlate.transform.rotation = base.transform.rotation;
					virtualBasePlate.transform.parent = base.transform;
					basePlate = virtualBasePlate.transform;
				}
				else
				{
					basePlate = base.transform;
				}
				defaultBasePlate = basePlate;
			}
			return basePlate;
		}
		set
		{
			if (basePlate != value)
			{
				if ((bool)virtualBasePlate && basePlate == virtualBasePlate.transform && value != virtualBasePlate.transform)
				{
					UnityEngine.Object.Destroy(virtualBasePlate);
				}
				basePlate = value;
				if (basePlate != defaultBasePlate)
				{
					defaultBasePlate = basePlate;
				}
				dispatchBasePlateChange();
				baseHasChanged = true;
			}
		}
	}

	public event BasePlateChangeHandler OnBasePlateChange;

	public void changeBasePlate(int _index)
	{
		if (AvailableBasePlates.Length >= _index + 1)
		{
			BasePlate = AvailableBasePlates[_index];
		}
	}

	protected void lockToMount()
	{
		base.transform.parent = currMount.transform;
		base.transform.localPosition = Vector3.zero;
		base.transform.localRotation = Quaternion.identity;
	}

	protected void dispatchBasePlateChange()
	{
		if (this.OnBasePlateChange != null)
		{
			this.OnBasePlateChange(basePlate);
		}
	}

	private void HandleOnBasePlateChange(Transform _newBasePlate)
	{
		CameraBaseRig cameraBaseRig = camRig;
		cameraBaseRig.OnBasePlateChange = (BasePlateChangeHandler)Delegate.Remove(cameraBaseRig.OnBasePlateChange, new BasePlateChangeHandler(HandleOnBasePlateChange));
		MountPoint = _newBasePlate.parent;
	}

	private void Awake()
	{
		OnAwake();
	}

	protected virtual void OnAwake()
	{
		MountRig();
		IndexBasePlates();
	}

	private void Update()
	{
		OnUpdate();
	}

	private void LateUpdate()
	{
		OnLateUpdate();
	}

	protected virtual void OnUpdate()
	{
		if (basePlate != defaultBasePlate)
		{
			BasePlate = defaultBasePlate;
		}
	}

	protected virtual void OnLateUpdate()
	{
		if (easeTime > 0f)
		{
			easeTime -= Time.deltaTime;
			startPosition = prevMount.transform.position;
			startRotation = prevMount.transform.rotation;
			destPosition = currMount.transform.position;
			destRotation = currMount.transform.rotation;
			Vector3 position = Vector3.Lerp(startPosition, destPosition, Mathf.Abs(easeTime - remountTime) / remountTime);
			Quaternion rotation = Quaternion.Slerp(startRotation, destRotation, Mathf.Abs(easeTime - remountTime));
			base.transform.position = position;
			base.transform.rotation = rotation;
			if (easeTime <= 0f)
			{
				lockToMount();
			}
		}
	}

	protected void MountRig()
	{
		Transform parent = base.transform.parent;
		if (!defaultMountPoint)
		{
			MountPoint = parent;
		}
		else
		{
			MountPoint = defaultMountPoint;
		}
	}

	protected void IndexBasePlates()
	{
		if (BasePlate == null)
		{
			UnityEngine.Debug.LogWarning("No baseplate established");
		}
	}

	public virtual void changeBase(Vector3 _newPlateLocation)
	{
	}

	protected void DevTrace(string _msg)
	{
		UnityEngine.Debug.Log("[" + MethodBase.GetCurrentMethod().ReflectedType.Name + "] " + _msg);
	}
}
