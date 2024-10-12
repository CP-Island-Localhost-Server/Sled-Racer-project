using Disney.ClubPenguin.CPModuleUtils;
using UnityEngine;

public abstract class BaseMenuController : MonoBehaviour
{
	public Animator[] LoadingAnimations;

	public bool WasStarted
	{
		get;
		private set;
	}

	public BaseMenuController()
	{
		WasStarted = false;
	}

	public void Start()
	{
		WasStarted = true;
		VStart();
	}

	protected abstract void VStart();

	public void DoInit()
	{
		UnityEngine.Debug.Log("DoInit " + this.GetPath());
		if (!WasStarted)
		{
			UnityEngine.Debug.LogError("DoInit must be called after Start is called by the engine.");
		}
		else
		{
			Init();
		}
	}

	protected abstract void Init();

	public virtual void InitAnimations()
	{
		SetLoadingAnimationTrigger("Init");
	}

	protected void SetLoadingAnimationTrigger(string trigger)
	{
		Animator[] loadingAnimations = LoadingAnimations;
		foreach (Animator animator in loadingAnimations)
		{
			animator.SetTrigger(trigger);
		}
	}
}
