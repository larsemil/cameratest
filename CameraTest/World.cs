#region Using Statements
using System;
using System.Collections.Generic;
using System.Drawing;

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
		List <Enemies> enemies; 

		public World (Texture2D texture, Texture2D enemyTexture)
		{
			gravity = 9; 
			this.texture = texture;

			Bitmap level = new Bitmap ("Content/level.png");
			enemies = new List<Enemies> (); 


			worldSize = level.Width;

			map = new Tile[worldSize,worldSize];

			numberOfTilesInTexture = texture.Width / Settings.gridsize;
			Random rnd = new Random (); 
			//loop through the level and generate world
			for (int x = 0; x < worldSize; x++) {
				for (int y = 0; y < worldSize; y++) {
					System.Drawing.Color tmpCol = level.GetPixel (x, y);

					if (tmpCol == System.Drawing.Color.FromArgb (0, 0, 0)) {
						map [x, y] = new Tile (3, new Vector2 (x * (texture.Width / numberOfTilesInTexture), y * (texture.Height / numberOfTilesInTexture)), texture, rnd);

						
					} else if (tmpCol == System.Drawing.Color.FromArgb (0, 255, 0)) {

						map [x, y] = new Tile (1, new Vector2 (x * (texture.Width / numberOfTilesInTexture), y * (texture.Height / numberOfTilesInTexture)), texture, rnd);
					} else if (tmpCol == System.Drawing.Color.FromArgb (0, 0, 255)) {
						map [x, y] = new Tile (2, new Vector2 (x * (texture.Width / numberOfTilesInTexture), y * (texture.Height / numberOfTilesInTexture)), texture, rnd);
					
					} else if (tmpCol == System.Drawing.Color.FromArgb (255, 0, 0)) {
						map [x, y] = new Tile (0, new Vector2 (x * (texture.Width / numberOfTilesInTexture), y * (texture.Height / numberOfTilesInTexture)), texture, rnd);
						enemies.Add (new Enemies (enemyTexture, new Vector2 (x * Settings.gridsize, y * Settings.gridsize)));

					}
					else{
						map [x, y] = new Tile (0, new Vector2 (x * (texture.Width / numberOfTilesInTexture), y * (texture.Height / numberOfTilesInTexture)), texture, rnd);
					}
				}

			}

		}

		public void Draw(SpriteBatch spriteBatch, Camera cam)
		{
			//Vi gör en rektangel lika stor som kameran. 
			Microsoft.Xna.Framework.Rectangle camRect = new Microsoft.Xna.Framework.Rectangle (
				                    Convert.ToInt32 (cam.position.X),
				                    Convert.ToInt32 (cam.position.Y),
				(cam.width +1) * (texture.Width / numberOfTilesInTexture),
				(cam.height +2) * texture.Height);

			cam.visibleTiles.Clear ();
			//vi loopar igenom ALLA rutor
			for (int x = (int)((cam.position.X / Settings.gridsize)); x <= ((cam.position.X / Settings.gridsize) + cam.width +1); x++) {
				for (int y = (int)((cam.position.Y / Settings.gridsize)); y <= ((cam.position.Y / Settings.gridsize) + cam.height +1); y++) {

					//För varje ruta så gör vi en rektangel-
					Microsoft.Xna.Framework.Rectangle tileRect = new Microsoft.Xna.Framework.Rectangle (x * (texture.Width / numberOfTilesInTexture), y * (texture.Height / numberOfTilesInTexture), texture.Width / numberOfTilesInTexture, texture.Height / numberOfTilesInTexture);

					//och så kollar vi om rektangeln för rutan är inom rektangeln för kameran.
					if(tileRect.Intersects(camRect)){

						//isåfall så ritar vi ut den. vi skickar med kamerans position för att kunna offsetta det vi ritar ut. 
						try{
							cam.visibleTiles.Add(map [x, y]);
							map [x, y].Draw (spriteBatch,cam.position);	
						}
						catch
						{
							continue; 
						}
					}
				}
			}


			foreach (var enemy in enemies) {
				enemy.Draw (spriteBatch, cam); 

			}

		}

		public void Update(Camera cam)
		{
			foreach (var enemy in enemies) {
				enemy.Update(this, cam); 
			}

		}
	}
}

