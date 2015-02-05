#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;

#endregion

namespace CameraTest
{
	/// <summary>
	/// This is the main type for your game
	/// </summary>
	public class Game1 : Game
	{
		GraphicsDeviceManager graphics;
		SpriteBatch spriteBatch;
		Camera cam; 
		World tellus; 
		Player player; 
		myMouse mouse; 

		public Game1 ()
		{
			graphics = new GraphicsDeviceManager (this);
			Content.RootDirectory = "Content";	            
			graphics.IsFullScreen = true;
			graphics.PreferredBackBufferWidth = 1800;  // set this value to the desired width of your window
			graphics.PreferredBackBufferHeight = 1200;   // set this value to the desired height of your window
			graphics.ApplyChanges();
			graphics.ApplyChanges (); 
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize ()
		{
			// TODO: Add your initialization logic here
			base.Initialize ();
				
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent ()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch (GraphicsDevice);
			Texture2D tile_texture = Content.Load<Texture2D> ("tile"); 
			tellus = new World (tile_texture); 
			cam = new Camera (); 
			player = new Player (Content.Load<Texture2D>("fighter"), new Vector2 (1500, 2000), Keys.D, Keys.A, Keys.Space); 
			mouse = new myMouse (Content.Load<Texture2D> ("cross")); 

			//TODO: use this.Content to load your game content here 
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update (GameTime gameTime)
		{
			// For Mobile devices, this logic will close the Game when the Back button is pressed
			if (GamePad.GetState (PlayerIndex.One).Buttons.Back == ButtonState.Pressed) {
				Exit ();
			}

			cam.Update (player.position); 
			player.Update (cam, tellus); 
			mouse.Update (cam, tellus); 
			// TODO: Add your update logic here			
			base.Update (gameTime);
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw (GameTime gameTime)
		{
			graphics.GraphicsDevice.Clear (Color.Black);
		
			//TODO: Add your drawing code here
			spriteBatch.Begin (); 

			//draw the world, send the camera
			tellus.Draw (spriteBatch, cam); 

			player.Draw (spriteBatch,cam); 
			mouse.Draw (spriteBatch); 
			spriteBatch.End (); 
			base.Draw (gameTime);
		}
	}
}

