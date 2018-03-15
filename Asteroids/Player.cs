using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Asteroids
{
	class Player
	{
		public Texture2D ActiveTexture; //make it easier to change the texture for the player
		public Texture2D PlayerTexture;
		public Vector2 Position;
		public Vector2 Velocity;
		public Vector2 Origin;
		public float Acceleration;
		public float Deceleration;
		public float Rotation;
		public float RotationSpeed;
        public float MaxRotationSpeed;
		public float RotationAcceleration;
		public float RotationDecay;
		public bool Active;
		public bool Alive;
		public float MaxX;
		public float MaxY;
		public float Scale;


		public int Width
		{
			get { return PlayerTexture.Width; }
		}

		public int Height
		{
			get { return PlayerTexture.Height; }
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

		public void Initialize(Texture2D texture, Vector2 position, Vector2 Max)
		{
			PlayerTexture = texture; 
			ActiveTexture = PlayerTexture; //set the active texture to playerTexture
			Position = position;
			Active = true;
			Alive = true;
			Origin = new Vector2(Height / 2, Width / 2); //set the origin to the center of the texture
			Acceleration = 2; //speed at which the ship increases in speed
			Deceleration = 1.01F; //how much speed decays 
			RotationAcceleration = 6F; //how quickly the rotation speed increases
            MaxRotationSpeed = 500f; //cap out the rotation speed
			RotationDecay = 20F; //how quickly rotation comes to a stop
			MaxX = Max.X;
			MaxY = Max.Y;
			Scale = 0.1f; //multiplier for texture size to determine the size of the player
		}


		public void Update(GameTime gameTime)
		{
			Input(Keyboard.GetState(), gameTime);
            //set roation and position based on velocity and deltaTime since last update
			float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
			Position += Velocity * deltaTime;
			Rotation += RotationSpeed * deltaTime;
            //cap the rotation speed
            if (RotationSpeed > MaxRotationSpeed) { RotationSpeed = MaxRotationSpeed; }
            if (RotationSpeed < -MaxRotationSpeed) { RotationSpeed = -MaxRotationSpeed; }

            //loop X and Y when player moves off screen
			if (Position.X > MaxX) { Position.X = 0; }
			if (Position.X < 0) { Position.X = MaxX; }
			if (Position.Y > MaxY) { Position.Y = 0; }
			if (Position.Y < 0) { Position.Y = MaxY; }
		}

		public void Input(KeyboardState Keyboard, GameTime gameTime)
		{

			//input

            //Move forward
			if (Keyboard.IsKeyDown(Keys.W))
			{
				//increase speed
				Velocity.X += ((float)Math.Cos(Rotation * Math.PI / 180) * Acceleration);
				Velocity.Y += ((float)Math.Sin(Rotation * Math.PI / 180) * Acceleration);
			}
			else
			{
                //decrease speed if not accelerating forward
				Velocity.X = (Math.Abs(Velocity.X) <= 0.1) ? 0 : (Velocity.X / Deceleration);
				Velocity.Y = (Math.Abs(Velocity.Y) <= 0.1) ? 0 : (Velocity.Y / Deceleration);
			}

			if (Keyboard.IsKeyDown(Keys.A))
			{
				//rotate left
				RotationSpeed -= RotationAcceleration;
			}
			else if (Keyboard.IsKeyDown(Keys.D))
			{
				//rotate right
				RotationSpeed += RotationAcceleration;
			}
			else
			{
                //Decrease rotation if not accelerating in either direction
				RotationSpeed -= (Math.Abs(RotationSpeed) <= RotationDecay) ? RotationSpeed : Math.Sign(RotationSpeed) * RotationDecay;
			}
			
            //Allow the player to slow down faster manually
            /*if (Keyboard.IsKeyDown(Keys.S))
			{
				Velocity.X = (Math.Abs(Velocity.X) <= 0.1) ? 0 : (Velocity.X / Deceleration);
				Velocity.Y = (Math.Abs(Velocity.Y) <= 0.1) ? 0 : (Velocity.Y / Deceleration);
			}*/
			//end input
		}

		public void Draw(SpriteBatch spriteBatch)
		{
            //draw an invisible hitbox only visible in wireframe mode for testing hitboxes
			spriteBatch.Draw(ActiveTexture, 
				HitBox, 
				Color.White * 0f);

			spriteBatch.Draw(
				ActiveTexture, //texture
				Position, //position
				null, //source rect
				Color.White, //color 
				(float)((Rotation + 90) * Math.PI / 180), //rotation
				Origin, //origin
				Scale, //scale
				SpriteEffects.None, //sprite effects
				0f //depth
				);
		}
	}
}
