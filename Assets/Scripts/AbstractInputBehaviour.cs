using UnityEngine;

public abstract class AbstractInputBehaviour : MonoBehaviour
{
	internal abstract bool jump();

	internal abstract bool right();

	internal abstract bool left();
}
