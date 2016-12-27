using UnityEngine;
using System.Collections;

public class DebugComponent : MonoBehaviour {
	public Color color = ColorNames.Aqua.toColor();
	public float weaponConeRange = 5.0f;
	public float weaponConeSpreadAngle = 90.0f;
	public int weaponConeRadialSegments = 8;
	public float weaponConeDegreesPerSegment = 5.0f;

	public float viewConeRange = 5.0f;
	public float viewConeHFov = 90.0f;
	public float viewConeVFov = 60.0f;
	public float viewConeDegreesPerSegment = 5.0f;

	public Transform targetObject;

	[TextArea(3, 50)]
	public string textString = "This is a test. Hello, WORLD!";
	public TextAnchor textAnchor = TextAnchor.MiddleCenter;
	public TextAnchor textAlign = TextAnchor.MiddleCenter;
	public float debugTextSize = 1.0f;

	void OnDrawGizmos(){
		var oldColor = Gizmos.color;
		Gizmos.color = color;
		//GizmoTools.drawVisionCone(transform, viewConeHFov, viewConeVFov, viewConeRange, viewConeDegreesPerSegment);
		//GizmoTools.drawVectorTextLine(textString, transform, debugTextSize);
		GizmoTools.drawVectorText(textString, transform, debugTextSize, textAnchor, textAlign);
		Gizmos.color = oldColor;
	}
}
