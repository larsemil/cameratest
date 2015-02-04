#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;

#endregion

namespace CameraTest
{
	public class World
	{
		int worldSize; 
		Texture2D texture; 
		public Tile[,] map; 
		int numberOfTilesInTexture; 
		public int gravity;

		public World (Texture2D texture)
		{
			gravity = 9; 
			worldSize = 40; 
			this.texture = texture;

			map = new Tile[worldSize,worldSize];

			Random rnd = new Random ();
			numberOfTilesInTexture = texture.Width / Settings.gridsize;


			//generate world, first do background. its all tiles, but they are passable (ie non colliding)
			for (int x = 0; x < worldSize; x++) {
				for (int y = 0; y < worldSize; y++) {
					map [x, y] = new Tile (
						rnd.Next(0,2),	
						new Vector2 (x * (texture.Width / numberOfTilesInTexture), (y) * texture.Height),
						texture);
				}

			}

			//add ground
			for (int x = 0; x < worldSize; x++) {
				map [x, worldSize-1] = new Tile (
					numberOfTilesInTexture-2,	
					new Vector2 (x * (texture.Width / numberOfTilesInTexture), (worldSize - 1) * texture.Height),
					texture);

				map [x, 0] = new Tile (
					numberOfTilesInTexture-2,	
					new Vector2 (x * (texture.Width / numberOfTilesInTexture), 1),
					texture);
			}

			//add walls on the sides
			for (int y = 0; y < worldSize; y++) {

				map [0, y].isPassable = false;
				map [worldSize-1,y].isPassable = false;

				map [0, y].type = 2;
				map [worldSize-1, y].type = 2;

			}

			//sätt ut lite plattformar
			for (int z = 0; z < 2; z++) {
				for (int y = 2; y < worldSize - 1; y += 3) {
					int platformLength = rnd.Next (4, 10);
					int xPos = rnd.Next (0, worldSize);

					for (int x = 0; x < platformLength; x++) {
						try {
											 
							map [xPos + x, y].type = 3;
							map [xPos + x, y].isPassable = false; 

						} catch {
							continue; 
						}
					
					}

				}
			}

		}

		public void Draw(SpriteBatch spriteBatch, Camera cam)
		{
			//Vi gör en rektangel lika stor som kameran. 
			Rectangle camRect = new Rectangle (
				                    Convert.ToInt32 (cam.position.X),
				                    Convert.ToInt32 (cam.position.Y),
				(cam.width +1) * (texture.Width / numberOfTilesInTexture),
				(cam.height +2) * texture.Height);
			cam.visibleTiles.Clear ();
			//vi loopar igenom ALLA rutor
			for (int x = 0; x < worldSize; x++) {
				for (int y = 0; y < worldSize; y++) {

					//För varje ruta så gör vi en rektangel-
					Rectangle tileRect = new Rectangle (x * (texture.Width / numberOfTilesInTexture), y * texture.Height, texture.Width / numberOfTilesInTexture, texture.Height);

					//och så kollar vi om rektangeln för rutan är inom rektangeln för kameran.
					if(tileRect.Intersects(camRect)){

						//isåfall så ritar vi ut den. vi skickar med kamerans position för att kunna offsetta det vi ritar ut. 
						cam.visibleTiles.Add(map [x, y]);
						map [x, y].Draw (spriteBatch,cam.position);	
						  
					}
				}
			}

		}
	}
}

