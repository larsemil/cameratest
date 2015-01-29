#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;

#endregion

namespace CameraTest
{
	public class Camera
	{
		public int width; 
		public int height; 

		float xSpeed;
		float ySpeed; 

		public Vector2 position; 

		public Camera ()
		{
			position = new Vector2 (0, 0); 
			width = 12; 
			height = 6;

			xSpeed = 0; 
			ySpeed = 0; 

		}

		public void Update()
		{

			KeyboardState newState = Keyboard.GetState (); 

			if (newState.IsKeyDown (Keys.Up)) {
				 
				ySpeed -= 0.5f; 
			}

			if (newState.IsKeyDown (Keys.Down)) {

				ySpeed += 0.5f; 
			}


			if (newState.IsKeyDown (Keys.Left)) {
				xSpeed -= 0.5f;

			}


			if (newState.IsKeyDown (Keys.Right)) {
				xSpeed += 0.5f; 


			}
			 
			position.X += xSpeed;
			position.Y += ySpeed; 
		}
	}
}

