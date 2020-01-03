using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFML.Window;
using SFML.Graphics;
using SFML.System;
using System.Threading;
using ShapesCollisions;

namespace WindowsFormsApp1
{
    static class Program
    {
        private static GameObject player;
		private static List<GameObject> objects;
		private static Random Rand;

		static void Main()
        {
            RenderWindow window = new RenderWindow(new VideoMode(1000, 1000), "shapes collisions");
			View view = new View(window.GetView());
            window.Closed += Window_Closed;
			Rand = new Random();

            objects = new List<GameObject>();
			objects.Add(new GameObject(new CircleCollision(new VectorF(200, 300), 50.0f)));
			objects.Add(new GameObject(new BoxCollision(new VectorF(100, 100), new VectorF(100, 100))));
			objects.Add(new GameObject(new BoxCollision(new VectorF(300, 100), new VectorF(200, 700))));
			objects.Add(new GameObject(new BoxCollision(new VectorF(30, 600), new VectorF(30, 30))));

			PolygonCollision poly = new PolygonCollision(new VectorF(750, 400));
			VectorF origin = new VectorF(-150, -150);
			poly.Vertex.Add(new VectorF(100, 100) + origin);
			poly.Vertex.Add(new VectorF(150, -50) + origin);
			poly.Vertex.Add(new VectorF(200, 100) + origin);
			poly.Vertex.Add(new VectorF(350, 150) + origin);
			poly.Vertex.Add(new VectorF(200, 200) + origin);
			poly.Vertex.Add(new VectorF(150, 350) + origin);
			poly.Vertex.Add(new VectorF(100, 200) + origin);
			poly.Vertex.Add(new VectorF(-50, 150) + origin);
			poly.SetUp();
			objects.Add(new GameObject(poly));


			PolygonCollision poly2 = new PolygonCollision(new VectorF(750, 700));
			VectorF origin2 = new VectorF(-150, -150);
			poly2.Vertex.Add(new VectorF(100, 100) + origin2);
			poly2.Vertex.Add(new VectorF(150, -50) + origin2);
			poly2.Vertex.Add(new VectorF(200, 100) + origin2);
			poly2.Vertex.Add(new VectorF(350, 150) + origin2);
			poly2.Vertex.Add(new VectorF(200, 200) + origin2);
			poly2.Vertex.Add(new VectorF(150, 350) + origin2);
			poly2.Vertex.Add(new VectorF(100, 200) + origin2);
			poly2.Vertex.Add(new VectorF(-50, 150) + origin2);
			poly2.SetUp();
			objects.Add(new GameObject(poly2));


			PolygonCollision poly3 = new PolygonCollision(new VectorF(50, 750));
			VectorF origin3 = new VectorF(0, 0);
			poly3.Vertex.Add(new VectorF(-50, -50) + origin3);
			poly3.Vertex.Add(new VectorF(-50, 50) + origin3);
			poly3.Vertex.Add(new VectorF(50, 50) + origin3);
			poly3.Vertex.Add(new VectorF(50, -50) + origin3);
			poly3.SetUp();
			objects.Add(new GameObject(poly3));

            float speed = 10.0f;
			SetPlayer();

			while (window.IsOpen)
            {
                Thread.Sleep(16);
                window.DispatchEvents();

				if (Keyboard.IsKeyPressed(Keyboard.Key.W))
                    player.collision.Position += new VectorF(0, -speed);
                if (Keyboard.IsKeyPressed(Keyboard.Key.S))
                    player.collision.Position += new VectorF(0, speed);
                if (Keyboard.IsKeyPressed(Keyboard.Key.A))
                    player.collision.Position += new VectorF(-speed, 0);
                if (Keyboard.IsKeyPressed(Keyboard.Key.D))
                    player.collision.Position += new VectorF(speed, 0);
				if (Keyboard.IsKeyPressed(Keyboard.Key.C))
				{
					SetPlayer();
				}
				if (Keyboard.IsKeyPressed(Keyboard.Key.R) && player.collision.GetType() == typeof(PolygonCollision))
				{
					((PolygonCollision)player.collision).Rotation -= 0.1f;
					((PolygonCollision)player.collision).SetUp();
				}
				if (Keyboard.IsKeyPressed(Keyboard.Key.T) && player.collision.GetType() == typeof(PolygonCollision))
				{
					((PolygonCollision)player.collision).Rotation += 0.1f;
					((PolygonCollision)player.collision).SetUp();
				}

				if (Keyboard.IsKeyPressed(Keyboard.Key.Up))
					view.Center += new Vector2f(0, -10);
				if (Keyboard.IsKeyPressed(Keyboard.Key.Down))
					view.Center += new Vector2f(0, 10);
				if (Keyboard.IsKeyPressed(Keyboard.Key.Left))
					view.Center += new Vector2f(-10, 0);
				if (Keyboard.IsKeyPressed(Keyboard.Key.Right))
					view.Center += new Vector2f(10, 0);
				window.SetView(view);

				window.Clear();
                for (int i = 0; i < objects.Count; i++)
                {
					objects[i].Update(objects);
                    window.Draw(objects[i]);
                }
				player.Update(objects);
				window.Draw(player);
				window.Display();
            }
        }

		private static void SetPlayer()
		{
			if (player != null)
			{
				player.cc = Color.White;
				player.ac = Color.Red;
				objects.Add(player);
			}
			player = objects[0];
			objects.Remove(player);
			player.cc = Color.Green;
			player.ac = Color.Cyan;
			Thread.Sleep(100);
		}

		private static void Window_Closed(object sender, EventArgs e)
        {
            ((RenderWindow)sender).Close();
        }
    }
}
