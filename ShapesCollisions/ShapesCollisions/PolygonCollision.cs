using System;
using System.Collections.Generic;

namespace ShapesCollisions
{
	public sealed class PolygonCollision : Collision
	{
		public List<VectorF> Vertex;
		public List<VectorF> Points;
		public float Rotation;

		public PolygonCollision(VectorF Position) : base(Position)
		{
			Vertex = new List<VectorF>();
			Points = new List<VectorF>();
			Rotation = 0.0f;
		}

		public override MacroCollisionRect GetMacroCollisionRect()
		{
			float minX, minY, maxX, maxY;
			minX = minY = float.PositiveInfinity;
			maxX = maxY = float.NegativeInfinity;
			for (int i = 0; i < Points.Count; i++)
			{
				if (Points[i].X < minX)
					minX = Points[i].X;
				if (Points[i].X > maxX)
					maxX = Points[i].X;
				if (Points[i].Y < minY)
					minY = Points[i].Y;
				if (Points[i].Y > maxY)
					maxY = Points[i].Y;
			}
			return new MacroCollisionRect(Position + new VectorF(minX, minY), new VectorF(maxX - minX, maxY - minY));
		}

		public override bool IsCollision(Collision collision)
		{
			if (GetMacroCollisionRect().IsMacroCollision(collision.GetMacroCollisionRect()) == false) return false;

			if (collision.GetType() == typeof(PolygonCollision))
			{
				PolygonCollision poly = (PolygonCollision)collision;

				for(int i = 0, p = Points.Count - 1; i < Points.Count; i++)
				{
					for (int j = 0, q = poly.Points.Count - 1; j < poly.Points.Count; j++)
					{
						if (SimpleGeometry.IsIntersection(Points[i] + Position, Points[p] + Position, poly.Points[j] + poly.Position, poly.Points[q] + poly.Position))
							return true;
						q = j;
					}
					p = i;
				}
				
				return IsPointInPoly(poly.Points[0] + poly.Position) || poly.IsPointInPoly(Points[0] + Position);
			}
			else if (collision.GetType() == typeof(BoxCollision))
			{
				BoxCollision box = (BoxCollision)collision;

				for (int i = 0, j = Points.Count - 1; i < Points.Count; i++)
				{
					if (SimpleGeometry.IsIntersection(Points[i] + Position, Points[j] + Position, box.Position, box.Position + new VectorF(box.Size.X, 0)) ||
						SimpleGeometry.IsIntersection(Points[i] + Position, Points[j] + Position, box.Position + new VectorF(box.Size.X, 0), box.Position + box.Size) ||
						SimpleGeometry.IsIntersection(Points[i] + Position, Points[j] + Position, box.Position + box.Size, box.Position + new VectorF(0, box.Size.Y)) ||
						SimpleGeometry.IsIntersection(Points[i] + Position, Points[j] + Position, box.Position + new VectorF(0, box.Size.Y), box.Position)) 
						return true;
					j = i;
				}

				return IsPointInPoly(box.Position) || (((Points[0].X + Position.X) >  box.Position.X) && ((Points[0].X + Position.X) < (box.Position.X + box.Size.X))
					 && ((Points[0].Y + Position.Y) > box.Position.Y) && ((Points[0].Y + Position.Y) < (box.Position.Y + box.Size.Y)));
			}
			else if (collision.GetType() == typeof(CircleCollision))
			{
				CircleCollision circle = (CircleCollision)collision;

				for (int i = 0, j = Points.Count - 1; i < Points.Count; i++)
				{
					if (SimpleGeometry.IsSegmentIntersectsWithCircle(circle.Center(), circle.Radius, Points[i] + Position, Points[j] + Position))
						return true;

					j = i;
				}
				
				return IsPointInPoly(circle.Center()) || ((Points[0] + Position - circle.Center()).Length() < circle.Radius);
			}
			else
			{
				return collision.IsCollision(this);
			}
		}

		public override void Update(List<Collision> collisions)
        {
            if (Vertex == null || collisions == null) return;

            for(int  i = 0; i < collisions.Count; i++)
            {
				if ((collisions[i] == null) || ReferenceEquals(collisions[i], this)) continue;

				if (IsCollision(collisions[i])) InvokeOnCollisionEvent(this, collisions[i]);
			}
		}

		public void SetUp()
		{
			Points = new List<VectorF>();
			for (int i = 0; i < Vertex.Count; i++)
			{
				float l = (float)Math.Sqrt(Vertex[i].X * Vertex[i].X + Vertex[i].Y * Vertex[i].Y);
				float a = (float)Math.Acos(Vertex[i].X / l);
				if (Vertex[i].Y < 0)
					a *= -1;
				a += Rotation;
				Points.Add(new VectorF(l * (float)Math.Cos(a), l * (float)Math.Sin(a)));
			}
		}

		public bool IsPointInPoly(VectorF point)
		{
			int i, j;
			bool c = false;

			for (i = 0, j = Points.Count - 1; i < Points.Count; i++)
			{
				switch (SimpleGeometry.GetEdgeType(point, Points[i] + Position,Points[j] + Position))
				{
					case SimpleGeometry.EdgeType.TOUCHING:
						return true;
					case SimpleGeometry.EdgeType.CROSSING:
						c = !c;
						break;
				}
				j = i;
			}

			return c;
		}
	}
}
