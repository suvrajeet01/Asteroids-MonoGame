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
	/// <summary>
	/// This is the main type for your game.
	/// </summary>
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;

		//Textures
		Texture2D ship;
		Texture2D shot;
		Texture2D asteroid;
		Texture2D explosion;

		//Objects
		Player player;
		Asteroid rock;

		//Settings
		bool wireframe;

		//other
		KeyboardState oldState;
		KeyboardState newState;
		Vector2 Max;
		Queue<Shot> shots;

		public Game1()
		{
			graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			player = new Player();
			shots = new Queue<Shot>(); //list of shots
			oldState = Keyboard.GetState(); //check against newState to detect button presses
			wireframe = false; //wireframe rendering on or off to check hitboxes
			base.Initialize();
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);
            Max = new Vector2(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height); // maximum X,Y for objects
			Vector2 playerPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + GraphicsDevice.Viewport.TitleSafeArea.Width / 2,
				GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2); //Center the player on the screen
			
            //load textures
            ship = Content.Load<Texture2D>("ship");
			shot = Content.Load<Texture2D>("shot");
			asteroid = Content.Load<Texture2D>("asteroid");
			explosion = Content.Load<Texture2D>("explosion");


			rock = new Asteroid(asteroid, explosion, new Vector2(100, 100), Max);
			player.Initialize(ship, playerPosition, Max);
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// game-specific content.
		/// </summary>
		protected override void UnloadContent()
		{
			// TODO: Unload any non ContentManager content here
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			newState = Keyboard.GetState(); //newState to check against oldState

            //Keyboard inputs
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

            //wireframe toggle
			if (newState.IsKeyDown(Keys.I) && oldState.IsKeyUp(Keys.I))
			{
				wireframe = !wireframe;
				Console.WriteLine("wireframe toggled");
			}
            //respawn asteroid for testing
			if (newState.IsKeyDown(Keys.R) && oldState.IsKeyUp(Keys.R))
			{
				rock.Respawn();
			}
            //shoot a shot
			if (newState.IsKeyDown(Keys.Space) && oldState.IsKeyUp(Keys.Space) && shots.Count < 4)
			{
				shots.Enqueue(new Shot(shot, player.Position, player.Rotation, Max));
			}

            //check hitboxes
			if(rock.HitBox.Intersects(player.HitBox))
			{
				rock.Explode();
			}
			bool dequeue = false;
			
            //update each shot in Queue of shots
            foreach (Shot shot in shots)
			{
				if (shot.sw.ElapsedMilliseconds > 1000)
				{
					dequeue = true;
				}
				shot.Update(gameTime);
                //check shot hitboxes
				if (rock.HitBox.Contains(shot.Position))
				{
					dequeue = true;
					rock.Explode();
				}
			}
            if (dequeue) shots.Dequeue(); //

            //update other objects
            player.Update(gameTime);
            rock.Update(gameTime);
			oldState = newState;
			base.Update(gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			//set rendering mode to wireframe if enabled
            if(wireframe)
			{
				GraphicsDevice.Clear(Color.CornflowerBlue);
				spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque);
				RasterizerState state = new RasterizerState
				{
					FillMode = FillMode.WireFrame
				};
				spriteBatch.GraphicsDevice.RasterizerState = state;
			}
			else {
				GraphicsDevice.Clear(Color.Black);
				spriteBatch.Begin();
            }

            //draw objects by calling their draw functions
			rock.Draw(spriteBatch);
			foreach (Shot shot in shots)
			{
				shot.Draw(spriteBatch);
			}
			player.Draw(spriteBatch);

			spriteBatch.End();

			base.Draw(gameTime);
		}
	}
}
