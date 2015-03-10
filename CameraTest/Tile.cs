#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;

#endregion


namespace CameraTest
{
	public class Tile
	{
		public bool isPassable; 
		public int type;
		public Vector2 position; 
		Texture2D texture;
		const int tileWidth = 64; 
		int numberOfTilesInTexture;

		int randTexture; 

		public Tile (int tileType, Vector2 position, Texture2D texture, Random rnd)
		{
			this.texture = texture;
			numberOfTilesInTexture = texture.Width / Settings.gridsize;

			this.type = tileType; 


			this.randTexture = rnd.Next (0,4); 

			this.position = position; 

			if (type > 0)
				isPassable = false;
			else
				isPassable = true; 

		}
		public void Draw(SpriteBatch spriteBatch, Vector2 camOffset){

			//vi måste räkna ut positionen i förhållande till kameran.

			//ny position = originalpositionen - kamerans offset(kamerans position). 
			// så om kameran har flyttats 100px till höger, så måste vi dra av 100px från positionen. 
			Vector2 drawPos = position - camOffset;
		 
			//Rita ut rutan
			spriteBatch.Draw (texture, drawPos, 
				new Rectangle (type * tileWidth, randTexture * tileWidth , texture.Width / numberOfTilesInTexture, texture.Height),
				Color.White);

		}

		public void Clicked()
		{
			if (isPassable)
				Console.WriteLine ("Passable");
			else {
				Console.WriteLine ("Not passable"); 

			}
			if (type  == 2) {
				isPassable = true; 
				type = 0;
			} 

		}

		public bool isColliding(Rectangle otherRect)
		{
			Rectangle myRect = new Rectangle (
				                   Convert.ToInt32 (position.X),
				Convert.ToInt32 (position.Y),
				                   texture.Width / numberOfTilesInTexture,
				texture.Height /numberOfTilesInTexture);

			if (myRect.Intersects (otherRect)) {
				if (isPassable) {
				
					return false; 
				} else {
					return true; 

				}


			}

			return false; 


		}
	}


}

