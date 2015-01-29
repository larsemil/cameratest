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
		bool isPassable; 
		int type;
		Vector2 position; 
		Texture2D texture;
		const int tileWidth = 64; 
		int numberOfTilesInTexture;

		public Tile (int tileType, Vector2 position, Texture2D texture )
		{
			this.texture = texture;
			numberOfTilesInTexture = texture.Width / Settings.gridsize;
			this.type = tileType; 
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
				new Rectangle (type * tileWidth, 0 , texture.Width / numberOfTilesInTexture, texture.Height),
				Color.White);

		}

		public void Clicked()
		{
			type = 3; 


		}
	}


}

