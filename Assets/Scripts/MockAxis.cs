using Disney.Utility;
using UnityEngine;

public class MockAxis : Axis
{
	public override float GetValue(int energy)
	{
		float result = -1f;
		UnityEngine.Debug.Log("[MockAxis] GetValue(" + energy + ")");
		if (energy == 1)
		{
			result = 0.9f;
		}
		if (energy == -1)
		{
			result = -0.9f;
		}
		if (energy == 0)
		{
			result = 0f;
		}
		return result;
	}
}
