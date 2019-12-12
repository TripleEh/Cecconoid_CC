// Cecconoid by Triple Eh? Ltd -- 2019
//
// Cecconoid_CC is distributed under a CC BY 4.0 license. You are free to:
//
// * Share copy and redistribute the material in any medium or format
// * Adapt remix, transform, and build upon the material for any purpose, even commercially.
//
// As long as you give credit to Gareth Noyce / Triple Eh? Ltd.
//
//

using UnityEngine;

public static class MathUtil
{
	// GNTODO: all the other overloads of this! :D
	public static uint Clamp01(int iValue)
	{
		if (iValue < 0) return 0;
		if (iValue > 1) return 1;
		return (uint)iValue;
	}


	public static uint Clamp01(uint iValue)
	{
		if (iValue < 0) return 0;
		if (iValue > 1) return 1;
		return (uint)iValue;
	}


	public static uint Clamp(int iValue, uint min, uint max)
	{
		if (iValue < min) return (uint)min;
		if (iValue > max) return (uint)max;
		return (uint)iValue;
	}


	public static uint Clamp(uint iValue, uint min, uint max)
	{
		if (iValue < min) return min;
		if (iValue > max) return max;
		return (uint)iValue;
	}


	public static ulong Clamp(ulong iValue, ulong min, ulong max)
	{
		if (iValue < min) return min;
		if (iValue > max) return max;
		return (ulong)iValue;
	}


	public static Vector3 GetCurvePoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
	{
		return Vector3.Lerp(Vector3.Lerp(p0, p1, t), Vector3.Lerp(p1, p2, t), t);
	}


	public static Vector3 GetBezierCurvePoint(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
	{
		float fT = Mathf.Clamp(t, 0.0f, 1.0f);
		float u = 1 - fT;
		float tt = fT * fT;
		float uu = u * u;
		float uuu = uu * u;
		float ttt = tt * t;

		Vector3 p = uuu * p0;
		p += 3 * uu * t * p1;
		p += 3 * u * tt * p2;
		p += ttt * p3;

		return p;
	}


	public static Vector3 GetBoundsCheckedVector(Vector2 vPos, float fFudge = 0f)
	{
		Vector2 vOrigin = GameMode.GetRoomOrigin();
		Vector2 vRet = vPos;

		vRet.x = Mathf.Clamp(vPos.x, vOrigin.x - Types.s_fRoomCollisionBoundsX + fFudge, vOrigin.x + Types.s_fRoomCollisionBoundsX - fFudge);
		vRet.y = Mathf.Clamp(vPos.y, vOrigin.y - Types.s_fRoomCollisionBoundsY + fFudge, vOrigin.y + Types.s_fRoomCollisionBoundsY - fFudge);
		return vRet;
	}


	public static Vector2 GetReflectionVectorFromBoundsCheck(Vector2 vPos, float fFudge = 0f)
	{
		Vector2 vOrigin = GameMode.GetRoomOrigin();
		Vector2 vRet = Vector3.zero;

		if ((vPos.x + fFudge) > vOrigin.x + Types.s_fRoomCollisionBoundsX) vRet.x = -1f;
		if ((vPos.x - fFudge) < vOrigin.x - Types.s_fRoomCollisionBoundsX) vRet.x = 1f;
		if ((vPos.y + fFudge) > vOrigin.y + Types.s_fRoomCollisionBoundsY) vRet.y = 1f;
		if ((vPos.y - fFudge) < vOrigin.y - Types.s_fRoomCollisionBoundsY) vRet.y = -1f;

		return vRet;
	}


	public static Vector2 GetReflectionVectorFromEugatronBoundsCheck(Vector2 vPos, float fFudge = 0f)
	{
		//Vector2 vOrigin = GameMode_Eugatron.GetRoomOrigin();
		Vector2 vOrigin = GameMode.GetRoomOrigin();
		Vector2 vRet = Vector3.zero;

		if ((vPos.x + fFudge) > vOrigin.x + Types.s_fEugatronRoomCollisionBoundsX) vRet.x = -1f;
		if ((vPos.x - fFudge) < vOrigin.x - Types.s_fEugatronRoomCollisionBoundsX) vRet.x = 1f;
		if ((vPos.y + fFudge) > vOrigin.y + Types.s_fEugatronRoomCollisionBoundsY) vRet.y = 1f;
		if ((vPos.y - fFudge) < vOrigin.y - Types.s_fEugatronRoomCollisionBoundsY) vRet.y = -1f;

		return vRet;
	}
}
