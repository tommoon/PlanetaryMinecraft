using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityAttractor : MonoBehaviour {
	public float gravity = -10;

	private Vector3[] m_GravityDirections =
	{
		Vector3.up,
		Vector3.down,
		Vector3.left,
		Vector3.right,
		Vector3.forward,
		Vector3.back
	};

	public void Attract(Transform body){
		Vector3 question = GravityDirection (transform, body);

		body.gameObject.GetComponent<Rigidbody> ().AddForce (question * gravity,ForceMode.Acceleration);

		body.rotation = Quaternion.FromToRotation (body.up, question) * body.rotation;
	}


	private Vector3 GravityDirection(Transform planet, Transform other)
	{
		float bestDot = -1.0f;
		Vector3 direction = Vector3.zero;

		for (int index=0; index < m_GravityDirections.Length; index++)
		{
			Vector3 gravityDirection = planet.TransformVector(m_GravityDirections[index]).normalized;
			Vector3 directionToOther = (other.position - planet.position).normalized;

			float dot = Vector3.Dot(gravityDirection, directionToOther);

			if (dot > bestDot)
			{
				direction = gravityDirection;
				bestDot = dot;
			}
		}

		return direction;
	}
}
