using UnityEngine;

public class BackAndForthMovement : MonoBehaviour
{
	private static float DEFAULT_ROTATION;

	private static float FLIPPED_ROTATION = 180f;

	public float movementSpeed = 0.15f;

	[Range(-5f, 5f)]
	public float movementOffset = -3f;

	public float distanceThreshold = 0.5f;

	private Vector3 goalPosition;

	private Vector3 startPosition;

	private bool facingGoalPosition;

	private void Start()
	{
		startPosition = base.transform.localPosition;
		goalPosition = startPosition;
		goalPosition.x -= movementOffset;
		facingGoalPosition = true;
	}

	private void Update()
	{
		if (facingGoalPosition)
		{
			base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, goalPosition, movementSpeed * Time.deltaTime);
			if (isCloseToGoal(goalPosition))
			{
				facingGoalPosition = false;
				base.transform.rotation = Quaternion.AngleAxis(FLIPPED_ROTATION, Vector3.up);
			}
		}
		else
		{
			base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, startPosition, movementSpeed * Time.deltaTime);
			if (isCloseToGoal(startPosition))
			{
				facingGoalPosition = true;
				base.transform.rotation = Quaternion.AngleAxis(DEFAULT_ROTATION, Vector3.up);
			}
		}
	}

	private bool isCloseToGoal(Vector3 goal)
	{
		return Vector3.Distance(base.transform.localPosition, goal) < distanceThreshold;
	}
}
