using UnityEngine;
using System.Collections;

public class CylinderLineGizmo : BaseTargetGizmo{
	public float radius = 1.0f;
	public int uSegments = 8;
	public int vSegments = 8;
	protected override void onDrawGizmo(){
		if (!target)
			return;
		GizmoTools.drawCylinderLine(transform.position, target.position, radius, uSegments, vSegments);
	}
}
