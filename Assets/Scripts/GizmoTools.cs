using UnityEngine;
using System.Collections;

public static class GizmoTools{
	public delegate void LineCallback(Vector3 a, Vector3 b);

	public delegate void LineCallback2DData<Data>(Vector2 a, Vector2 b, Data data);
	public delegate void LineCallback2D(Vector2 a, Vector2 b);
	//public delegate void AngleCallback(float prevAngle, float nextAngle);

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

	public static void processWireCube(Vector3 position, 
			Vector3 xVec, Vector3 yVec, Vector3 zVec, bool makePivotAxes = false){
		processWireCube(position, xVec, yVec, zVec, makePivotAxes, new Vector3(0.5f, 0.5f, 0.5f), gizmoLine);
	}

	public static void processWireCube(Vector3 position, 
			Vector3 xVec, Vector3 yVec, Vector3 zVec, bool makePivotAxes, Vector3 pivotOffset){
		processWireCube(position, xVec, yVec, zVec, makePivotAxes, pivotOffset, gizmoLine);
	}

	public static void processWireCube(Vector3 position, 
			Vector3 xVec, Vector3 yVec, Vector3 zVec, bool makePivotAxes, Vector3 pivotOffset, LineCallback lineCallback){

		if (lineCallback == null)
			throw new System.ArgumentNullException();

		var a = position - xVec * pivotOffset.x - yVec * pivotOffset.y - zVec * pivotOffset.z;
		var b = a + xVec;
		var c  = a + yVec;
		var d = b + yVec;
		var a1 = a + zVec;
		var b1 = b + zVec;
		var c1 = c + zVec;
		var d1 = d + zVec;

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

		var pivotX = position - pivotOffset.x * xVec;
		var pivotY = position - pivotOffset.y * yVec;
		var pivotZ = position - pivotOffset.z * zVec;

		lineCallback(pivotX, pivotX + xVec);
		lineCallback(pivotY, pivotY + yVec);
		lineCallback(pivotZ, pivotZ + zVec);
	}


	//For the love of..   why the heck can't I just have new Vector3() as a default parameter?
	public static void drawWireCube(Vector3 position, Vector3 size){
		drawWireCube(position, size, Quaternion.identity);
	}

	public static void drawWireCube(Vector3 position, Vector3 size, Quaternion rotation){
		drawWireCube(position, size, rotation, new Vector3(0.5f, 0.5f, 0.5f));
	}

	public static void drawWireCube(Vector3 position, Vector3 size, Quaternion rotation, Vector3 pivot, bool pivotAxes = false){
		var xVec = size.x * Vector3.right;
		var yVec = size.y * Vector3.up;
		var zVec = size.z * Vector3.forward;

		if (rotation != Quaternion.identity){
			xVec = rotation * xVec;
			yVec = rotation * yVec;
			zVec = rotation * zVec;
		}

		processWireCube(position, xVec, yVec, zVec, pivotAxes, pivot);
	}

	public static void drawWireCube(Transform transform, Vector3 size){
		drawWireCube(transform, size, new Vector3(0.5f, 0.5f, 0.5f));
	}

	public static void drawWireCube(Transform transform, Vector3 size, Vector3 pivot, bool pivotAxes = false){
		if (!transform){
			throw new System.ArgumentNullException();
		}
		var xVec = transform.TransformVector(Vector3.right * size.x);
		var yVec = transform.TransformVector(Vector3.up * size.y);
		var zVec = transform.TransformVector(Vector3.forward * size.z);

		processWireCube(transform.position, xVec, yVec, zVec, pivotAxes, pivot);
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

	static Vector3 getSphereVertex(float u, float v, Vector3 position, Vector3 xVec, Vector3 yVec, Vector3 zVec){
		var unitVert = getUnitSphereVertex(u, v);
		return position + unitVert.x * xVec + unitVert.y * yVec + unitVert.z * zVec;
	}

	public static void processUvSphere(Vector3 position, Vector3 xVec, Vector3 yVec, Vector3 zVec, 
			int uSegments, int vSegments, LineCallback lineCallback){
		if (uSegments < 3)
			uSegments = 3;
		if (vSegments < 3)
			vSegments = 3;

		float uStep = 1.0f / (float)uSegments;
		float vStep = 1.0f / (float)vSegments;

		Vector3 northPole = getSphereVertex(0.0f, 1.0f, position, xVec, yVec, zVec);
		Vector3 southPole = getSphereVertex(0.0f, 0.0f, position, xVec, yVec, zVec);
		float minSegmentV = vStep;
		float maxSegmentV = 1.0f - vStep;

		for (int iU = 0; iU < uSegments; iU++){
			float curU = uStep * (float)iU;
			float nextU = uStep * (float)(iU + 1);
			//I really need to use matrices here instead of this
			//poles
			{
				var north1 = getSphereVertex(curU, maxSegmentV, position, xVec, yVec, zVec);
				//var north2 = getSphereVertex(nextU, maxSegmentV, position, xVec, yVec, zVec);
				var south1 = getSphereVertex(curU, minSegmentV, position, xVec, yVec, zVec);
				var south2 = getSphereVertex(nextU, minSegmentV, position, xVec, yVec, zVec);
				lineCallback(northPole, north1);
				lineCallback(southPole, south1);
				lineCallback(south1, south2);
			}
			//segments between poles
			for (int iV = 2; iV < vSegments; iV++){
				float curV = vStep * (float)iV;
				float prevV = vStep * (float)(iV - 1);

				var a = getSphereVertex(curU, curV, position, xVec, yVec, zVec);
				var b = getSphereVertex(nextU, curV, position, xVec, yVec, zVec);
				var c = getSphereVertex(curU, prevV, position, xVec, yVec, zVec);

				lineCallback(a, b);
				lineCallback(a, c);
			}
		}
	}

	public static void drawWireSphere(Transform transform, float localRadius, int uSegments = 8, int vSegments = 4){
		if (!transform)
			throw new System.ArgumentNullException();

		var pos = transform.position;
		var xVec = transform.TransformVector(Vector3.right * localRadius);
		var yVec = transform.TransformVector(Vector3.up * localRadius);
		var zVec = transform.TransformVector(Vector3.forward * localRadius);

		drawWireSphere(pos, xVec, yVec, zVec, uSegments, vSegments);
	}

	public static void drawWireSphere(Vector3 position, float radius, int uSegments = 8, int vSegments = 4){
		drawWireSphere(position, radius, uSegments, vSegments, Quaternion.identity);
	}

	public static void drawWireSphere(Vector3 position, float radius, int uSegments, int vSegments, Quaternion rotation){
		var xVec = Vector3.right * radius;
		var yVec = Vector3.up * radius;
		var zVec = Vector3.forward * radius;
		if (rotation != Quaternion.identity){
			xVec = rotation * xVec;
			yVec = rotation * yVec;
			zVec = rotation * zVec;
		}
		drawWireSphere(position, xVec, yVec, zVec, uSegments, vSegments);
	}

	public static void drawWireSphere(Vector3 position, Vector3 xVec, Vector3 yVec, Vector3 zVec, 
			int uSegments, int vSegments){
		processUvSphere(position, xVec, yVec, zVec, uSegments, vSegments, gizmoLine);
	}
}
