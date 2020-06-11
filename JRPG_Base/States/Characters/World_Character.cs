using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JRPG_Base.States.Characters
{
    public class World_Character
    {
        float MoveSpeed = 1;
        private int CharacterAnimationRow = 0;
        Texture2D BaseTexture;
        public Vector2 CameraPostition = new Vector2(550, 130), CharacterOffset;
        public void PlaceCamera()
        {
            CharacterX = GSS.CenterScreenTileOffsetX * GSS.TileSize;
            CharacterY = GSS.CenterScreenTileOffsetY * GSS.TileSize;
            CameraPostition.X -= GSS.CenterScreenTileOffsetX;
            CameraPostition.Y -= GSS.CenterScreenTileOffsetY;
        }
        public World_Character(Texture2D BaseTexture)
        {
            this.BaseTexture = BaseTexture;
        }
        public void Update(GameTime gameTime)
        {
            if (GSS.KeyState.IsKeyDown(Keys.LeftControl)) //Sprint
                MoveSpeed = 5;
            else
                MoveSpeed = 1;

            Vector2 CharacterMovement = Vector2.Zero;
            if (GSS.PadState.DPad.Up == ButtonState.Pressed || GSS.KeyState.IsKeyDown(Keys.Up) || GSS.KeyState.IsKeyDown(Keys.W))
            {
                //Up direction
                CharacterMovement.Y -= MoveSpeed;
            }
            else if (GSS.PadState.DPad.Down == ButtonState.Pressed || GSS.KeyState.IsKeyDown(Keys.Down) || GSS.KeyState.IsKeyDown(Keys.S))
            {
                //Down direction
                CharacterMovement.Y += MoveSpeed;
            }
            if (GSS.PadState.DPad.Right == ButtonState.Pressed || GSS.KeyState.IsKeyDown(Keys.Right) || GSS.KeyState.IsKeyDown(Keys.D))
            {
                //Right direction
                CharacterMovement.X += MoveSpeed;
            }
            else if (GSS.PadState.DPad.Left == ButtonState.Pressed || GSS.KeyState.IsKeyDown(Keys.Left) || GSS.KeyState.IsKeyDown(Keys.A))
            {
                //Left direction
                CharacterMovement.X -= MoveSpeed;
            }
            CharacterMovement = getDirection(CharacterMovement);
            if (CharacterMovement != Vector2.Zero)
            {
                CameraPostition += CharacterMovement / (float)GSS.TileSize;
                if (CameraPostition.X <= 0) CameraPostition.X += GSS.world.mapWidth;
                else if (CameraPostition.X > GSS.world.mapWidth) CameraPostition.X -= GSS.world.mapWidth;
                if (CameraPostition.Y <= 0) CameraPostition.Y += GSS.world.mapHeight;
                else if (CameraPostition.Y > GSS.world.mapHeight) CameraPostition.Y -= GSS.world.mapHeight;

                CharacterOffset = new Vector2(
                    ((int)CameraPostition.X - CameraPostition.X) * GSS.TileSize,
                    ((int)CameraPostition.Y - CameraPostition.Y) * GSS.TileSize);


                if (!BattleChance()) //Check if battle occurs
                {
                    updateCharacter(CharacterMovement);
                }
                else
                {
                    stepsSinceLastBattle = 0;
                    //Start battle animation
                    GSS.world.startBattle();
                    //load battle
                    //Go to battle scene (continue there)
                }
            }
        }
        int CharacterX = 0, CharacterY = 0;
        int CharacterActiveTileX = 0, CharacterActiveTileY = 0;
        int tx, ty;
        private Vector2 getDirection(Vector2 direction)
        {
            if (direction != Vector2.Zero)
            {
                if (GSS.CameraMode || isValid(direction))
                {
                    return direction;
                }
                else
                {
                    Vector2 tmpX = new Vector2(direction.X, 0);
                    Vector2 tmpY = new Vector2(0, direction.Y);
                    if (isValid(tmpX) && tmpX != Vector2.Zero)
                        return tmpX;
                    if (isValid(tmpY) && tmpY != Vector2.Zero)
                        return tmpY;
                    return Vector2.Zero;
                }
            }
            return Vector2.Zero;
        }
        bool isValid(Vector2 direction)
        {
            Vector2 futurePos = CameraPostition + (direction / (float)GSS.TileSize);

            CharacterActiveTileX = (int)((GSS.CenterScreenTileOffsetX + 0.5 + (int)futurePos.X - futurePos.X) * GSS.TileSize);
            CharacterActiveTileY = (int)((GSS.CenterScreenTileOffsetY + 0.5 + (int)futurePos.Y - futurePos.Y) * GSS.TileSize);

            tx = (int)(futurePos.X + GSS.CenterScreenTileOffsetX);
            ty = (int)(futurePos.Y + GSS.CenterScreenTileOffsetY + 1);
            return GSS.world.IsWalkable(tx, ty);
        }
        int stepsSinceLastBattle = 0;
        private bool BattleChance()
        {
            //check if battle occurs
            //(1/(log((((x+20)^2)/2)+5)-log(x-1)))*35
            if (stepsSinceLastBattle < 2 || GSS.NoRandomBattle)
                return false;

            double odds = (1 / (Math.Log(Math.Pow((stepsSinceLastBattle + 20), 2) + 5) - Math.Log(stepsSinceLastBattle - 1))) * 35;
            if (GSS.globalRand.NextDouble() < odds)
                return true;
            return false;
        }
        public Vector2 getPosition()
        {
            return CameraPostition * GSS.TileSize;
        }
        private void updateCharacter(Vector2 direction)
        {
            if (direction.X > 0)
            {
                if (direction.Y > 0)
                {
                    //Walk Down Left
                    CharacterAnimationRow = 6;
                }
                else if (direction.Y < 0)
                {
                    //Walk Up Left
                    CharacterAnimationRow = 8;
                }
                else
                {
                    //Walk Left
                    CharacterAnimationRow = 7;
                }
            }
            else if (direction.X < 0)
            {
                if (direction.Y > 0)
                {
                    //Walk Down Right
                    CharacterAnimationRow = 4;
                }
                else if (direction.Y < 0)
                {
                    //Walk Up Right
                    CharacterAnimationRow = 2;
                }
                else
                {
                    //Walk Right
                    CharacterAnimationRow = 3;
                }
            }
            else
            {
                if (direction.Y > 0)
                {
                    //Walk Down
                    CharacterAnimationRow = 5;
                }
                else if (direction.Y < 0)
                {
                    //Walk Up
                    CharacterAnimationRow = 1;
                }
                else
                {
                    //Idle
                    CharacterAnimationRow = 0;
                }
            }
        }
        public void Draw()
        {
            GSS.spriteBatch.Draw(BaseTexture,
                new Rectangle(
                    CharacterX,
                    CharacterY,
                    GSS.TileSize, GSS.TileSize
                    ), Color.White);

            if (GSS.DebugMode)
            {
                GSS.spriteBatch.Draw(BaseTexture,
                    new Rectangle(
                        CharacterActiveTileX,
                        CharacterActiveTileY,
                        GSS.TileSize, GSS.TileSize
                        ), Color.White);
            }
            //GSS.ExtraOutput = GSS.getTime().ToString("dd-mmm-yyyy HH:mm:ss");
        }
    }
}
