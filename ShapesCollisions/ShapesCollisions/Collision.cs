using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShapesCollisions
{
	public abstract class Collision
	{
		public delegate void OnCollisionE(Collision currentCollision, Collision anotherCollision);

		public VectorF Position;
		public event OnCollisionE OnCollision;

		public Collision(VectorF Position)
		{
			this.Position = Position;
		}

		public abstract void Update(List<Collision> collisions);

		public abstract bool IsCollision(Collision collision);

		public abstract MacroCollisionRect GetMacroCollisionRect();

		protected void InvokeOnCollisionEvent(Collision currentCollision, Collision anotherCollision)
		{
			OnCollision?.Invoke(currentCollision, anotherCollision);
		}
	}
}
