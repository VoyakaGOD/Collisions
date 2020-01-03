using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesCollisions
{
	public sealed class CircleCollision : Collision
	{
		public float Radius;

		public CircleCollision(VectorF Position, float Radius) : base(Position)
		{
			this.Radius = Radius;
		}

		public VectorF Center()
		{
			return Position + new VectorF(Radius, Radius);
		}

		public override MacroCollisionRect GetMacroCollisionRect()
		{
			return new MacroCollisionRect(Position, new VectorF(Radius * 2, Radius * 2));
		}

		public override bool IsCollision(Collision collision)
		{
			if (collision.GetType() == typeof(CircleCollision))
			{
				CircleCollision circle = (CircleCollision)collision;

				if ((circle.Position - Position).Length() <= (circle.Radius + Radius))
					return true;
				return false;
			}
			else
			{
				return collision.IsCollision(this);
			}
		}

		public override void Update(List<Collision> collisions)
		{
			if ((Radius <= 0) || (collisions == null)) return;

			for (int i = 0; i < collisions.Count; i++)
			{
				if ((collisions[i] == null) || ReferenceEquals(collisions[i], this)) continue;

				if (IsCollision(collisions[i])) InvokeOnCollisionEvent(this, collisions[i]);
			}
		}
	}
}
