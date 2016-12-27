using UnityEngine;
using UnityEditor;

static class CustomTextMeshMenu{
	[MenuItem("GameObject/3D Object/Custom Text Mesh", false, 0)]
	public static void meshMenu(MenuCommand menuCommand){
		GameObject parent = Selection.activeGameObject;

		var newObj = new GameObject();
		newObj.name = "Text Mesh";
		newObj.AddComponent<CustomTextMesh>();

		if (parent){
			newObj.transform.SetParent(parent.transform, false);
		}
		newObj.transform.localPosition = Vector3.zero;
		newObj.transform.localRotation = Quaternion.identity;
		newObj.transform.localScale = Vector3.one;
	}	
}