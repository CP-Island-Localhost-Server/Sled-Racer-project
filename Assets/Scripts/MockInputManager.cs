public class MockInputManager : AbstractInputBehaviour
{
	public bool mockJump;

	public bool mockRight;

	public bool mockLeft;

	internal override bool jump()
	{
		return mockJump;
	}

	internal override bool right()
	{
		return mockRight;
	}

	internal override bool left()
	{
		return mockLeft;
	}
}
