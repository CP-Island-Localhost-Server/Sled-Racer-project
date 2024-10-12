using Disney.ClubPenguin.SledRacer;
using Disney.Utility;

public class CameraSpringRail : CameraBaseRig
{
	private PlayerController SpringInfluence;

	private Axis Xaxis;

	private Axis Yaxis;

	private Axis Zaxis;

	private ConfigController config;

	protected override void OnAwake()
	{
		base.OnAwake();
		config = Service.Get<ConfigController>();
		startPosition = base.transform.localPosition;
		Xaxis = base.gameObject.AddComponent<Axis>();
		Yaxis = base.gameObject.AddComponent<Axis>();
		Zaxis = base.gameObject.AddComponent<Axis>();
		SpringInfluence = GetComponentInParent<PlayerController>();
	}

	protected override void OnUpdate()
	{
		base.OnUpdate();
	}
}
