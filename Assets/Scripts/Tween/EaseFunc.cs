using UnityEngine;
namespace Framework
{

	public class EaseFunc
	{
		const float M_PI_X_2 = Mathf.PI * 2f;
		const float M_PI_2 = Mathf.PI / 2f;
		const float M_PI = Mathf.PI;
		// Linear
		public static float Linear(float time)
		{
			return time;
		}


		// Sine Ease
		public static float SineEaseIn(float time)
		{
			return -1 * Mathf.Cos(time * (float)M_PI_2) + 1;
		}

		public static float SineEaseOut(float time)
		{
			return Mathf.Sin(time * (float)M_PI_2);
		}

		public static float SineEaseInOut(float time)
		{
			return -0.5f * (Mathf.Cos((float)M_PI * time) - 1);
		}


		// Quad Ease
		public static float QuadEaseIn(float time)
		{
			return time * time;
		}

		public static float QuadEaseOut(float time)
		{
			return -1 * time * (time - 2);
		}

		public static float QuadEaseInOut(float time)
		{
			time = time * 2;
			if (time < 1)
				return 0.5f * time * time;
			--time;
			return -0.5f * (time * (time - 2) - 1);
		}



		// Cubic Ease
		public static float CubicEaseIn(float time)
		{
			return time * time * time;
		}
		public static float CubicEaseOut(float time)
		{
			time -= 1;
			return (time * time * time + 1);
		}
		public static float CubicEaseInOut(float time)
		{
			time = time * 2;
			if (time < 1)
				return 0.5f * time * time * time;
			time -= 2;
			return 0.5f * (time * time * time + 2);
		}


		// Quart Ease
		public static float QuartEaseIn(float time)
		{
			return time * time * time * time;
		}

		public static float QuartEaseOut(float time)
		{
			time -= 1;
			return -(time * time * time * time - 1);
		}

		public static float QuartEaseInOut(float time)
		{
			time = time * 2;
			if (time < 1)
				return 0.5f * time * time * time * time;
			time -= 2;
			return -0.5f * (time * time * time * time - 2);
		}


		// Quint Ease
		public static float QuintEaseIn(float time)
		{
			return time * time * time * time * time;
		}

		public static float QuintEaseOut(float time)
		{
			time -= 1;
			return (time * time * time * time * time + 1);
		}

		public static float QuintEaseInOut(float time)
		{
			time = time * 2;
			if (time < 1)
				return 0.5f * time * time * time * time * time;
			time -= 2;
			return 0.5f * (time * time * time * time * time + 2);
		}


		// Expo Ease
		public static float ExpoEaseIn(float time)
		{
			return time == 0 ? 0 : Mathf.Pow(2, 10 * (time / 1 - 1)) - 1 * 0.001f;
		}
		public static float ExpoEaseOut(float time)
		{
			return time == 1 ? 1 : (-Mathf.Pow(2, -10 * time / 1) + 1);
		}
		public static float ExpoEaseInOut(float time)
		{
			time /= 0.5f;
			if (time < 1)
			{
				time = 0.5f * Mathf.Pow(2, 10 * (time - 1));
			}
			else
			{
				time = 0.5f * (-Mathf.Pow(2, -10 * (time - 1)) + 2);
			}

			return time;
		}


		// Circ Ease
		public static float CircEaseIn(float time)
		{
			return -1 * (Mathf.Sqrt(1 - time * time) - 1);
		}
		public static float CircEaseOut(float time)
		{
			time = time - 1;
			return Mathf.Sqrt(1 - time * time);
		}
		public static float CircEaseInOut(float time)
		{
			time = time * 2;
			if (time < 1)
				return -0.5f * (Mathf.Sqrt(1 - time * time) - 1);
			time -= 2;
			return 0.5f * (Mathf.Sqrt(1 - time * time) + 1);
		}


		// Elastic Ease
		public static float ElasticEaseIn(float time, float period)
		{

			float newT = 0;
			if (time == 0 || time == 1)
			{
				newT = time;
			}
			else
			{
				float s = period / 4;
				time = time - 1;
				newT = -Mathf.Pow(2, 10 * time) * Mathf.Sin((time - s) * M_PI_X_2 / period);
			}

			return newT;
		}
		public static float ElasticEaseOut(float time, float period)
		{

			float newT = 0;
			if (time == 0 || time == 1)
			{
				newT = time;
			}
			else
			{
				float s = period / 4;
				newT = Mathf.Pow(2, -10 * time) * Mathf.Sin((time - s) * M_PI_X_2 / period) + 1;
			}

			return newT;
		}
		public static float ElasticEaseInOut(float time, float period)
		{

			float newT = 0;
			if (time == 0 || time == 1)
			{
				newT = time;
			}
			else
			{
				time = time * 2;
				if (period == 0f)
				{
					period = 0.3f * 1.5f;
				}

				float s = period / 4;

				time = time - 1;
				if (time < 0)
				{
					newT = -0.5f * Mathf.Pow(2, 10 * time) * Mathf.Sin((time - s) * M_PI_X_2 / period);
				}
				else
				{
					newT = Mathf.Pow(2, -10 * time) * Mathf.Sin((time - s) * M_PI_X_2 / period) * 0.5f + 1;
				}
			}
			return newT;
		}


		// Back Ease
		public static float BackEaseIn(float time)
		{
			float overshoot = 1.70158f;
			return time * time * ((overshoot + 1) * time - overshoot);
		}
		public static float BackEaseOut(float time)
		{
			float overshoot = 1.70158f;

			time = time - 1;
			return time * time * ((overshoot + 1) * time + overshoot) + 1;
		}
		public static float BackEaseInOut(float time)
		{
			float overshoot = 1.70158f * 1.525f;

			time = time * 2;
			if (time < 1)
			{
				return (time * time * ((overshoot + 1) * time - overshoot)) / 2;
			}
			else
			{
				time = time - 2;
				return (time * time * ((overshoot + 1) * time + overshoot)) / 2 + 1;
			}
		}



		// Bounce Ease
		public static float BounceTime(float time)
		{
			if (time < 1f / 2.75f)
			{
				return 7.5625f * time * time;
			}
			else if (time < 2f / 2.75f)
			{
				time -= 1.5f / 2.75f;
				return 7.5625f * time * time + 0.75f;
			}
			else if (time < 2.5f / 2.75f)
			{
				time -= 2.25f / 2.75f;
				return 7.5625f * time * time + 0.9375f;
			}

			time -= 2.625f / 2.75f;
			return 7.5625f * time * time + 0.984375f;
		}
		public static float BounceEaseIn(float time)
		{
			return 1 - BounceTime(1 - time);
		}

		public static float BounceEaseOut(float time)
		{
			return BounceTime(time);
		}

		public static float BounceEaseInOut(float time)
		{
			float newT = 0;
			if (time < 0.5f)
			{
				time = time * 2;
				newT = (1 - BounceTime(1 - time)) * 0.5f;
			}
			else
			{
				newT = BounceTime(time * 2 - 1) * 0.5f + 0.5f;
			}

			return newT;
		}


		public static float EaseIn(float time, float rate)
		{
			return Mathf.Pow(time, rate);
		}

		public static float EaseOut(float time, float rate)
		{
			return Mathf.Pow(time, 1 / rate);
		}

		public static float EaseInOut(float time, float rate)
		{
			time *= 2;
			if (time < 1)
			{
				return 0.5f * Mathf.Pow(time, rate);
			}
			else
			{
				return (1.0f - 0.5f * Mathf.Pow(2 - time, rate));
			}
		}

		public static float QuadraticIn(float time)
		{
			return Mathf.Pow(time, 2);
		}

		public static float QuadraticOut(float time)
		{
			return -time * (time - 2);
		}

		public static float QuadraticInOut(float time)
		{

			float resultTime = time;
			time = time * 2;
			if (time < 1)
			{
				resultTime = time * time * 0.5f;
			}
			else
			{
				--time;
				resultTime = -0.5f * (time * (time - 2) - 1);
			}
			return resultTime;
		}

	}

}