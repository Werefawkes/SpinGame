using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CustomInspector;
public class ProjectileOrbiter : Projectile
{
	[SelfFill(true)]
	public DistanceJoint2D distanceJoint;
	public override void Start()
	{
		base.Start();

		distanceJoint.connectedBody = owner.Rigidbody;
		distanceJoint.distance = data.orbitDistance;
		distanceJoint.maxDistanceOnly = data.orbitMaxOnly;
	}

	public override void Update()
	{
		base.Update();

		//Vector2 dir = transform.position - owner.ProjectileOrigin;
		//rb.velocity = Vector2.Perpendicular(dir) * data.speed;
	}
}
