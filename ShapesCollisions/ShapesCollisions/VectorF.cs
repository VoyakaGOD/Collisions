 using System;

namespace ShapesCollisions
{
	public struct VectorF
	{
		public float X;
		public float Y;

		public VectorF(float X, float Y)
		{
			this.X = X;
			this.Y = Y;
		}

		public float SqrLength()
		{
			return (float)(Math.Pow(X, 2) + Math.Pow(Y, 2));
		}

		public float Length()
		{
			return (float)Math.Sqrt(SqrLength());
		}

		public bool Equals(VectorF vector)
		{
			return (Math.Abs(X - vector.X) < float.Epsilon) && (Math.Abs(Y - vector.Y) < float.Epsilon);
		}

		public VectorF Scale(float scale)
		{
			return new VectorF(X * scale, Y * scale);
		}

		public VectorF Normalized()
		{
			return Scale(1 / Length());
		}

		public bool IsNaN()
		{
			return (float.IsNaN(X) || float.IsNaN(Y));
		}

		public override string ToString()
		{
			return "{" + X + ";" + Y + "}";
		}

		public override int GetHashCode()
		{
			var hashCode = 1861411795;
			hashCode = hashCode * -1521134295 + X.GetHashCode();
			hashCode = hashCode * -1521134295 + Y.GetHashCode();
			return hashCode;
		}

		public override bool Equals(object obj)
		{
			if (obj.GetType() != GetType()) return false;

			return Equals((VectorF)obj);
		}

		public static float ScalarMultiply(VectorF first, VectorF second)
		{
			return (first.X * second.X + first.Y * second.Y);
		}

		public static float Cos(VectorF first, VectorF second)
		{
			if (first.Length() == 0 || second.Length() == 0) return 0;

			return (ScalarMultiply(first, second) / (first.Length() * second.Length()));
		}

		public static float AngleRad(VectorF first, VectorF second)
		{
			double angle1 = Math.Atan2(first.Y, first.X);
			if (angle1 < 0)
				angle1 += 2 * Math.PI;
			double angle2 = Math.Atan2(second.Y, second.X);
			if (angle2 < 0)
				angle2 += 2 * Math.PI;
			return (float)Math.Abs(angle1 - angle2);
		}

		public static float AngleDeg(VectorF first, VectorF second)
		{
			return (float)(AngleRad(first,second) * 180 / Math.PI);
		}

		#region operators
		public static VectorF operator -(VectorF right)
		{
			return right.Scale(-1);
		}

		public static VectorF operator +(VectorF left, VectorF right)
		{
			return new VectorF(left.X + right.X, left.Y + right.Y);
		}

		public static VectorF operator -(VectorF left, VectorF right)
		{
			return new VectorF(left.X - right.X, left.Y - right.Y);
		}

		public static VectorF operator *(VectorF left, float right)
		{
			return left.Scale(right);
		}

		public static VectorF operator *(float left, VectorF right)
		{
			return right.Scale(left);
		}

		public static float operator *(VectorF left, VectorF right)
		{
			return ScalarMultiply(left, right);
		}

		public static VectorF operator /(VectorF left, float right)
		{
			return left.Scale(1 / right);
		}

		public static bool operator ==(VectorF left, VectorF right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(VectorF left, VectorF right)
		{
			return !left.Equals(right);
		}
		#endregion
	}
}
