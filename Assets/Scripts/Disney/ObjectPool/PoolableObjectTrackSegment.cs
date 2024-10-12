using UnityEngine;

namespace Disney.ObjectPool
{
	public class PoolableObjectTrackSegment : PoolableObject
	{
		public Transform endSocket;

		public Vector3 SegmentLength => endSocket.transform.position;
	}
}
