using UnityEngine;
using System.Collections;

namespace ObjectExtensions{
	public static class Extensions{
		static void nullCheck(GameObject gameObject){
			if (!gameObject)
				throw new System.ArgumentNullException();
		}

		static void nullCheck(Component component){
			if (!component)
				throw new System.ArgumentNullException();
		}

		public static Comp getOrCreateComponent<Comp>(this GameObject gameObject, System.Action<Comp> initializer = null) where Comp: Component{
			nullCheck(gameObject);
			var result = gameObject.GetComponent<Comp>();
			if (!result){
				result = gameObject.AddComponent<Comp>();
				if (initializer != null)
					initializer(result);
			}
			return result;
		}

		public static Comp getOrCreateComponent<Comp>(this Component curComponent, System.Action<Comp> initializer = null) where Comp: Component{
			nullCheck(curComponent);
			return curComponent.gameObject.getOrCreateComponent<Comp>(initializer);
		}

		public static bool processComponent<Comp>(this GameObject gameObject, System.Action<Comp> action) where Comp: Component{
			nullCheck(gameObject);
			var comp = gameObject.GetComponent<Comp>();
			if (!comp)
				return false;
			if (action != null){
				action(comp);
			}
			return true;
		}

		public static bool processComponent<Comp>(this Component curComponent, System.Action<Comp> action) where Comp: Component{
			nullCheck(curComponent);
			return curComponent.gameObject.processComponent(action);
		}

		public static bool processComponent<Comp, Data>(this GameObject gameObject, System.Action<Comp, Data> action, Data data) where Comp: Component{
			nullCheck(gameObject);
			var comp = gameObject.GetComponent<Comp>();
			if (!comp)
				return false;
			if (action != null){
				action(comp, data);
			}
			return true;
		}

		public static bool processComponent<Comp, Data>(this Component curComponent, System.Action<Comp, Data> action, Data data) where Comp: Component{
			nullCheck(curComponent);
			return curComponent.gameObject.processComponent<Comp, Data>(action, data);
		}


		public static bool hasComponent<Comp>(this GameObject gameObject) where Comp: Component{
			nullCheck(gameObject);
			return gameObject.GetComponent<Comp>();
		}

		public static bool hasComponent<Comp>(this Component curComponent) where Comp: Component{
			nullCheck(curComponent);
			return curComponent.gameObject.hasComponent<Comp>();
		}

		public static void setLocalTransform(this GameObject gameObject, Vector3 position, Quaternion rotation, Vector3 scale){
			nullCheck(gameObject);

			gameObject.transform.localPosition = position;
			gameObject.transform.localRotation = rotation;
			gameObject.transform.localScale = scale;
		}

		public static void setLocalPosition(this GameObject gameObject, Vector3 position){
			nullCheck(gameObject);

			gameObject.transform.localPosition = position;
		}

		public static Vector3 getLocalPosition(this GameObject gameObject){
			nullCheck(gameObject);
			return gameObject.transform.localPosition;
		}

		public static Quaternion getLocalRotation(this GameObject gameObject){
			nullCheck(gameObject);
			return gameObject.transform.localRotation;
		}

		public static Vector3 getLocalScale(this GameObject gameObject){
			nullCheck(gameObject);
			return gameObject.transform.localScale;
		}

		public static void setLocalRotation(this GameObject gameObject, Quaternion rotation){
			nullCheck(gameObject);
			gameObject.transform.localRotation = rotation;
		}

		public static void setLocalScale(this GameObject gameObject, Vector3 scale){
			nullCheck(gameObject);
			gameObject.transform.localScale = scale;
		}

		public static void resetLocalTransform(this GameObject gameObject){
			nullCheck(gameObject);
			gameObject.transform.localPosition = Vector3.zero;
			gameObject.transform.localRotation = Quaternion.identity;
			gameObject.transform.localScale = Vector3.one;
		}

		public static void setParent(this GameObject gameObject, GameObject newParent, bool worldPositionStays = true){
			nullCheck(gameObject);
			Transform newParentTransform = null;
			if (newParent)
				newParentTransform = newParent.transform;
			gameObject.transform.SetParent(newParentTransform, worldPositionStays);
		}

		public static void setParent(this GameObject gameObject, Transform newParent, bool worldPositionStays = true){
			nullCheck(gameObject);
			gameObject.transform.SetParent(newParent, worldPositionStays);
		}

		//childAction must return true in order to continue traversing hierarchy
		public static void processHierarchy(this GameObject gameObject, System.Func<GameObject, bool> childAction){
			if (!gameObject)
				return;
			if (childAction == null)
				return;
			if (!childAction(gameObject))
				return;
			foreach(Transform cur in gameObject.transform){
				cur.gameObject.processHierarchy(childAction);
			}
		}

		//childAction must return true in order to continue traversing hierarchy
		public static void processHierarchy<Data>(this GameObject gameObject, System.Func<GameObject, Data, bool> childAction, Data data){
			if (!gameObject)
				return;
			if (childAction == null)
				return;
			if (!childAction(gameObject, data))
				return;
			foreach(Transform cur in gameObject.transform){
				cur.gameObject.processHierarchy(childAction, data);
			}
		}

		struct ProcessComponentsPayload<Comp> where Comp: Component{
			public System.Action<Comp> componentAction;
			public System.Func<GameObject, bool> objectFilter;
		}

		struct ProcessComponentsDataPayload<Comp, Data> where Comp: Component{
			public System.Action<Comp, Data> componentAction;
			public System.Func<GameObject, Data, bool> objectFilter;
			public Data data;
		}

		public static void processComponentsInChildren<Comp>(this GameObject gameObject, 
				System.Action<Comp> componentAction, System.Func<GameObject, bool> objectFilter) where Comp: Component{
			if (componentAction == null)
				return;
			if (gameObject && (objectFilter != null) && !objectFilter(gameObject))
				return;
			var payload = new ProcessComponentsPayload<Comp>();
			payload.componentAction = componentAction;
			payload.objectFilter = objectFilter;
			gameObject.processHierarchy((obj, data) => {
				if ((data.objectFilter != null) && (!data.objectFilter(obj)))
					return false;
				obj.processComponent<Comp>(data.componentAction);
				return true;
			}, payload);			
		}

		public static void processComponentsInChildren<Comp>(this Component curComponent, 
				System.Action<Comp> componentAction, System.Func<GameObject, bool> objectFilter) where Comp: Component{
			if (!curComponent)
				return;
			curComponent.gameObject.processComponentsInChildren<Comp>(componentAction, objectFilter);
		}

		public static void processComponentsInChildren<Comp, Data>(this GameObject gameObject, 
				System.Action<Comp, Data> componentAction, System.Func<GameObject, Data, bool> objectFilter, Data data) where Comp: Component{
			if (componentAction == null)
				return;
			if (gameObject && (objectFilter != null) && !objectFilter(gameObject, data))
				return;
			var payload = new ProcessComponentsDataPayload<Comp, Data>();
			payload.componentAction = componentAction;
			payload.objectFilter = objectFilter;
			payload.data = data;
			gameObject.processHierarchy((_obj, _objData) => {
				if ((_objData.objectFilter != null) && (!_objData.objectFilter(_obj, _objData.data)))
					return false;
				_obj.processComponent<Comp, Data>(_objData.componentAction, _objData.data);
				return true;
			}, payload);			
		}

		public static void processComponentsInChildren<Comp, Data>(this Component curComponent, 
				System.Action<Comp, Data> componentAction, System.Func<GameObject, Data, bool> objectFilter, Data data) where Comp: Component{
			if (!curComponent)
				return;
			curComponent.gameObject.processComponentsInChildren<Comp, Data>(componentAction, objectFilter, data);
		}
	}		
}