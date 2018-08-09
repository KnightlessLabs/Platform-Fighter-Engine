using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct FPVector2{
    public FPValue x;
    public FPValue y;

	#region OPERATOR OVERLOADING
	public static FPVector2 operator +(FPVector2 lhs, FPVector2 rhs){
		lhs.x.rawValue += rhs.x.rawValue;
        lhs.y.rawValue += rhs.x.rawValue;
		return lhs;
	}

	public static FPVector2 operator -(FPVector2 lhs, FPVector2 rhs){
		lhs.x.rawValue -= rhs.x.rawValue;
        lhs.y.rawValue -= rhs.y.rawValue;
		return lhs;
	}

	public static FPVector2 operator *(FPVector2 lhs, FPVector2 rhs){
		lhs.x.rawValue *= rhs.x.rawValue;
		lhs.x.rawValue >>= 16; // convert the Q32 number back into Q16
		lhs.y.rawValue *= rhs.y.rawValue;
		lhs.y.rawValue >>= 16; // convert the Q32 number back into Q16
		return lhs;
	}

	public static FPVector2 operator /(FPVector2 lhs, FPVector2 rhs){
		lhs.x.rawValue <<= 16; // convert into Q32 number
		lhs.x.rawValue /= rhs.x.rawValue; // is now a Q16 number again
		lhs.y.rawValue <<= 16; // convert into Q32 number
		lhs.y.rawValue /= rhs.y.rawValue; // is now a Q16 number again
		return lhs;
	}
	#endregion
}