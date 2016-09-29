using UnityEngine;
using UnityEditor;
using System.Collections;

public static class GizmoMenu{	
	static void clearLocalTransform(GameObject obj){
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localRotation = Quaternion.identity;
		obj.transform.localScale = Vector3.one;
	}

	static Type createTargetGizmoObject<Type>(string name) where Type: BaseTargetGizmo{
		var comp = createGizmoObject<Type>(name);
		var childObj = new GameObject();
		childObj.name = "Target";
		childObj.transform.SetParent(comp.gameObject.transform, false);
		clearLocalTransform(childObj);
		childObj.transform.localPosition = new Vector3(0, 0, 10.0f);
		comp.target = childObj.transform;
		return comp;
	}

	static Type createGizmoObject<Type>(string name) where Type: Component{
		GameObject parentObj = Selection.activeGameObject;

		var newObj = new GameObject();
		newObj.name = name;
		var result = newObj.AddComponent<Type>();
		if (parentObj){
			newObj.transform.SetParent(parentObj.transform, false);
		}
		clearLocalTransform(newObj);
		return result;
	}
	
	[MenuItem("GameObject/Gizmos/Cube", false, 0)]
	public static void cube(MenuCommand menuCommand){
		createGizmoObject<CubeGizmo>("CubeGizmo");
	}
	[MenuItem("GameObject/Gizmos/Cone", false, 0)]
	public static void cone(MenuCommand menuCommand){
		createGizmoObject<ConeGizmo>("ConeGizmo");
	}
	[MenuItem("GameObject/Gizmos/Cylinder", false, 0)]
	public static void cylinder(MenuCommand menuCommand){
		createGizmoObject<CylinderGizmo>("CylinderGizmo");
	}
	[MenuItem("GameObject/Gizmos/Sphere", false, 0)]
	public static void sphere(MenuCommand menuCommand){
		createGizmoObject<SphereGizmo>("SphereGizmo");
	}
	[MenuItem("GameObject/Gizmos/WeaponCone", false, 0)]
	public static void weaponCone(MenuCommand menuCommand){
		createGizmoObject<WeaponConeGizmo>("WeaponConeGizmo");
	}
	[MenuItem("GameObject/Gizmos/ViewArea", false, 0)]
	public static void viewArea(MenuCommand menuCommand){
		createGizmoObject<ViewAreaGizmo>("ViewAreaGizmo");
	}
	[MenuItem("GameObject/Gizmos/TextLine", false, 0)]
	public static void textLine(MenuCommand menuCommand){
		createGizmoObject<TextLineGizmo>("TextLineGizmo");
	}
	[MenuItem("GameObject/Gizmos/Text", false, 0)]
	public static void text(MenuCommand menuCommand){
		createGizmoObject<TextGizmo>("TextGizmo");
	}
	[MenuItem("GameObject/Gizmos/CylinderLine", false, 0)]
	public static void cylinderLine(MenuCommand menuCommand){
		createTargetGizmoObject<CylinderLineGizmo>("CylinderLineGizmo");
	}
	[MenuItem("GameObject/Gizmos/ConeLine", false, 0)]
	public static void coneLine(MenuCommand menuCommand){
		createTargetGizmoObject<ConeLineGizmo>("ConeLineGizmo");
	}
	[MenuItem("GameObject/Gizmos/AdaptiveCylinderLine", false, 0)]
	public static void adaptiveCylinderLine(MenuCommand menuCommand){
		createTargetGizmoObject<AdaptiveCylinderLineGizmo>("AdaptiveCylinderLineGizmo");
	}
	[MenuItem("GameObject/Gizmos/AdaptiveConeLine", false, 0)]
	public static void adaptiveConeLine(MenuCommand menuCommand){
		createTargetGizmoObject<AdaptiveConeLineGizmo>("AdaptiveConeLineGizmo");
	}
}
