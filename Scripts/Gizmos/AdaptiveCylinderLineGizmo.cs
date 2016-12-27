using UnityEngine;
using System.Collections;

public class AdaptiveCylinderLineGizmo : BaseTargetGizmo{
	public float radius = 1.0f;
	public int uSegments = 8;
	public float vSize = 1.0f;
	protected override void onDrawGizmo(){
		if (!target)
			return;
		GizmoTools.drawCylinderLineAdaptive(transform.position, target.position, radius, uSegments, vSize);
	}
}
