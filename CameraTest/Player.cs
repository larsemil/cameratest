#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;

#endregion


namespace CameraTest
{

	public class Player:PhysicalObject{

		private Keys Right;
		private Keys Left;
		private Keys Jump;

	
		Texture2D heartTexture; 
		//private bool isAlive;
		int wasHit;
		int damage; 

		public Player (Texture2D texture, Vector2 position, Keys right, Keys left, Keys jump, Texture2D heartTexture)
			:base(texture, position)
		{

			this.Right = right;
			this.Left = left; 
			this.Jump = jump; 
			direction = Directions.Right;

			action = Actions.falling;
			isAlive = true; 
			inAir = true; 
			speed = 5; 
			weight = 1; 
			health = 100;
			damage = 0;

			wasHit = 0; 
			spritesHigh = 3; 
			this.heartTexture = heartTexture; 


		}

		public void Update(Camera cam, World tellus)
		{
			KeyboardState newState = Keyboard.GetState (); 

			// get which key is pressed and set the direction for the player. 
		
			if (newState.IsKeyDown (Right)) {
				position.X += speed;
				direction = Directions.Right;
			}

			if (newState.IsKeyDown (Left)) {
				position.X -= speed;
				direction = Directions.Left;
			}

			if (newState.IsKeyDown (Jump) ) {
				if (action == Actions.falling || action == Actions.jumping ) {
				
				}
				else{
					action = Actions.jumping;
					jumpForce = 30;
				 
				}

			} 

			if (inAir ) {
				position.Y += tellus.gravity - jumpForce; 

				if (jumpForce-- < 0) {
					action = Actions.falling;
					if (jumpForce <= -60)
						jumpForce = -60; 
				}
			}

			this.checkCollision (cam, tellus); 
			//addGravity (tellus); 

			if (wasHit > 0)
				wasHit--; 
		}

		public void checkCollision(Camera cam, World tellus)
		{
			/* @TODO: 
			 * Refactor collision code so that it only needs to loop the tiles once. Now its doing it thrice... 
			 * Split myRect into three rects that get checked individually. 
			 * 
			 */


			//first check if player is on the ground. 
			Rectangle myRect = new Rectangle (
				Convert.ToInt32 (position.X+ ( texture.Width/3)), 
				Convert.ToInt32 (position.Y + (texture.Height / 3) -1 ), 
				texture.Width / 2, 
				1); 

			bool checkIfOnGround = false; 
			if (inAir && action == Actions.still) {
				action = Actions.falling;
			}
			inAir = true; 
			//we only have to loop through visible tiles. 
			foreach (var tile in cam.visibleTiles) {

			
				while(tile.isColliding(myRect))
				{
					checkIfOnGround = true; 

					position.Y--; 

					myRect = new Rectangle (
						Convert.ToInt32 (position.X+ ( texture.Width/3)), 
						Convert.ToInt32 (position.Y + (texture.Height / 3) -1), 
						texture.Width / 2, 
						1);  

				}


			}

			if (checkIfOnGround || action == Actions.still) {
				jumpForce = 0; 

				action = Actions.still;
				//inAir = false; 


			} else {
				inAir = true; 
			}


			 
			//then we check if we WALK into something to the sides
			if (direction == Directions.Left) {

				myRect = new Rectangle (
					Convert.ToInt32 (position.X ), 
					Convert.ToInt32 (position.Y + ((texture.Height / 3) / 3) ), 
					1, 
					(texture.Height / 3) /3 ); 
			} else if (direction == Directions.Right) {

				myRect = new Rectangle (
					Convert.ToInt32 (position.X + texture.Width), 
					Convert.ToInt32 (position.Y + ((texture.Height / 3) / 3) ), 
					1, 
					(texture.Height / 3) /3 );
			}

			foreach (var tile in cam.visibleTiles) {
				while (tile.isColliding (myRect)) {
					if (direction == Directions.Left) {
						position.X++; 
						myRect = new Rectangle (
							Convert.ToInt32 (position.X ), 
							Convert.ToInt32 (position.Y + ((texture.Height / 3) / 3) ), 
							1, 
							(texture.Height / 3) /3 );

					}
					if (direction == Directions.Right) {
						position.X--; 
						myRect = new Rectangle (
							Convert.ToInt32 (position.X + texture.Width), 
							Convert.ToInt32 (position.Y + ((texture.Height / 3) / 3) ), 
							1, 
							(texture.Height / 3) /3 );

					}



				}

			}
			if (action == Actions.jumping || action == Actions.falling) {
				myRect = new Rectangle (
					Convert.ToInt32 (position.X + (texture.Width / 3)), 
					Convert.ToInt32 (position.Y ), 
					texture.Width / 2, 
					1);
				foreach (var tile in cam.visibleTiles) {
					while (tile.isColliding (myRect)) {
						position.Y++;
						myRect = new Rectangle (
							Convert.ToInt32 (position.X +(texture.Width/ 3)), 
							Convert.ToInt32 (position.Y ), 
							texture.Width / 2, 
							1);
					}

				}

			}

		}

		public override void Hit(int damage)
		{
			if (wasHit <= 0) {
				this.damage += damage; 

				wasHit = 100; 
				jumpForce = 25; 

			}

		}

		public void Draw(SpriteBatch spriteBatch, Camera cam, GameWindow window)
		{

			//vi måste räkna ut positionen i förhållande till kameran.

			//ny position = originalpositionen - kamerans offset(kamerans position). 
			// så om kameran har flyttats 100px till höger, så måste vi dra av 100px från positionen. 
			Vector2 drawPos = position - cam.position;

			//check to see if recently hit, if so, blink
			if (wasHit > 0) {
				if ((wasHit % 4) == 1) {
					if (direction == Directions.Right)
						spriteBatch.Draw (texture, drawPos, new Rectangle (0, (int)action * (texture.Height / 3), texture.Width, texture.Height / 3), Color.AliceBlue);
					else {

						spriteBatch.Draw (texture, drawPos, null, new Rectangle (0, (int)action * (texture.Height / 3), texture.Width, texture.Height / 3), null, 0, null, Color.White, SpriteEffects.FlipHorizontally, 0);
					}
				}
			} else {

				if (direction == Directions.Right)
					spriteBatch.Draw (texture, drawPos, new Rectangle (0, (int)action * (texture.Height / 3), texture.Width, texture.Height / 3), Color.AliceBlue);
				else {

					spriteBatch.Draw (texture, drawPos, null, new Rectangle (0, (int)action * (texture.Height / 3), texture.Width, texture.Height / 3), null, 0, null, Color.White, SpriteEffects.FlipHorizontally, 0);
				}
			}



			//render health
			for (int i = 0; i < 10; i++) {
				if ((i * 10) < (health - damage)) {

					spriteBatch.Draw (heartTexture, new Vector2 ((i* (heartTexture.Width+5)) , 40), new Rectangle (0, 0, heartTexture.Width, heartTexture.Height / 2), Color.AliceBlue);

				} else {
					spriteBatch.Draw (heartTexture, new Vector2 ((i* (heartTexture.Width+5)), 40), new Rectangle (0, heartTexture.Height / 2, heartTexture.Width, heartTexture.Height / 2), Color.AliceBlue);
				}

			}



		}

	
	} //end of class


}

