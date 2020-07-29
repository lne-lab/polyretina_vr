using UnityEngine;

namespace LNE.ProstheticVision
{
	public static class AxonModel
	{
		/*
		 * Public fields
		 */

		public static float RhoMin { get => 4; }
		public static float RhoMax { get => 45; }

		/*
		 * Public methods
		 */

		public static float CalculatePhi(float phi0, float rho)
		{
			return CalculatePhi(phi0, rho, CalculateB(phi0), CalculateC(phi0));
		}

		public static float CalculatePhi(float phi0, float rho, float b, float c)
		{
			return phi0 + b * Mathf.Pow(rho - RhoMin, c);
		}

		public static float CalculateB(float phi0)
		{
			float lnB;

			if (IsSuperior(phi0))
			{
				lnB = -1.9f + 3.9f * AuxMath.Tanh(-(phi0 - 121) / 14);
			}
			else
			{
				lnB = .5f + 1.5f * AuxMath.Tanh(-(-phi0 - 90) / 25);
			}

			var b = Mathf.Exp(lnB);

			if (IsInferior(phi0))
			{
				b = -b;
			}

			return b;
		}

		public static float CalculateC(float phi0)
		{
			var c = 0f;
			if (IsSuperior(phi0))
			{
				c = 1.9f + 1.4f * AuxMath.Tanh((phi0 - 121) / 14);
			}
			else
			{
				c = 1.0f + .5f * AuxMath.Tanh((-phi0 - 90) / 25);
			}

			return c;
		}

		public static bool IsSuperior(float phi)
		{
			return phi >= 0 && phi <= 180f;
		}

		public static bool IsInferior(float phi)
		{
			return phi < 0 || phi > 180f;
		}
	}
}
