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
using System.Collections;
using System;

// Quick and dirty class to provide Easing curves, in code.
// Ported from C++ 	version that originated here:  http://www.robertpenner.com/easing/


// Define the types of easing curves we support
//
public enum EEasingType
{
	Step,
	Linear,
	Sine,
	Quadratic,
	Cubic,
	Quartic,
	Quintic
}

// Define easing end types
//
public enum EEasingEnds {
	_In,
	_InOut,
	_Out,
};



// Static methods for each of the curves so we can call via the Class name, ie: Easing::EaseInOut(....) etc. 
//	
public static class Easing
{
	public static float Ease(double linearStep, float acceleration, EEasingType type)
	{
		float easedStep = acceleration > 0 ? EaseIn(linearStep, type) :
											acceleration < 0 ? EaseOut(linearStep, type) :
											(float)linearStep;

		return MathHelper.Lerp(linearStep, easedStep, Math.Abs(acceleration));
	}

	public static float EaseIn(double linearStep, EEasingType type)
	{
		switch (type)
		{
			case EEasingType.Step: return linearStep < 0.5 ? 0 : 1;
			case EEasingType.Linear: return (float)linearStep;
			case EEasingType.Sine: return Sine.EaseIn(linearStep);
			case EEasingType.Quadratic: return Power.EaseIn(linearStep, 2);
			case EEasingType.Cubic: return Power.EaseIn(linearStep, 3);
			case EEasingType.Quartic: return Power.EaseIn(linearStep, 4);
			case EEasingType.Quintic: return Power.EaseIn(linearStep, 5);
		}
		throw new NotImplementedException();
	}

	public static float EaseOut(double linearStep, EEasingType type)
	{
		switch (type)
		{
			case EEasingType.Step: return linearStep < 0.5 ? 0 : 1;
			case EEasingType.Linear: return (float)linearStep;
			case EEasingType.Sine: return Sine.EaseOut(linearStep);
			case EEasingType.Quadratic: return Power.EaseOut(linearStep, 2);
			case EEasingType.Cubic: return Power.EaseOut(linearStep, 3);
			case EEasingType.Quartic: return Power.EaseOut(linearStep, 4);
			case EEasingType.Quintic: return Power.EaseOut(linearStep, 5);
		}
		throw new NotImplementedException();
	}

	public static float EaseInOut(double linearStep, EEasingType easeInType, EEasingType easeOutType)
	{
		return linearStep < 0.5 ? EaseInOut(linearStep, easeInType) : EaseInOut(linearStep, easeOutType);
	}

	public static float EaseInOut(double linearStep, EEasingType type)
	{
		switch (type)
		{
			case EEasingType.Step: return linearStep < 0.5 ? 0 : 1;
			case EEasingType.Linear: return (float)linearStep;
			case EEasingType.Sine: return Sine.EaseInOut(linearStep);
			case EEasingType.Quadratic: return Power.EaseInOut(linearStep, 2);
			case EEasingType.Cubic: return Power.EaseInOut(linearStep, 3);
			case EEasingType.Quartic: return Power.EaseInOut(linearStep, 4);
			case EEasingType.Quintic: return Power.EaseInOut(linearStep, 5);
		}
		throw new NotImplementedException();
	}

	static class Sine
	{
		public static float EaseIn(double s)
		{
			return (float)Math.Sin(s * MathHelper.HalfPi - MathHelper.HalfPi) + 1;
		}
		public static float EaseOut(double s)
		{
			return (float)Math.Sin(s * MathHelper.HalfPi);
		}
		public static float EaseInOut(double s)
		{
			return (float)(Math.Sin(s * MathHelper.Pi - MathHelper.HalfPi) + 1) / 2;
		}
	}

	static class Power
	{
		public static float EaseIn(double s, int power)
		{
			return (float)Math.Pow(s, power);
		}
		public static float EaseOut(double s, int power)
		{
			var sign = power % 2 == 0 ? -1 : 1;
			return (float)(sign * (Math.Pow(s - 1, power) + sign));
		}
		public static float EaseInOut(double s, int power)
		{
			s *= 2;
			if (s < 1) return EaseIn(s, power) / 2;
			var sign = power % 2 == 0 ? -1 : 1;
			return (float)(sign / 2.0 * (Math.Pow(s - 2, power) + sign * 2));
		}
	}
}

// GNTODO: Class doesn't need this, can use the standard Math class
//
public static class MathHelper
{
	public const float Pi = (float)Math.PI;
	public const float HalfPi = (float)(Math.PI / 2);

	public static float Lerp(double from, double to, double step)
	{
		return (float)((to - from) * step + from);
	}
}

