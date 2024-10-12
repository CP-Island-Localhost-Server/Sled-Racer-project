using Disney.ClubPenguin.SledRacer;

public class PlayerStateObjectWhoopeeCushion : PlayerStateObject
{
	public float AudioCutoffFrequency = 2200f;

	public override void EnterState()
	{
		base.EnterState();
		base.enabled = true;
		DevTrace("Entered WhoopeeCushion State (Object)");
		player.TriggerAnimation("RiderWhoopee");
		if (stateAnimator != null)
		{
			stateAnimator.SetTrigger("RiderWhoopee");
		}
	}

	protected override void ExitState()
	{
		base.ExitState();
	}

	private void AnimComplete()
	{
		ExitState();
	}

	public override void AbortState()
	{
		base.AbortState();
		base.enabled = false;
	}
}
