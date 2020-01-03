using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesCollisions
{
	class SimpleGeometry
	{
		public enum PointPosition
		{
			LEFT,
			RIGHT,
			FRONT,
			BEHIND,
			BETWEEN,
			ORIGIN,
			DESTINATION
		}

		public enum EdgeType
		{
			TOUCHING,
			CROSSING,
			INESSENTIAL
		}

		public static EdgeType GetEdgeType(VectorF point, VectorF s, VectorF e)
		{
			switch (Classify(s, e, point))
			{
				case PointPosition.LEFT:
					return ((s.Y < point.Y) && (point.Y <= e.Y)) ? EdgeType.CROSSING : EdgeType.INESSENTIAL;
				case PointPosition.RIGHT:
					return ((e.Y < point.Y) && (point.Y <= s.Y)) ? EdgeType.CROSSING : EdgeType.INESSENTIAL;
				case PointPosition.BETWEEN:
				case PointPosition.ORIGIN:
				case PointPosition.DESTINATION:
					return EdgeType.TOUCHING;
				default:
					return EdgeType.INESSENTIAL;
			}
		}

		public static PointPosition Classify(VectorF s, VectorF e, VectorF p)
		{
			VectorF a = e - s;
			VectorF b = p - s;
			double d = a.X * b.Y - b.X * a.Y;
			if (d > 0.0)
				return PointPosition.LEFT;
			if (d < 0.0)
				return PointPosition.RIGHT;
			if ((a.X * b.X < 0.0) || (a.Y * b.Y < 0.0))
				return PointPosition.BEHIND;
			if (a.Length() < b.Length())
				return PointPosition.FRONT;
			if (p == s)
				return PointPosition.ORIGIN;
			if (p == e)
				return PointPosition.DESTINATION;
			return PointPosition.BETWEEN;
		}

		public static bool IsIntersection(VectorF A, VectorF B, VectorF C, VectorF D)
		{
			float k1 = (B.Y - A.Y) / (B.X - A.X);
			float b1 = A.Y - A.X * k1;
			float k2 = (D.Y - C.Y) / (D.X - C.X);
			float b2 = C.Y - C.X * k2;

			if ((Math.Abs(B.X - A.X) < float.Epsilon) && (Math.Abs(D.X - C.X) < float.Epsilon))
				return (Math.Abs(A.X - C.X) < float.Epsilon) && (Math.Max(C.Y, D.Y) > Math.Min(A.Y, B.Y)) && (Math.Max(A.Y, B.Y) > Math.Min(C.Y, D.Y));
			else if (Math.Abs(B.X - A.X) < float.Epsilon)
				return IsPointOnSegment(k2 * A.X + b2, A.Y, B.Y) && IsPointOnSegment(A.X, C.X, D.X) && IsPointOnSegment(k2 * A.X + b2, C.Y, D.Y);
			else if (Math.Abs(D.X - C.X) < float.Epsilon)
				return IsPointOnSegment(k1 * C.X + b1, A.Y, B.Y) && IsPointOnSegment(C.X, A.X, B.X) && IsPointOnSegment(k1 * C.X + b1, C.Y, D.Y);
			else if (Math.Abs(k1 - k2) < float.Epsilon)
				return (Math.Abs(b1 - b2) < float.Epsilon) && (Math.Max(C.X, D.X) > Math.Min(A.X, B.X)) && (Math.Max(A.X, B.X) > Math.Min(C.X, D.X));

			VectorF M = GetStraightsIntersectionPoint(k1, b1, k2, b2);
			if (IsPointOnSegment(M.X, A.X, B.X) && IsPointOnSegment(M.X, C.X, D.X))
				return true;
			return false;
		}

		public static VectorF GetStraightsIntersectionPoint(float k1, float b1, float k2, float b2)
		{
			float x = (b2 - b1) / (k1 - k2);
			return new VectorF(x, k1 * x + b1);
		}

		public static bool IsPointOnSegment(float p, float s, float e)
		{
			return ((p >= s && p <= e) || (p <= s && p >= e));
		}

		public static bool IsSegmentIntersectsWithCircle(VectorF O, float Radius, VectorF A, VectorF B)
		{
			VectorF p1, p2;
			if (IsLineIntersectsWithCircle(O, Radius, A, B, out p1, out p2) == false) return false;

			return (IsPointOnSegment(p1.X, A.X, B.X) && IsPointOnSegment(p1.Y, A.Y, B.Y)) ||
				   (IsPointOnSegment(p2.X, A.X, B.X) && IsPointOnSegment(p2.Y, A.Y, B.Y));
		}

		public static bool IsLineIntersectsWithCircle(VectorF O, float Radius, VectorF A, VectorF B, out VectorF p1, out VectorF p2)
		{
			if (Math.Abs(A.X - B.X) < float.Epsilon)
			{
				p1 = new VectorF(A.X, -(float)Math.Sqrt(Radius * Radius - Math.Pow(A.X - O.X, 2)) + O.Y);
				p2 = new VectorF(A.X, (float)Math.Sqrt(Radius * Radius - Math.Pow(A.X - O.X, 2)) + O.Y);
				return Math.Abs(O.X - A.X) < Radius;
			}
			if (Math.Abs(A.Y - B.Y) < float.Epsilon)
			{
				p1 = new VectorF(-(float)Math.Sqrt(Radius * Radius - Math.Pow(A.Y - O.Y, 2)) + O.X, A.Y);
				p2 = new VectorF((float)Math.Sqrt(Radius * Radius - Math.Pow(A.Y - O.Y, 2)) + O.X, A.Y);
				return Math.Abs(O.Y - A.Y) < Radius;
			}

			float k = (B.Y - A.Y) / (B.X - A.X);
			float b = (A.Y - O.Y) - (A.X - O.X) * k;
			float D = 4 * b * b * k * k - 4 * (k * k + 1) * (b * b - Radius * Radius);
			if (D >= 0)
			{
				float x1 = (-2 * b * k - (float)Math.Sqrt(D)) / (2 * (k * k + 1)) + O.X;
				float x2 = (-2 * b * k + (float)Math.Sqrt(D)) / (2 * (k * k + 1)) + O.X;
				b = A.Y - A.X * k;
				p1 = new VectorF(x1, k * x1 + b);
				p2 = new VectorF(x2, k * x2 + b);
				return true;
			}

			p1 = p2 = new VectorF(float.NaN, float.NaN);
			return false;
		}
	}
}
