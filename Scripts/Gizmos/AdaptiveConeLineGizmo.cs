using UnityEngine;
using System.Collections;

public class ConeLineGizmo : BaseTargetGizmo{
	public float radius = 1.0f;
	public int uSegments = 8;
	public float vSize = 1.0f;
	public bool reverse = false;
	protected override void onDrawGizmo(){
		if (!target)
			return;
		var start = reverse ? transform.position: target.position;
		var end = reverse ? target.position: transform.position;
		GizmoTools.drawConeLineAdaptive(start, end, radius, uSegments, vSize);
	}
}
