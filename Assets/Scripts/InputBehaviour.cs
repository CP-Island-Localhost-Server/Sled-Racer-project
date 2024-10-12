using System.Collections.Generic;
using UnityEngine;

public class InputBehaviour : AbstractInputBehaviour
{
	private InputManager manager;

	public bool UseMouse;

	public bool applicationIsOnPC
	{
		private get;
		set;
	}

	private void Awake()
	{
		manager = new InputManager();
		CalcApplicationIsOnPC();
	}

	private void CalcApplicationIsOnPC()
	{
		bool flag = Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer;
		bool flag2 = Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.OSXPlayer;
		applicationIsOnPC = (flag || flag2);
	}

	public void setInputManager(InputManager inputManager)
	{
		manager = inputManager;
	}

	internal override bool jump()
	{
		if (applicationIsOnPC)
		{
			return manager.jump(UnityEngine.Input.GetAxis("Vertical"));
		}
		return manager.jump(getTouchPositions());
	}

	internal override bool right()
	{
		if (applicationIsOnPC)
		{
			if (UseMouse)
			{
				if (Input.GetMouseButton(0))
				{
					return manager.right(UnityEngine.Input.mousePosition);
				}
				return false;
			}
			return manager.right(UnityEngine.Input.GetAxis("Horizontal"));
		}
		return manager.right(getTouchPositions());
	}

	internal override bool left()
	{
		if (applicationIsOnPC)
		{
			if (UseMouse)
			{
				if (Input.GetMouseButton(0))
				{
					return manager.left(UnityEngine.Input.mousePosition);
				}
				return false;
			}
			return manager.left(UnityEngine.Input.GetAxis("Horizontal"));
		}
		return manager.left(getTouchPositions());
	}

	private Vector2[] getTouchPositions()
	{
		List<Vector2> list = new List<Vector2>();
		if ((float)Input.touches.Length > 0f)
		{
			Touch[] touches = Input.touches;
			for (int i = 0; i < touches.Length; i++)
			{
				Touch touch = touches[i];
				if (touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
				{
					list.Add(touch.position);
				}
			}
		}
		return list.ToArray();
	}
}
