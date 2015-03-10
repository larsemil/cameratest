#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;

#endregion

namespace CameraTest
{
	public class Enemies:PhysicalObject
	{
		int sightDistance;
		int health; 
		public int attackDamage;

		public Enemies (Texture2D texture, Vector2 position)
			:base(texture, position)
		{
			this.health = 100; 
 
			this.speed = 1; 
 	
			weight = 1; 
			attackDamage = 10; 
			this.sightDistance = 5; 
		}

		public virtual void Update(World tellus, Camera cam, Player player){

			//Move forward
			position.X += speed * (int)direction;


			var myRect = new Rectangle ((int)position.X, (int)position.Y, texture.Width, texture.Height);

			int myPosOnMapX = this.mapX;
			int myPosOnMapY = this.mapY;

			if (direction == Directions.Right) {
				myPosOnMapX += 1; 
			} 
			try{
				if (tellus.map [myPosOnMapX, myPosOnMapY].isColliding (myRect)) {
					direction = (Directions)((int)direction * -1);

				}
			}
			catch{
				//ENEMY HAS FALLEN OUT OF THE MAP!
				isAlive = false; 
				return; 
			}
			/* Check if players is in front. For now it does not care if there are any solid blocks within sight */

			var viewRect = new Rectangle (((int)position.X + texture.Width), (int)position.Y, (sightDistance * Settings.gridsize), texture.Height);
			if (direction == Directions.Left) {

				viewRect = new Rectangle (((int)position.X -(5* Settings.gridsize)), (int)position.Y, (sightDistance * Settings.gridsize), texture.Height);
			} 


			//if player is within view of the enemy(5 squares in front)

			if (player.isColliding (viewRect)) {
				speed = 10; 

			} else
				speed = 1; 


			//if player is EATEN by enemy(or shot, or tazered, or whatevah)

			if(player.isColliding(myRect))
				player.Hit(attackDamage);

			addGravity (tellus); 

		}






	}

	public class jumperEnemy:Enemies{

		public jumperEnemy(Texture2D texture, Vector2 position)
			:base(texture, position){
		}
		public override void Update (World tellus, Camera cam, Player player)
		{
			if (jumpForce <= 0 && action == Actions.still)
				jumpForce = 20; 

			base.Update (tellus, cam, player);
		}

	}

	public class rotatingEnemy:Enemies{
		int currentAnimationFrame; 
		int ticksPerAnimationFrame; 
		int currentTick; 


		public rotatingEnemy(Texture2D texture, Vector2 position)
			:base(texture, position)
		{
			currentAnimationFrame = 0; 
			ticksPerAnimationFrame = 10;
			currentTick = 0; 
			speed = 3; 
			attackDamage = 20; 
			spritesHigh = 4; 
		}


		public override void Update(World tellus, Camera cam, Player player)
		{
		
			base.Update (tellus, cam, player); 
		}

		public override void Draw(SpriteBatch spriteBatch, Camera cam)
		{
			if (++currentTick == ticksPerAnimationFrame){
				if (++currentAnimationFrame == 4)
					currentAnimationFrame = 0; 

				currentTick = 0; 
			}
			spriteBatch.Draw (texture, position - cam.position,new Rectangle(0, currentAnimationFrame * Settings.gridsize, texture.Width, texture.Height / 4), Color.White);

		}
	}
}

