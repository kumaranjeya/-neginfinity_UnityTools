using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MathExtensions;

public struct DualQuaternion{
	public Quaternion real;
	public Quaternion dual;

	public static DualQuaternion fromUnityTransform(Quaternion rot, Vector3 pos){
		var r = new DualQuaternion(rot.conjugate(), Vector3.zero);
		var t = new DualQuaternion(Quaternion.identity, pos);
		var result = t * r;
		return result;
	}

	static Quaternion computeTranslation(Quaternion real, Vector3 pos){
		var t = new Quaternion(pos.x, pos.y, pos.z, 0.0f);
		var result = (t* real).scale(0.5f);
		return result;
	}

	public Vector3 getTranslation(){
		//Quaternion t = (real.conjugate() * dual).scale(2.0f);
		Quaternion t = (dual * real.conjugate()).scale(2.0f);
		return new Vector3(t.x, t.y, t.z);
	}

	public void setTranslation(Vector3 newVal){
		dual = computeTranslation(real, newVal);
	}

	public Quaternion getRotation(){
		return real;
	}

	public void setRotation(Quaternion newRotation){
		var pos = getTranslation();
		real = newRotation;
		setTranslation(pos);
	}

	public DualQuaternion(Quaternion r, Quaternion d){
		real = r;
		dual = d;
	}

	public DualQuaternion(Quaternion r, Vector3 pos){
		real = r.normalized();
		dual = computeTranslation(r, pos);
	}

	public static DualQuaternion identity{
		get{
			return new DualQuaternion(new Quaternion(0.0f, 0.0f, 0.0f, 0.0f), new Quaternion(0.0f, 0.0f, 0.0f, 1.0f));
		}
	}

	public static DualQuaternion operator+ (DualQuaternion lhs, DualQuaternion rhs){
		return new DualQuaternion(lhs.real.add(rhs.real), lhs.dual.add(rhs.dual));
	}

	public static DualQuaternion operator*(DualQuaternion lhs, DualQuaternion rhs){
		Quaternion r = rhs.real * lhs.real;
		Quaternion t = (rhs.real * lhs.dual).add(rhs.dual * lhs.real);
		return new DualQuaternion(r, t);
	}

	public static DualQuaternion operator*(DualQuaternion lhs, float rhs){
		return new DualQuaternion(lhs.real.scale(rhs), lhs.dual.scale(rhs));
	}

	public DualQuaternion conjugate(){
		return new DualQuaternion(real.conjugate(), dual.conjugate());
	}

	public DualQuaternion translationConjugate(){
		return new DualQuaternion(real.conjugate(), dual.conjugate().negative());
	}

	public DualQuaternion normalized(){
		float mag = real.dot(real);
		float scale = 1.0f/mag;
		return this * scale;
	}

	public float dot(DualQuaternion v){
		return real.dot(v.real);
	}

	public DualQuaternion sclerp(DualQuaternion to, float t){
		return sclerp(this, to, t);
	}

	public static DualQuaternion sclerp(DualQuaternion from, DualQuaternion to, float t){
		float dot = from.dot(to);
		if (dot < 0)
			to = to * -1.0f;
		var diff = from.conjugate() * to;

		var vr = diff.real.getVectorPart();
		var vd = diff.dual.getVectorPart();

		float invr = 1.0f / Mathf.Sqrt(Vector3.Dot(vr, vr));

		float angle = 2.0f * Mathf.Acos(diff.real.w);
		float pitch = -2.0f * diff.dual.w * invr;

		Vector3 dir = vr * invr;
		Vector3 moment = (vd - dir * pitch * diff.real.w * 0.5f)* invr;

		angle *= t;
		pitch *= t;

		float sinAngle = Mathf.Sin(0.5f * angle);
		float cosAngle = Mathf.Cos(0.5f * angle);

		var result = new DualQuaternion();
		result.real = new Quaternion(dir.x * sinAngle, dir.y * sinAngle, dir.z * sinAngle, cosAngle);
		var rdv = (sinAngle * moment + pitch * 0.5f * cosAngle * dir);
		result.dual = new Quaternion(rdv.x, rdv.y, rdv.z, -pitch * 0.5f * sinAngle);

		return result;
	}

	public Quaternion rotation{
		get{
			return getRotation();
		}
		set{
			setRotation(value);
		}
	}

	public Vector3 translation{
		get{
			return getTranslation();
		}
		set{
			setTranslation(value);
		}
	}

	public Vector3 transformPoint(Vector3 arg){
		var val = new DualQuaternion(new Quaternion(0.0f, 0.0f, 0.0f, 1.0f), new Quaternion(arg.x, arg.y, arg.z, 0.0f));
		var result = this * val * this.translationConjugate();
		return new Vector3(result.dual.x, result.dual.y, result.dual.z);
	}

	public Vector3 transformVector(Vector3 arg){
		return real.transformVector(arg);
	}

	public override string ToString(){
		return string.Format("({0} {1})", real, dual);
	}
}



