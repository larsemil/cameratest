#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;

#endregion

namespace CameraTest
{
	public class Enemies
	{
		int health;
		Vector2 position; 
		Texture2D texture; 
		enum Directions {Left = -1, Right =1, None =0}; 
		float speed; 

		private Directions direction; 

		public Enemies (Texture2D texture, Vector2 position)
		{
			this.health = 100; 
			this.texture = texture; 
			this.speed = 1; 
			this.position = position; 
			direction = Directions.Right;
		}

		public virtual void Update(World tellus, Camera cam){

			position.X += speed * (int)direction;


			Rectangle myRect = new Rectangle ((int)position.X, (int)position.Y, texture.Width, texture.Height);

			int myPosOnMapX = (Convert.ToInt32(position.X)) / Settings.gridsize;
			int myPosOnMapY = (Convert.ToInt32(position.Y)) / Settings.gridsize;
			Console.WriteLine ("X for enemy: " + myPosOnMapX); 
			if (direction == Directions.Right) {
				myPosOnMapX += 1; 

			}

			if (tellus.map [myPosOnMapX, myPosOnMapY].isColliding (myRect)) {
				direction = (Directions)((int)direction * -1);
				Console.WriteLine ("DARN. I COLLIDED!" + speed); 
			}

		}



		public virtual void Draw(SpriteBatch spriteBatch, Camera cam){

			if (direction == Directions.Left) {
				spriteBatch.Draw (texture, position - cam.position, Color.White); 
			} else {
				spriteBatch.Draw(texture, position - cam.position,null, new Rectangle(0,0, texture.Width, texture.Height),  null, 0, null, Color.White, SpriteEffects.FlipHorizontally,0);

			}
		}


	}
}

