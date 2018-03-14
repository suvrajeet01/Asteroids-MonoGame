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
	class Shot
	{
		public Texture2D ShotTexture;
		public Vector2 Position;
		public Vector2 Velocity;
		public float Speed;
		public float Rotation;
		public Vector2 Origin;
		public Stopwatch sw;
		public float MaxX;
		public float MaxY;
		public int Width
		{
			get { return ShotTexture.Width; }
		}

		public int Height
		{
			get { return ShotTexture.Height; }
		}

		public Shot(Texture2D texture, Vector2 position, float rotation, Vector2 Max)
		{
			ShotTexture = texture;
			Position = position;
			Rotation = rotation;
			sw = new Stopwatch();
			sw.Start();
			Speed = 500F;
			Origin = new Vector2(Width/2, Height);
			Velocity.X = ((float)Math.Cos(Rotation * Math.PI / 180) * Speed);
			Velocity.Y = ((float)Math.Sin(Rotation * Math.PI / 180) * Speed);
			MaxX = Max.X;
			MaxY = Max.Y;
		}

		public void Update(GameTime gameTime)
		{
			float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
			Position += Velocity * deltaTime;
			if (Position.X > MaxX) { Position.X = 0; }
			if (Position.X < 0) { Position.X = MaxX; }
			if (Position.Y > MaxY) { Position.Y = 0; }
			if (Position.Y < 0) { Position.Y = MaxY; }
		}

		public void Draw(SpriteBatch spriteBatch)
		{
			spriteBatch.Draw(ShotTexture, Position, null, Color.White, (float)((Rotation + 90) * Math.PI / 180), Origin, 0.05f, SpriteEffects.None, 0f);
		}
	}
}
