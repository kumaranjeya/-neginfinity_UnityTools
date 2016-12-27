using UnityEngine;
using System.Collections;

[System.Serializable]
public class ViewAreaGizmo: BaseGizmo{
	public float hFovDegrees = 90.0f;
	public float vFovDegrees = 60.0f;
	public float range = 5.0f;
	public float degreesPerSegment = 10.0f;
	protected override void onDrawGizmo(){
		GizmoTools.drawVisionCone(transform, hFovDegrees, vFovDegrees, range, degreesPerSegment);
	}
}
