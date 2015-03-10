#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;

#endregion

namespace CameraTest
{
	public class PhysicalObject
	{
		public Vector2 position; 

		public Texture2D texture; 

		public float weight; 
		public int jumpForce;
		public float speed; 

		public bool isAlive; 
		public bool inAir;

		public Actions action; 

		public enum Directions {Left = -1, None = 0, Right = 1}; 
		public Directions direction;


		public int health =100; 
		public int damage; 

		public int spritesHigh;

		public PhysicalObject (Texture2D texture, Vector2 position )
		{
			this.texture = texture;
			this.position = position;
			this.speed = 0; 
			isAlive = true; 
			action = Actions.falling;
			jumpForce = 0; 
			direction = Directions.Right; 

			spritesHigh = 1; 
		}

		public virtual void Draw(SpriteBatch spriteBatch, Camera cam)
		{
			if (direction == Directions.Left) {
				spriteBatch.Draw (texture, position - cam.position, Color.White); 
			} else {
				spriteBatch.Draw(texture, position - cam.position,null, new Rectangle(0,0, texture.Width, texture.Height),  null, 0, null, Color.White, SpriteEffects.FlipHorizontally,0);

			}


		}

		public void addGravity(World tellus)
		{ 
			position.Y += (tellus.gravity - jumpForce);  

			if (jumpForce-- < 0)
				action = Actions.falling;



			Rectangle myRect = new Rectangle (
				Convert.ToInt32 (position.X+ ( texture.Width / 3)), 
				Convert.ToInt32 (position.Y + (texture.Height / spritesHigh) -1 ), 
				texture.Width / 2, 
				1); 

			                   

			bool checkIfOnGround = false; 
			if (action == Actions.still) {
				action = Actions.falling;
			}

			for(int x = 0; x < 4; x++) {

				try{
					while(tellus.map[mapX + (x-2),mapY+1].isColliding(myRect))
					{
						checkIfOnGround = true; 

						position.Y--; 

						myRect = new Rectangle (
							Convert.ToInt32 (position.X+ ( texture.Width / 3)), 
							Convert.ToInt32 (position.Y + (texture.Height / spritesHigh) -1 ), 
							texture.Width / 2, 
							1); 
					}
				}
				catch(Exception e)
				{
					continue; 
				}

			}

			if (checkIfOnGround || action == Actions.still) {
				jumpForce = 0; 

				action = Actions.still;
				//inAir = false; 


			} else {
				inAir = true; 
			}

		}


		public bool isColliding(Rectangle otherRect)
		{
			Rectangle myRect = new Rectangle ((int)position.X, (int)position.Y, texture.Width, texture.Height / spritesHigh);

			if(myRect.Intersects(otherRect))
				return true;

			return false;

		}

		public virtual void Hit(int damage)
		{
			this.damage += damage;  

		}


		public int mapX{
			get{
				return (int)position.X / Settings.gridsize;
			}

		}

		public int mapY{
			get{
				return (int)position.Y / Settings.gridsize;
			}

		}
	}
}

