using System;
using System.Collections.Generic;


namespace ShapesCollisions
{
    public sealed class BoxCollision : Collision
    {
        public VectorF Size;

        public BoxCollision(VectorF Position, VectorF Size) : base(Position)
        {
            this.Size = Size;
        }

		public override MacroCollisionRect GetMacroCollisionRect()
		{
			return new MacroCollisionRect(Position, Size);
		}

		public override bool IsCollision(Collision collision)
		{
			if (collision.GetType() == typeof(BoxCollision))
			{
				return GetMacroCollisionRect().IsMacroCollision(collision.GetMacroCollisionRect());
			}
			else if (collision.GetType() == typeof(CircleCollision))
			{
				CircleCollision circle = (CircleCollision)collision;

				float cX = circle.Center().X;
				float cY = circle.Center().Y;
				if (cX < Position.X)
					cX = Position.X;
				else if (cX > (Position.X + Size.X))
					cX = Position.X + Size.X;
				if (cY < Position.Y)
					cY = Position.Y;
				else if (cY > (Position.Y + Size.Y))
					cY = Position.Y + Size.Y;
				if ((circle.Center() - new VectorF(cX, cY)).Length() < circle.Radius) return true;
				return false;
			}
			else
			{
				return collision.IsCollision(this);
			}
		}

		public override void Update(List<Collision> collisions)
        {
            if (Size.X < 0 || Size.Y < 0 || collisions == null) return;

            for(int i = 0; i < collisions.Count; i++)
            {
				if ((collisions[i] == null) || ReferenceEquals(collisions[i], this)) continue;

				if (IsCollision(collisions[i])) InvokeOnCollisionEvent(this, collisions[i]);
            }
        }
	}
}
