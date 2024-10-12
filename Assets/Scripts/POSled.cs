using Disney.ObjectPool;
using UnityEngine;

public class POSled : PoolableObject
{
	public Transform playerSeat;

	public Vector3 PlayerSeat => playerSeat.transform.position;

	public Vector3 LocalPlayerSeat => playerSeat.transform.localPosition;

	private void Start()
	{
	}
}
