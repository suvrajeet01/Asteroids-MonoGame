using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids
{
	class Asteroid
	{
		public Texture2D AsteroidTexture;
		public Texture2D ExplosionTexture;
		public Texture2D ActiveTexture;
		public Vector2 Position;
		public Vector2 Velocity;
		public float Speed;
		public float Rotation;
		public Vector2 Origin;
		public float MaxX;
		public float MaxY;
		public float Scale;
		Stopwatch sw;
		public bool Active;
		public int Width
		{
			get { return AsteroidTexture.Width; }
		}

		public int Height
		{
			get { return AsteroidTexture.Height; }
		}

		public Rectangle HitBox
		{
			get
			{
				return new Rectangle(
					  (int)(Position.X - ((Width * Scale) / 2)),
					  (int)(Position.Y - ((Height * Scale) / 2)),
					  (int)(Width * Scale),
					  (int)(Height * Scale));
			}
		}

		public Asteroid(Texture2D rockTexture, Texture2D explosionTexture, Vector2 position, Vector2 Max)
		{
			AsteroidTexture = rockTexture;
			ExplosionTexture = explosionTexture;
			ActiveTexture = AsteroidTexture;
			Position = position;
			Origin = new Vector2(Width / 2, Height / 2);
			//Speed = 20F;
			Speed = 0f;
			Velocity.X = ((float)Math.Cos(Rotation * Math.PI / 180) * Speed);
			Velocity.Y = ((float)Math.Sin(Rotation * Math.PI / 180) * Speed);
			MaxX = Max.X;
			MaxY = Max.Y;
			Scale = 0.2f;
			sw = new Stopwatch();
			Active = true;
		}

		public void Update(GameTime gameTime)
		{
			Rotation += 1;
			float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
			Position += Velocity * deltaTime;
			if (Position.X > MaxX) { Position.X = 0; }
			if (Position.X < 0) { Position.X = MaxX; }
			if (Position.Y > MaxY) { Position.Y = 0; }
			if (Position.Y < 0) { Position.Y = MaxY; }
			if (sw.ElapsedMilliseconds >= 500) { Active = false; }
		}

		public void Explode()
		{
			sw.Start();
			ActiveTexture = ExplosionTexture;
		}

		public void Respawn()
		{
			sw.Reset();
			ActiveTexture = AsteroidTexture;
			Active = true;
			Console.WriteLine("Respawn asteroid");
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			if (Active)
			{
				spriteBatch.Draw(ActiveTexture, HitBox, Color.White * 0f);
				spriteBatch.Draw(ActiveTexture, Position, null, Color.White, (float)((Rotation + 90) * Math.PI / 180), Origin, Scale, SpriteEffects.None, 0f);
			}
		}
	}
}
