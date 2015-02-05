#region Using Statements
using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.Input;

#endregion


namespace CameraTest
{

	public class Player{
		private Actions action;


		private const int speed = 5;
		bool inAir; 

		int jumpForce = 0; 
		private Keys Right;
		private Keys Left;
		private Keys Jump;
		enum Directions {Left, Right, None}; 
		
		private Directions direction; 
		private Texture2D texture;
		public Vector2 position;

		private bool isAlive;



		public Player (Texture2D texture, Vector2 position, Keys right, Keys left, Keys jump)
		{
			this.texture = texture;
			this.position = position;

			this.Right = right;
			this.Left = left; 
			this.Jump = jump; 
			direction = Directions.Right;

			action = Actions.falling;
			isAlive = true; 
			inAir = true; 

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
					Console.WriteLine ("JUMPING!"); 
				}

			} 

			if (inAir ) {
				position.Y += tellus.gravity - jumpForce; 

				if (jumpForce-- < 0)
					action = Actions.falling;
			}

			this.checkCollision (cam, tellus); 

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

		public void Draw(SpriteBatch spriteBatch, Camera cam)
		{

			//vi måste räkna ut positionen i förhållande till kameran.

			//ny position = originalpositionen - kamerans offset(kamerans position). 
			// så om kameran har flyttats 100px till höger, så måste vi dra av 100px från positionen. 
			Vector2 drawPos = position - cam.position;
			 
			if (direction == Directions.Right)
				spriteBatch.Draw (texture, drawPos, new Rectangle (0, (int)action * (texture.Height / 3), texture.Width, texture.Height / 3), Color.AliceBlue);
			else {

				spriteBatch.Draw(texture, drawPos,null, new Rectangle(0,(int)action * (texture.Height / 3),texture.Width, texture.Height / 3),  null, 0, null, Color.White, SpriteEffects.FlipHorizontally,0);
			}
		}

	
	
	
	} //end of class


}

