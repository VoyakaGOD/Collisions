using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShapesCollisions;
using SFML.Graphics;
using SFML.System;

namespace WindowsFormsApp1
{
	class GameObject : Drawable
	{
		public Collision collision;
		bool f;
		public Color cc;
		public Color ac;

		public GameObject(Collision collision)
		{
			this.collision = collision;
			this.collision.OnCollision += Collision_OnCollision;
			f = false;
			cc = Color.White;
			ac = Color.Red;
		}

		public void Draw(RenderTarget target, RenderStates states)
		{
			Shape shape;
			if (collision.GetType() == typeof(BoxCollision))
			{
				shape = new RectangleShape(new Vector2f(((BoxCollision)collision).Size.X, ((BoxCollision)collision).Size.Y));
			}
			else if (collision.GetType() == typeof(CircleCollision))
			{
				CircleCollision circle = (CircleCollision)collision;

				shape = new CircleShape(circle.Radius);
			}
			else if (collision.GetType() == typeof(PolygonCollision))
			{
				PolygonCollision poly = (PolygonCollision)collision;

				var cs = new ConvexShape((uint)poly.Vertex.Count());
				for (int i = 0; i < poly.Points.Count(); i++)
					cs.SetPoint((uint)i, new Vector2f(poly.Points[i].X, poly.Points[i].Y));
				shape = cs;
			}
			else return;
			shape.Position = new Vector2f(collision.Position.X, collision.Position.Y);
			if (f)
				shape.FillColor = ac;
			else
				shape.FillColor = cc;

			target.Draw(shape);
		}

		public void Update(List<GameObject> gameObjects)
		{
			f = false;
			List<Collision> collisions = new List<Collision>();
			for (int i = 0; i < gameObjects.Count; i++)
				collisions.Add(gameObjects[i].collision);
			collision.Update(collisions);
		}

		private void Collision_OnCollision(Collision currentCollision, Collision anotherCollision)
		{
			f = true;
		}
	}
}
