#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;

#endregion

namespace CameraTest
{
	public class myMouse
	{
		public Vector2 position;
		public Texture2D Texture; 

		private MouseState mousestate; 
		private MouseState laststate; 

		private int inRange; 

		public myMouse (Texture2D texture)
		{
			this.Texture = texture;
			inRange = 0; 
		}

		public void Update(Camera cam, World tellus, Player player)
		{
			laststate = mousestate;
			mousestate = Mouse.GetState(); 
			position.X = mousestate.X;
			position.Y = mousestate.Y; 
			int x = (Convert.ToInt32(cam.position.X + position.X)) / Settings.gridsize;
			int y = (Convert.ToInt32(cam.position.Y + position.Y)) / Settings.gridsize;

			double distanceToClick = Math.Sqrt ( (Math.Pow( ((x * Settings.gridsize) - player.position.X) , 2) + Math.Pow(( player.position.Y - (y*Settings.gridsize)),2)) );
			if (distanceToClick < (float)(Settings.gridsize * 2.5f)) {
				inRange = 1; 
				if ((mousestate.LeftButton == ButtonState.Pressed) && (laststate.LeftButton != ButtonState.Pressed)) {

					Console.WriteLine ("Mouse X" + (int)(x * Settings.gridsize) + " Player X" + (int)player.position.X); 

					try {

						Console.WriteLine (distanceToClick); 

						tellus.map [x, y].Clicked (); 
					} catch (IndexOutOfRangeException e) {
						return; 
					}
				}
			} else {
				inRange = 0; 
			}

		}

		public void Draw(SpriteBatch spriteBatch)
		{
			Vector2 drawPos = position;


			spriteBatch.Draw (Texture, drawPos, new Rectangle(0, inRange * (Texture.Height / 2), Texture.Width, Texture.Height / 2),Color.AliceBlue); 
		}
	}
}

