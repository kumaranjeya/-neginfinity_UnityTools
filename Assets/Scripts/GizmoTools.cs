using UnityEngine;
using System.Collections;

public static class GizmoTools{
	public delegate void LineCallback(Vector3 a, Vector3 b);

	public delegate void LineCallback2DData<Data>(Vector2 a, Vector2 b, Data data);
	public delegate void LineCallback2D(Vector2 a, Vector2 b);

	public static void gizmoLine(Vector3 a, Vector3 b){
		Gizmos.DrawLine(a, b);
	}

	public static void unitCircleLoop2D<Data>(int numSegments, LineCallback2DData<Data> lineCallback, Data data){
		if (lineCallback == null)
			throw new System.ArgumentNullException();
		if (numSegments < 3)
			numSegments = 3;

		unitArcLoop2D(0.0f, (float)Mathf.PI * 2.0f, numSegments, lineCallback, data);
	}

	static Vector2 getSinCos(float radiansAngle){
		return new Vector2(Mathf.Cos(radiansAngle), Mathf.Sin(radiansAngle));
	}

	public static void unitCircleLoop2D(int numSegments, LineCallback lineCallback){
		if (lineCallback == null)
			throw new System.ArgumentNullException();
		unitCircleLoop2D(numSegments, (a, b, data) => {
			data(a, b);
		}, lineCallback);
	}

	public static void unitArcLoop2D(float startAngle, float endAngle, int numSegments, LineCallback2D lineCallback){
		if (lineCallback == null)
			throw new System.ArgumentNullException();
		unitArcLoop2D<LineCallback2D>(startAngle, endAngle, numSegments, (a, b, callback) =>{
			callback(a, b);
		}, lineCallback);
	}

	public static void unitArcLoop2D<Data>(float startAngle, float endAngle, int numSegments, LineCallback2DData<Data> lineCallback, Data data){
		if (lineCallback == null)
			throw new System.ArgumentNullException();

		if (numSegments <= 0)
			throw new System.ArgumentException();

		var prevSinCos = getSinCos(startAngle);
		float angleStep = (endAngle - startAngle)/(float)numSegments;
		for(int i = 1; i <= numSegments; i++){
			float curAngle = startAngle + angleStep * (float)i;
			var curSinCos = getSinCos(curAngle);
			lineCallback(prevSinCos, curSinCos, data);
			prevSinCos = curSinCos;
		}
	}

	public static void processWireCube(GizmoCoords coords, bool makePivotAxes = false){
		processWireCube(coords, makePivotAxes, new Vector3(0.5f, 0.5f, 0.5f), gizmoLine);
	}

	public static void processWireCube(GizmoCoords coords, bool makePivotAxes, Vector3 pivotOffset){
		processWireCube(coords, makePivotAxes, pivotOffset, gizmoLine);
	}

	public static void processWireCube(GizmoCoords coords, bool makePivotAxes, Vector3 pivotOffset, LineCallback lineCallback){
		if (lineCallback == null)
			throw new System.ArgumentNullException();

		var a = coords.pos - coords.xVec * pivotOffset.x - coords.yVec * pivotOffset.y - coords.zVec * pivotOffset.z;
		var b = a + coords.xVec;
		var c  = a + coords.yVec;
		var d = b + coords.yVec;
		var a1 = a + coords.zVec;
		var b1 = b + coords.zVec;
		var c1 = c + coords.zVec;
		var d1 = d + coords.zVec;

		lineCallback(a, b);
		lineCallback(a, c);
		lineCallback(b, d);
		lineCallback(c, d);
		lineCallback(a1, b1);
		lineCallback(a1, c1);
		lineCallback(b1, d1);
		lineCallback(c1, d1);
		lineCallback(a, a1);
		lineCallback(b, b1);
		lineCallback(c, c1);
		lineCallback(d, d1);

		if (!makePivotAxes)
			return;

		var pivotX = coords.pos - pivotOffset.x * coords.xVec;
		var pivotY = coords.pos - pivotOffset.y * coords.yVec;
		var pivotZ = coords.pos - pivotOffset.z * coords.zVec;

		lineCallback(pivotX, pivotX + coords.xVec);
		lineCallback(pivotY, pivotY + coords.yVec);
		lineCallback(pivotZ, pivotZ + coords.zVec);
	}


	//For the love of..   why the heck can't I just have new Vector3() as a default parameter?
	public static void drawWireCube(Vector3 position, Vector3 size){
		drawWireCube(position, size, Quaternion.identity);
	}

	public static void drawWireCube(Vector3 position, Vector3 size, Quaternion rotation){
		drawWireCube(position, size, rotation, new Vector3(0.5f, 0.5f, 0.5f));
	}

	public static void drawWireCube(Vector3 position, Vector3 size, Quaternion rotation, Vector3 pivot, bool pivotAxes = false){
		var coords = new GizmoCoords(position, rotation, size);
		processWireCube(coords, pivotAxes, pivot);
	}

	public static void drawWireCube(Transform transform, Vector3 size){
		drawWireCube(transform, size, new Vector3(0.5f, 0.5f, 0.5f));
	}

	public static void drawWireCube(Transform transform, Vector3 size, Vector3 pivot, bool pivotAxes = false){
		var coords = new GizmoCoords(transform, size);
		processWireCube(coords, pivotAxes, pivot);
	}

	static Vector3 getUnitSphereVertex(float u, float v){
		var uSinCos = getSinCos(u * (float)Mathf.PI * 2.0f);
		var vSinCos = getSinCos(v * (float)Mathf.PI);

		return new Vector3(
			-vSinCos.y * uSinCos.y,
			-vSinCos.x,
			vSinCos.y * uSinCos.x
		);
	}

	static Vector3 getSphereVertex(float u, float v, GizmoCoords coords){
		var unitVert = getUnitSphereVertex(u, v);
		return coords.transformPoint(unitVert);
	}

	public static void processUvSphere(GizmoCoords coords, int uSegments, int vSegments, LineCallback lineCallback){
		if (lineCallback == null)
			throw new System.ArgumentNullException();
		
		if (uSegments < 3){
			uSegments = 3;
		}
		if (vSegments < 2){
			vSegments = 12;
		}

		Vector3 northPole = getSphereVertex(0.0f, 1.0f, coords);
		Vector3 southPole = getSphereVertex(0.0f, 0.0f, coords);

		processUvSurface<GizmoCoords>(uSegments, vSegments, 0, uSegments, 1, vSegments - 1,
			northPole, southPole, true, true, 
			(u, v, data) => getSphereVertex(u, v, data),
			lineCallback, coords);
	}

	public static void drawWireSphere(Transform transform, float localRadius, int uSegments = 8, int vSegments = 4){
		var coords = new GizmoCoords(transform, new Vector3(localRadius, localRadius, localRadius));
		drawWireSphere(coords, uSegments, vSegments);
	}

	public static void drawWireSphere(Vector3 position, float radius, int uSegments = 8, int vSegments = 4){
		drawWireSphere(position, radius, uSegments, vSegments, Quaternion.identity);
	}

	public static void drawWireSphere(Vector3 position, float radius, int uSegments, int vSegments, Quaternion rotation){
		var coords = new GizmoCoords(position, rotation, new Vector3(radius, radius, radius));
		drawWireSphere(coords, uSegments, vSegments);
	}

	static Vector3 getUnitCylinderVertex(float u, float v, GizmoCoords coords){
		var sinCos = getSinCos(u * Mathf.PI * 2.0f);

		var result = coords.transformPoint(new Vector3(-sinCos.y, v, sinCos.x));
		return result;
	}

	public static void drawWireSphere(GizmoCoords coords, int uSegments, int vSegments){
		processUvSphere(coords, uSegments, vSegments, gizmoLine);
	}

	public static void processWireCylinder(GizmoCoords coords, int uSegments, int vSegments, float vPivot, LineCallback lineCallback){
		if (lineCallback == null)
			throw new System.ArgumentNullException();
		
		var southPole = coords.pos - coords.yVec * vPivot;
		var northPole = southPole + coords.yVec;

		coords.pos = southPole;

		if (uSegments < 3){
			uSegments = 3;
		}
		if (vSegments < 1){
			vSegments = 1;
		}

		processUvSurface<GizmoCoords>(uSegments, vSegments, 0, uSegments, 0, vSegments,
			northPole, southPole, true, true, 
			(u, v, data) => getUnitCylinderVertex(u, v, data), 
			lineCallback, coords);
	}

	public static void drawWireCylinder(Vector3 position, float radius, float height){
		drawWireCylinder(position, radius, height, Quaternion.identity);
	}

	public static void drawWireCylinder(Vector3 position, float radius, float height, Quaternion rotation, float vPivot = 0.0f, int uSegments = 8, int vSegments = 1){		
		var coords = new GizmoCoords(position, rotation, new Vector3(radius, height, radius));
		processWireCylinder(coords, uSegments, vSegments, vPivot, gizmoLine);
	}

	public static void drawWireCylinder(Transform transform, float localRadius, float localHeight, float vPivot = 0.0f, int uSegments = 8, int vSegments = 1){
		var coords = new GizmoCoords(transform, new Vector3(localRadius, localHeight, localRadius));
		processWireCylinder(coords, uSegments, vSegments, vPivot, gizmoLine);
	}

	public delegate Vector3 UvSurfaceCallback<Data>(float u, float v, Data data);

	public struct GizmoCoords{
		public Vector3 pos;
		public Vector3 xVec;
		public Vector3 yVec;
		public Vector3 zVec;

		public Vector3 transformVector(Vector3 arg){
			return xVec * arg.x + yVec * arg.y + zVec * arg.z;
		}

		public Vector3 transformPoint(Vector3 arg){
			return pos + transformVector(arg);
		}

		public GizmoCoords(Vector3 pos_){
			pos = pos_;
			xVec = Vector3.right;
			yVec = Vector3.up;
			zVec = Vector3.forward;
		}

		void defaultInit(){
			pos = Vector3.zero;
			xVec = Vector3.right;
			yVec = Vector3.up;
			zVec = Vector3.forward;
		}

		public GizmoCoords(Vector3 pos_, Quaternion rotation)
		:this(pos_, rotation, Vector3.one){
		}

		public GizmoCoords(Vector3 pos_, Quaternion rotation, Vector3 scale){
			pos = pos_;
			xVec = Vector3.right * scale.x;
			yVec = Vector3.up * scale.y;
			zVec = Vector3.forward * scale.z;
			if (rotation != Quaternion.identity){
				xVec = rotation * xVec;
				yVec = rotation * yVec;
				zVec = rotation * zVec;
			}
		}

		public GizmoCoords(Transform transform)
		:this(transform, Vector3.one){
		}

		public GizmoCoords(Transform transform, Vector3 localScale){
			if (transform == null)
				throw new System.ArgumentNullException();
			pos = transform.position;
			xVec = transform.TransformVector(new Vector3(localScale.x, 0.0f, 0.0f));
			yVec = transform.TransformVector(new Vector3(0.0f, localScale.y, 0.0f));
			zVec = transform.TransformVector(new Vector3(0.0f, 0.0f, localScale.z));
		}

		public GizmoCoords(Vector3 pos_, Vector3 xVec_, Vector3 yVec_, Vector3 zVec_){
			pos = pos_;
			xVec = xVec_;
			yVec = yVec_;
			zVec = zVec_;
		}
	}

	public static void processUvSurface<Data>(int uSegments, int vSegments, 
		int minUSegment, int maxUSegment, int minVSegment, int maxVSegment, 
		Vector3 northPole, Vector3 southPole, bool capNorthPole, bool capSouthPole, 
		UvSurfaceCallback<Data> surfaceCallback, LineCallback lineCallback, Data data){

		if (lineCallback == null)
			throw new System.ArgumentNullException();
		if (lineCallback == null)
			throw new System.ArgumentNullException();

		if (uSegments <= 0)
			throw new System.ArgumentException();
		if (vSegments <= 0)
			throw new System.ArgumentException();

		float uStep = 1.0f/(float)uSegments;
		float vStep = 1.0f/(float)vSegments;

		float minV = vStep * (float)minVSegment;
		float maxV = vStep * (float)maxVSegment;
		for(int iU = minUSegment; iU < maxUSegment; iU++){
			float curU = uStep * (float)iU;
			float nextU = curU + uStep;

			{
				var south1 = surfaceCallback(curU, minV, data);
				var south2 = surfaceCallback(nextU, minV, data);
				lineCallback(south1, south2);

				if (capSouthPole){
					lineCallback(southPole, south1);
				}

				if (capNorthPole){
					var north1 = surfaceCallback(curU, maxV, data);
					lineCallback(northPole, north1);
				}
			}

			for(int iV = minVSegment + 1; iV <= maxVSegment; iV++){
				float curV = vStep * (float)iV;
				float prevV = curV - vStep;
				var a = surfaceCallback(curU, curV, data);
				var b = surfaceCallback(nextU, curV, data);
				var c = surfaceCallback(curU, prevV, data);
				lineCallback(a, b);
				lineCallback(a, c);
			}
		}
	}

	static Vector3 getUnitConeVertex(float u, float v){
		var sinCos = getSinCos(u * (float)Mathf.PI * 2.0f);
		var r = 1.0f - v;
		return new Vector3(-sinCos.y * r, v, sinCos.x*r);
	}

	static Vector3 getConeVertex(float u, float v, GizmoCoords coords){//Vector3 pos, Vector3 xVec, Vector3 yVec, Vector3 zVec){
		var tmp = getUnitConeVertex(u, v);
		return coords.transformPoint(tmp);
		//return pos + tmp.x * xVec + tmp.y*yVec + tmp.z*zVec;
	}

	public static void processWireCone(GizmoCoords coords, 
			int uSegments, int vSegments, float vPivot, LineCallback lineCallback){
		if (lineCallback == null)
			throw new System.ArgumentNullException();
		
		var southPole = coords.pos - coords.yVec * vPivot;
		var northPole = southPole + coords.yVec;

		if (uSegments < 3){
			uSegments = 3;
		}
		if (vSegments < 1)
			vSegments = 1;

		coords.pos = southPole;
		processUvSurface<GizmoCoords>(uSegments, vSegments, 0, uSegments, 0, vSegments,
			northPole, southPole, true, true, 
			(u, v, data) => getConeVertex(u, v, data),
			lineCallback, coords);
	}

	public static void drawWireCone(Transform transform, float localRadius, float localHeight, float vPivot = 0.0f, int uSegments = 8, int vSegments = 1){
		GizmoCoords coords = new GizmoCoords(transform, new Vector3(localRadius, localHeight, localRadius));
		processWireCone(coords, uSegments, vSegments, vPivot, gizmoLine);
	}

	public static void drawWireCone(Vector3 position, float radius, float height, Quaternion rotation, float vPivot = 0.0f, int uSegments = 8, int vSegments = 1){
		GizmoCoords coords = new GizmoCoords(position, rotation, new Vector3(radius, height, radius));
		processWireCone(coords, uSegments, vSegments, vPivot, gizmoLine);
	}

	struct UvLineTransform{
		public float len;
		public Quaternion rotation;
		public UvLineTransform(Vector3 start, Vector3 end){
			rotation = Quaternion.identity;
			len = 0.0f;
			var diff = end - start;
			if (diff == Vector3.zero)
				return;
			len = diff.magnitude;
			var dir = diff/len;
			rotation = Quaternion.FromToRotation(Vector3.up, dir);			
		}
	}

	public static void drawCylinderLine(Vector3 start, Vector3 end, float radius, int uSegments = 8, int vSegments = 1){
		var uvTransform = new UvLineTransform(start, end);
		if (uvTransform.len <= 0.0f)
			return;

		drawWireCylinder(start, radius, uvTransform.len, uvTransform.rotation, 0.0f, uSegments, vSegments);
	}

	public static void drawCylinderLineAdaptive(Vector3 start, Vector3 end, float radius, int uSegments = 8, float vSize = 1.0f){
		var uvTransform = new UvLineTransform(start, end);
		if (uvTransform.len <= 0.0f)
			return;

		int vSegments = 1;

		if (vSize > 0.0f)
			vSegments = Mathf.CeilToInt(uvTransform.len/vSize);

		drawWireCylinder(start, radius, uvTransform.len, uvTransform.rotation, 0.0f, uSegments, vSegments);
	}

	public static void drawConeLine(Vector3 start, Vector3 end, float radius, int uSegments = 8, int vSegments = 1){
		var uvTransform = new UvLineTransform(start, end);
		if (uvTransform.len <= 0.0f)
			return;

		drawWireCone(start, radius, uvTransform.len, uvTransform.rotation, 0.0f, uSegments, vSegments);
	}

	public static void drawConeLineAdaptive(Vector3 start, Vector3 end, float radius, int uSegments = 8, float vSize = 1.0f){
		var uvTransform = new UvLineTransform(start, end);
		if (uvTransform.len <= 0.0f)
			return;

		int vSegments = 1;

		if (vSize > 0.0f)
			vSegments = Mathf.CeilToInt(uvTransform.len/vSize);

		drawWireCone(start, radius, uvTransform.len, uvTransform.rotation, 0.0f, uSegments, vSegments);
	}

	public static void drawWeaponCone(Vector3 start, Vector3 target, float spreadAngleDegrees, float range, float angleDegreesPerSegment = 10.0f, int numRadialSegments = 8){
		Vector3 dir = target-start;
		if (range < 0.0f)
			range = dir.magnitude;
		dir = dir.normalized;
		drawWeaponConeDir(start, dir, spreadAngleDegrees, range, angleDegreesPerSegment, numRadialSegments);
	}

	public static void drawWeaponCone(Transform transform, float spreadAngleDegrees, float range, float angleDegreesPerSegment = 10.0f, int numRadialSegments = 8){
		if (!transform)
			throw new System.ArgumentNullException();

		drawWeaponConeDir(transform.position, transform.forward, spreadAngleDegrees, range, angleDegreesPerSegment, numRadialSegments);
	}

	struct WeaponConeData{
		public GizmoCoords coords;
		public float minV;
		public float vScale;
	}

	public static void drawWeaponConeDir(Vector3 start, Vector3 dir, float spreadAngleDegrees, float range, float angleDegreesPerSegment = 10.0f, int numRadialSegments = 8){
		if (angleDegreesPerSegment <= 0.0f)
			throw new System.ArgumentException();
		var rotation = Quaternion.FromToRotation(Vector3.up, dir);
		var coneData = new WeaponConeData();
		coneData.coords = new GizmoCoords(start, rotation, new Vector3(range, range, range));
		float halfAngle = spreadAngleDegrees * 0.5f * Mathf.Deg2Rad;
		coneData.vScale = halfAngle / (float)Mathf.PI;
		coneData.minV = 1.0f - coneData.vScale;
		int numVSegments = (int)(spreadAngleDegrees / angleDegreesPerSegment );
		numVSegments = Mathf.Max(1, numVSegments);// + 1;
		if (numRadialSegments < 3)
			numRadialSegments = 3;
		int numUSegments = numRadialSegments;

		Vector3 northPole = start + dir * range;
		Vector3 southPole = start;

		processUvSurface(numUSegments, numVSegments, 
			0, numUSegments, 0, numVSegments, northPole, southPole, true, true, 
				(u, v, data) => data.coords.transformPoint(getUnitSphereVertex(u, data.minV + data.vScale * v)), 
			gizmoLine, coneData);
	}
}
