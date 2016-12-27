using UnityEngine;
using System.Collections;

public class AdaptiveConeLineGizmo: BaseTargetGizmo{
	public float radius = 1.0f;
	public int uSegments = 8;
	public int vSegments = 8;
	public bool reverse = false;
	protected override void onDrawGizmo(){
		if (!target)
			return;
		var start = reverse ? transform.position: target.position;
		var end = reverse ? target.position: transform.position;
		GizmoTools.drawConeLine(start, end, radius, uSegments, vSegments);
	}
}
