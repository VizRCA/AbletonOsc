//
// Klak - Utilities for creative coding with Unity
//
// Copyright (C) 2016 Keijiro Takahashi
//
// Klak Math Utils - Extended maths functions
// 
// Copyright (C) 2018 Thomas Deacon
//
// Klak Math Utils is a port of toxiclibs, Copyright (c) 2006-2011 Karsten Schmidt
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//

namespace Klak.Math
{
    using System;

    public static class MathUtils
    {
        

    public static float THIRD = 1f / 3;

    /**
     * Square root of 2
     */
    public static float SQRT2 = (float) Math.Sqrt(2);

    /**
     * Square root of 3
     */
    public static float SQRT3 = (float) Math.Sqrt(3);

    /**
     * Log(2)
     */
    public static float LOG2 = (float) Math.Log(2);

    /**
     * PI
     */
    public static float PI = 3.14159265358979323846f;

    /**
     * The reciprocal of PI: (1/PI)
     */
    public static float INV_PI = 1f / PI;

    /**
     * PI/2
     */
    public static float HALF_PI = PI / 2;

    /**
     * PI/3
     */
    public static float THIRD_PI = PI / 3;

    /**
     * PI/4
     */
    public static float QUARTER_PI = PI / 4;

    /**
     * PI*2
     */
    public static float TWO_PI = PI * 2;

    /**
     * PI*1.5
     */
    public static float THREE_HALVES_PI = TWO_PI - HALF_PI;

    /**
     * PI*PI
     */
    public static float PI_SQUARED = PI * PI;

    /**
     * Epsilon value
     */
    public static float EPS = 1.1920928955078125E-7f;

    /**
     * Degrees to radians conversion factor
     */
    public static float DEG2RAD = PI / 180;

    /**
     * Radians to degrees conversion factor
     */
    public static float RAD2DEG = 180 / PI;

    private static float SHIFT23 = 1 << 23;

    private static float INV_SHIFT23 = 1.0f / SHIFT23;
    private static double SIN_A = -4d / (PI * PI);

    private static double SIN_B = 4d / PI;
    private static double SIN_P = 9d / 40;

    /**
     * Default random number generator used by random methods of this class
     * which don't use a passed in {@link Random} instance.
     */
    public static Random RND = new Random();

    /**
     * @param x
     * @return Absolute value of x
     */
    public static  double Abs(double x) {
        return x < 0 ? -x : x;
    }

    /**
     * @param x
     * @return Absolute value of x
     */
    public static  float Abs(float x) {
        return x < 0 ? -x : x;
    }

    /**
     * @param x
     * @return Absolute value of x
     */
    public static  int Abs(int x) {
        int y = x >> 31;
        return (x ^ y) - y;
    }

    /**
     * Rounds up the value to the nearest higher power^2 value.
     * 
     * @param x
     * @return power^2 value
     */
    public static  int CeilPowerOf2(int x) {
        int pow2 = 1;
        while (pow2 < x) {
            pow2 <<= 1;
        }
        return pow2;
    }

    public static  double Clip(double a, double min, double max) {
        return a < min ? min : (a > max ? max : a);
    }

    public static  float Clip(float a, float min, float max) {
        return a < min ? min : (a > max ? max : a);
    }

    public static  int Clip(int a, int min, int max) {
        return a < min ? min : (a > max ? max : a);
    }

    public static double ClipNormalized(double a) {
        if (a < 0) {
            return 0;
        } else if (a > 1) {
            return 1;
        }
        return a;
    }

    /**
     * Clips the value to the 0.0 .. 1.0 interval.
     * 
     * @param a
     * @return Clipped value
     * @Since 0012
     */
    public static  float ClipNormalized(float a) {
        if (a < 0) {
            return 0;
        } else if (a > 1) {
            return 1;
        }
        return a;
    }

    public static  double Cos( double theta) {
        return Sin(theta + HALF_PI);
    }

    /**
     * Returns fast CoSine approximation of a value. Note: code from <a
     * href="http://wiki.java.net/bin/view/Games/JeffGems">wiki posting on
     * java.net by jeffpk</a>
     * 
     * @param theta
     *            angle in radians.
     * @return CoSine of theta.
     */
    public static  float Cos( float theta) {
        return Sin(theta + HALF_PI);
    }

    public static  double Degrees(double radians) {
        return radians * RAD2DEG;
    }

    public static  float Degrees(float radians) {
        return radians * RAD2DEG;
    }

    public static double DualSign(double a, double b) {
        double x = (a >= 0 ? a : -a);
        return (b >= 0 ? x : -x);
    }

    /**
     * Fast CoSine approximation.
     * 
     * @param x
     *            angle in -PI/2 .. +PI/2 interval
     * @return CoSine
     */
    public static  double FastCos( double x) {
        return FastSin(x + ((x > HALF_PI) ? -THREE_HALVES_PI : HALF_PI));
    }

    // /**
    //  * @deprecated
    //  */
    // public static  float FastInverseSqrt(float x) {
    //     float half = 0.5F * x;
    //     int i = BitConverter.ToInt32(BitConverter.GetBytes(x), 0);
    //     i = 0x5f375a86 - (i >> 1);
    //     x = Float.intBitsToFloat(i);
    //     return x * (1.5F - half * x * x);
    // }

    /**
     * Computes a fast approximation to <code>Math.pow(a, b)</code>. Adapted
     * from http://www.dctsystems.co.uk/Software/power.html.
     * 
     * @param a
     *            a positive number
     * @param b
     *            a number
     * @return a^b
     * 
     */
    public static  float FastPow(float a, float b) {
        float x = FloatToRawIntBits(a);
        x *= INV_SHIFT23;
        x -= 127;
        float y = x - (x >= 0 ? (int) x : (int) x - 1);
        b *= x + (y - y * y) * 0.346607f;
        y = b - (b >= 0 ? (int) b : (int) b - 1);
        y = (y - y * y) * 0.33971f;
        return IntBitsToFloat((int) ((b + 127 - y) * SHIFT23));
    }

    public static int FloatToRawIntBits(float value)
    {
        byte[] bytes = BitConverter.GetBytes(value);
        int result = BitConverter.ToInt32(bytes, 0);
        return result;
    }

    public static float IntBitsToFloat(int value)
    {
        byte[] bytes = BitConverter.GetBytes(value);
        float f = BitConverter.ToSingle(bytes, 0);
        return f;
    }

    /**
     * Fast Sine approximation.
     * 
     * @param x
     *            angle in -PI/2 .. +PI/2 interval
     * @return Sine
     */
    public static  double FastSin(double x) {
        // float B = 4/pi;
        // float C = -4/(pi*pi);
        // float y = B * x + C * x * Abs(x);
        // y = P * (y * Abs(y) - y) + y;
        x = SIN_B * x + SIN_A * x * Abs(x);
        return SIN_P * (x * Abs(x) - x) + x;
    }

    public static  bool FlipCoin() {
        return RND.Next(100) % 2 == 0;
    }

    public static  bool FlipCoin(Random rnd) {
        return rnd.Next(100) % 2 == 0;
    }

    public static  long Floor(double x) {
        long y = (long) x;
        if (x < 0 && x != y) {
            y--;
        }
        return y;
    }

    /**
     * This method is a *lot* faster than uSing (int)Math.Floor(x).
     * 
     * @param x
     *            value to be Floored
     * @return Floored value as integer
     * @Since 0012
     */
    public static  int Floor(float x) {
        int y = (int) x;
        if (x < 0 && x != y) {
            y--;
        }
        return y;
    }

    /**
     * Rounds down the value to the nearest lower power^2 value.
     * 
     * @param x
     * @return power^2 value
     */
    public static  int FloorPowerOf2(int x) {
        return (int) Math.Pow(2, (int) (Math.Log(x) / LOG2));
    }

    /**
     * Computes the Greatest Common Devisor of integers p and q.
     * 
     * @param p
     * @param q
     * @return gcd
     */
    public static  int Gcd(int p, int q) {
        if (q == 0) {
            return p;
        }
        return Gcd(q, p % q);
    }

    /**
     * Creates a Single normalized impulse signal with its peak at t=1/k. The
     * attack and decay period is configurable via the k parameter. Code from:
     * http://www.iquilezles.org/www/articles/functions/functions.htm
     * 
     * @param k
     *            smoothness
     * @param t
     *            time position (should be >= 0)
     * @return impulse value (as double)
     */
    public static double Impulse(double k, double t) {
        double h = k * t;
        return h * Math.Exp(1.0 - h);
    }

    /**
     * Creates a Single normalized impulse signal with its peak at t=1/k. The
     * attack and decay period is configurable via the k parameter. Code from:
     * http://www.iquilezles.org/www/articles/functions/functions.htm
     * 
     * @param k
     *            smoothness
     * @param t
     *            time position (should be >= 0)
     * @return impulse value (as float)
     */
    public static float Impulse(float k, float t) {
        float h = k * t;
        return (float) (h * Math.Exp(1.0f - h));
    }

    public static  int Lcm(int p, int q) {
        return Abs(p * q) / Gcd(p, q);
    }

    // public static double lerp(double a, double b, double t) {
    //     return a + (b - a) * t;
    // }

    // public static float lerp(float a, float b, float t) {
    //     return a + (b - a) * t;
    // }

    public static double MapInterval(double x, double minIn, double maxIn,
            double minOut, double maxOut) {
        return minOut + (maxOut - minOut) * (x - minIn) / (maxIn - minIn);
    }

    public static float MapInterval(float x, float minIn, float maxIn,
            float minOut, float maxOut) {
        return minOut + (maxOut - minOut) * (x - minIn) / (maxIn - minIn);
    }

    public static  double Max(double a, double b) {
        return a > b ? a : b;
    }

    public static  double Max(double a, double b, double c) {
        return (a > b) ? ((a > c) ? a : c) : ((b > c) ? b : c);
    }

    public static  double Max(double[] values) {
        return Max(values[0], values[1], values[2]);
    }

    public static  float Max(float a, float b) {
        return a > b ? a : b;
    }

    /**
     * Returns the maximum value of three floats.
     * 
     * @param a
     * @param b
     * @param c
     * @return max val
     */
    public static  float Max(float a, float b, float c) {
        return (a > b) ? ((a > c) ? a : c) : ((b > c) ? b : c);
    }

    public static  float Max(float[] values) {
        return Max(values[0], values[1], values[2]);
    }

    public static  int Max(int a, int b) {
        return a > b ? a : b;
    }

    /**
     * Returns the maximum value of three ints.
     * 
     * @param a
     * @param b
     * @param c
     * @return max val
     */
    public static  int Max(int a, int b, int c) {
        return (a > b) ? ((a > c) ? a : c) : ((b > c) ? b : c);
    }

    public static  int Max(int[] values) {
        return Max(values[0], values[1], values[2]);
    }

    public static  double Min(double a, double b) {
        return a < b ? a : b;
    }

    public static  double Min(double a, double b, double c) {
        return (a < b) ? ((a < c) ? a : c) : ((b < c) ? b : c);
    }

    public static  float Min(float a, float b) {
        return a < b ? a : b;
    }

    /**
     * Returns the minimum value of three floats.
     * 
     * @param a
     * @param b
     * @param c
     * @return min val
     */
    public static  float Min(float a, float b, float c) {
        return (a < b) ? ((a < c) ? a : c) : ((b < c) ? b : c);
    }

    public static  int Min(int a, int b) {
        return a < b ? a : b;
    }

    /**
     * Returns the minimum value of three ints.
     * 
     * @param a
     * @param b
     * @param c
     * @return min val
     */
    public static  int Min(int a, int b, int c) {
        return (a < b) ? ((a < c) ? a : c) : ((b < c) ? b : c);
    }

    /**
     * Returns a random number in the interval -1 .. +1.
     * 
     * @return random float
     */
    public static  float NormalizedRandom() {
        return RND.Next() * 2 - 1;
    }

    /**
     * Returns a random number in the interval -1 .. +1 uSing the {@link Random}
     * instance provided.
     * 
     * @return random float
     */
    public static  float NormalizedRandom(Random rnd) {
        return rnd.Next() * 2 - 1;
    }

    public static double Radians(double degrees) {
        return degrees * DEG2RAD;
    }

    public static  float Radians(float degrees) {
        return degrees * DEG2RAD;
    }

    public static  float Random(float max) {
        return RND.Next() * max;
    }

    public static  float Random(float min, float max) {
        return RND.Next() * (max - min) + min;
    }

    public static  int Random(int max) {
        return (int) (RND.Next() * max);
    }

    public static  int Random(int min, int max) {
        return (int) ((RND.Next() * (float)(max - min)) + min);
    }

    public static  double Random(Random rnd, double max) {
        return rnd.NextDouble() * max;
    }

    public static  double Random(Random rnd, double min, double max) {
        return rnd.NextDouble() * (max - min) + min;
    }

    public static  float Random(Random rnd, float max) {
        return rnd.Next() * max;
    }

    public static  float Random(Random rnd, float min, float max) {
        return rnd.Next() * (max - min) + min;
    }

    public static  int Random(Random rnd, int max) {
        return (int) (rnd.NextDouble() * max);
    }

    public static  int Random(Random rnd, int min, int max) {
        return (int) (rnd.NextDouble() * (max - min)) + min;
    }

    public static  bool RandomChance(double chance) {
        return RND.NextDouble() < chance;
    }

    public static  bool RandomChance(float chance) {
        return RND.Next() < chance;
    }

    public static  bool RandomChance(Random rnd, double chance) {
        return rnd.NextDouble() < chance;
    }

    public static  bool RandomChance(Random rnd, float chance) {
        return rnd.Next() < chance;
    }

    public static  double ReduceAngle(double theta) {
        theta %= TWO_PI;
        if (Abs(theta) > PI) {
            theta = theta - TWO_PI;
        }
        if (Abs(theta) > HALF_PI) {
            theta = PI - theta;
        }
        return theta;
    }

    /**
     * Reduces the given angle into the -PI/4 ... PI/4 interval for faster
     * computation of Sin/Cos. This method is used by {@link #Sin(float)} &
     * {@link #Cos(float)}.
     * 
     * @param theta
     *            angle in radians
     * @return reduced angle
     * @see #Sin(float)
     * @see #Cos(float)
     */
    public static  float ReduceAngle(float theta) {
        theta %= TWO_PI;
        if (Abs(theta) > PI) {
            theta = theta - TWO_PI;
        }
        if (Abs(theta) > HALF_PI) {
            theta = PI - theta;
        }
        return theta;
    }

    /**
     * Rounds a double precision value to the given precision.
     * 
     * @param val
     * @param prec
     * @return rounded value
     */
    public static  double RoundTo(double val, double prec) {
        return Floor(val / prec + 0.5) * prec;
    }

    /**
     * Rounds a Single precision value to the given precision.
     * 
     * @param val
     * @param prec
     * @return rounded value
     */
    public static  float RoundTo(float val, float prec) {
        return Floor(val / prec + 0.5f) * prec;
    }

    /**
     * Rounds an integer value to the given precision.
     * 
     * @param val
     * @param prec
     * @return rounded value
     */
    public static  int RoundTo(int val, int prec) {
        return Floor((float) val / prec + 0.5f) * prec;
    }

    /**
     * Sets the default Random number generator for this class. This generator
     * is being reused by all future calls to random() method versions which
     * don't explicitly ask for a {@link Random} instance to be used.
     * 
     * @param rnd
     */
    public static void SetDefaultRandomGenerator(Random rnd) {
        RND = rnd;
    }

    public static int Sign(double x) {
        return x < 0 ? -1 : (x > 0 ? 1 : 0);
    }

    public static int Sign(float x) {
        return x < 0 ? -1 : (x > 0 ? 1 : 0);
    }

    public static int Sign(int x) {
        return x < 0 ? -1 : (x > 0 ? 1 : 0);
    }

    public static  double Sin(double theta) {
        theta = ReduceAngle(theta);
        if (Abs(theta) <= QUARTER_PI) {
            return (float) FastSin(theta);
        }
        return (float) FastCos(HALF_PI - theta);
    }

    /**
     * Returns a fast Sine approximation of a value. Note: code from <a
     * href="http://wiki.java.net/bin/view/Games/JeffGems">wiki posting on
     * java.net by jeffpk</a>
     * 
     * @param theta
     *            angle in radians.
     * @return Sine of theta.
     */
    public static  float Sin(float theta) {
        theta = ReduceAngle(theta);
        if (Abs(theta) <= QUARTER_PI) {
            return (float) FastSin(theta);
        }
        return (float) FastCos(HALF_PI - theta);
    }

    // /**
    //  * @deprecated
    //  */
    // public static float Sqrt(float x) {
    //     x = FastInverseSqrt(x);
    //     if (x > 0) {
    //         return 1.0f / x;
    //     } else {
    //         return 0;
    //     }
    // }
    // 
    }
}