using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MathExtensions;

public class DualQuaternionDebug: MonoBehaviour{

	void drawDualQuaternion(DualQuaternion q){
		Vector3 x = Vector3.right * 5.0f;
		Vector3 y = Vector3.up * 5.0f;
		Vector3 z = Vector3.forward * 5.0f;
		Vector3 p = Vector3.zero;

		var x1 = q.transformPoint(x);
		var y1 = q.transformPoint(y);
		var z1 = q.transformPoint(z);
		var p1 = q.transformPoint(p);

		Gizmos.DrawLine(p1, x1);
		Gizmos.DrawLine(p1, y1);
		Gizmos.DrawLine(p1, z1);
	}
	void drawGizmos(Color color){
		var oldColor = Gizmos.color;
		Gizmos.color = color;

		var pos = transform.position;
		var rot = transform.rotation;

		var q = DualQuaternion.fromUnityTransform(rot, pos);//new DualQuaternion(rot.conjugate(), pos);//o_O Well, whatever.
		drawDualQuaternion(q);

		foreach(Transform cur in transform){
			var childQ = DualQuaternion.fromUnityTransform(cur.transform.localRotation, cur.transform.localPosition);
			//new DualQuaternion(cur.transform.localRotation.conjugate(), cur.transform.localPosition);

			var childQ2 = q * childQ;

			drawDualQuaternion(childQ2);

			const int numSteps = 10;
			for (int i = 1; i < numSteps; i++){
				float t = (float)i/(float)numSteps;
				var tmpQ = q.sclerp(childQ2, t);
				drawDualQuaternion(tmpQ);
			}
		}

		Gizmos.color = oldColor;

	}
	void OnDrawGizmos(){
		drawGizmos(Color.blue);
	}

	void OnDrawGizmosSeleccted(){
		drawGizmos(Color.white);
	}
}
