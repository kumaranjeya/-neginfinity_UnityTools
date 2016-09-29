using UnityEngine;
using System.Collections;

[System.Serializable]
public class WeaponConeGizmo: BaseGizmo{
	public float angleDegrees = 90.0f;
	public float range = 5.0f;
	public float degreesPerSegment = 10.0f;
	public int numRadialSegments = 8;
	protected override void onDrawGizmo(){
		GizmoTools.drawWeaponCone(transform, angleDegrees, range, degreesPerSegment, numRadialSegments);
	}
}
