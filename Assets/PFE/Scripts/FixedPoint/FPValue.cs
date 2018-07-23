using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct FPValue{
	public Int64 rawValue; // the raw Q16 value
	// 2^16 = 65536

	public Int16 IntValue{
		get{
			return (Int16) (( this.rawValue >> 16 ) + ( this.rawValue < 0 ? 1 : 0 ));
		}
	}

	public Int16 RawDecimalValue{
		get{
			return (Int16)( this.rawValue & 0xffff );
		}
	}

	public float DecimalValue{
		get{
			return this.RawDecimalValue / (float)( 1 << 16 );
		}
	}

	public float FloatValue{
		get{
			return (float)( ((int)(this.rawValue >> 16)) + ( this.DecimalValue ) );
		}
	}

	public static FPValue FromWholeInt( int value ){
		FPValue fixe = new FPValue();
		fixe.rawValue = ( value << 16 );
		return fixe;
	}

	public static FPValue FromFloat( float value ){
		int wholePortion = (int)System.Math.Truncate( value );
		int decimalPortion = (int)( System.Math.Abs(value - wholePortion) * (1 << 16) );

		if (wholePortion < 0 && decimalPortion != 0){
			wholePortion--; // If we subtracted 0.5 from -10 or added 0.5 to -11 we'd get integer portion of -11 and decimal portion of 0.5, so we ensure the same behavior exists in a float conversion.
		}

		FPValue fixe = new FPValue();
		fixe.rawValue = (wholePortion << 16) | (decimalPortion & 0xffff);
		return fixe;
	}

	public FPValue( int rawValue ){
		this.rawValue = rawValue;
	}

	#region EXPLICIT CAST
	public static explicit operator FPValue( int wholeInteger ){
		return FPValue.FromWholeInt( wholeInteger );
	}

	public static explicit operator int( FPValue fixedInt ){
		return fixedInt.IntValue;
	}

	public static explicit operator FPValue( float floatVal ){
		return FPValue.FromFloat( floatVal );
	}

	public static explicit operator float( FPValue fixedInt ){
		return fixedInt.FloatValue;
	}
	#endregion

	#region OPERATOR OVERLOADING
	public static FPValue operator +(FPValue lhs, FPValue rhs){
		lhs.rawValue += rhs.rawValue;
		return lhs;
	}

	public static FPValue operator -(FPValue lhs, FPValue rhs){
		lhs.rawValue -= rhs.rawValue;
		return lhs;
	}

	public static FPValue operator *(FPValue lhs, FPValue rhs){
		lhs.rawValue *= rhs.rawValue;
		lhs.rawValue >>= 16; // convert the Q32 number back into Q16
		return lhs;
	}

	public static FPValue operator /(FPValue lhs, FPValue rhs){
		lhs.rawValue <<= 16; // convert into Q32 number
		lhs.rawValue /= rhs.rawValue; // is now a Q16 number again
		return lhs;
	}
	#endregion
}
