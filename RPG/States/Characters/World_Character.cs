using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RPG.States.Configuration;
using RPG.States.DebugHelp;
using RPG.States.World;
using System;

namespace RPG.States.Characters
{
    public class World_Character : Character
    {
        private float MoveSpeed = 1;
        private int CharacterAnimationRow = 0;
        private readonly Texture2D BaseTexture;
        public Vector2 CameraPostition = new Vector2(550, 100), CharacterOffset;
        public void PlaceCamera()
        {
            CharacterX = DisplayOutputSettings.CenterScreenTileOffsetX * EngineSettings.TileSize;
            CharacterY = DisplayOutputSettings.CenterScreenTileOffsetY * EngineSettings.TileSize;
            CameraPostition.X -= DisplayOutputSettings.CenterScreenTileOffsetX;
            CameraPostition.Y -= DisplayOutputSettings.CenterScreenTileOffsetY;
        }
        public World_Character(Texture2D BaseTexture)
        {
            this.BaseTexture = BaseTexture;
        }
        public void Update(GameTime gameTime)
        {
            WalkSpeed();
            Vector2 CharacterMovement = UpdatePosition();
            if (CharacterMovement != Vector2.Zero)
            {
                CameraPostition += CharacterMovement / EngineSettings.TileSize;
                if (CameraPostition.X <= 0) CameraPostition.X += WorldInformation.mapWidth;
                else if (CameraPostition.X > WorldInformation.mapWidth) CameraPostition.X -= WorldInformation.mapWidth;
                if (CameraPostition.Y <= 0) CameraPostition.Y += WorldInformation.mapHeight;
                else if (CameraPostition.Y > WorldInformation.mapHeight) CameraPostition.Y -= WorldInformation.mapHeight;

                CharacterOffset = new Vector2(
                    ((int)CameraPostition.X - CameraPostition.X) * EngineSettings.TileSize,
                    ((int)CameraPostition.Y - CameraPostition.Y) * EngineSettings.TileSize);


                if (!BattleChance()) //Check if battle occurs
                {
                    UpdateCharacter(CharacterMovement);
                }
                else
                {
                    stepsSinceLastBattle = 0;
                    //Start battle animation
                    GSS.World.StartBattle();
                    //load battle
                    //Go to battle scene (continue there)
                }
            }
        }

        private void WalkSpeed()
        {
            if (GSS.Controlls.R2()) //Sprint
                MoveSpeed = 5;
            else
                MoveSpeed = 1;
        }

        private Vector2 UpdatePosition()
        {
            Vector2 CharacterMovement = Vector2.Zero;
            if (GSS.Controlls.Up())
            {
                //Up direction
                CharacterMovement.Y -= MoveSpeed;
            }
            else if (GSS.Controlls.Down())
            {
                //Down direction
                CharacterMovement.Y += MoveSpeed;
            }
            if (GSS.Controlls.Right())
            {
                //Right direction
                CharacterMovement.X += MoveSpeed;
            }
            else if (GSS.Controlls.Left())
            {
                //Left direction
                CharacterMovement.X -= MoveSpeed;
            }
            CharacterMovement = GetDirection(CharacterMovement);
            return CharacterMovement;
        }

        int CharacterX = 0, CharacterY = 0;
        int CharacterActiveTileX = 0, CharacterActiveTileY = 0;
        int tx, ty;
        private Vector2 GetDirection(Vector2 direction)
        {
            if (direction != Vector2.Zero)
            {
                if (GSS.FreeCameraMode || IsValid(direction))
                {
                    return direction;
                }
                else
                {
                    Vector2 tmpX = new Vector2(direction.X, 0);
                    Vector2 tmpY = new Vector2(0, direction.Y);
                    if (IsValid(tmpX) && tmpX != Vector2.Zero)
                        return tmpX;
                    if (IsValid(tmpY) && tmpY != Vector2.Zero)
                        return tmpY;
                    return Vector2.Zero;
                }
            }
            return Vector2.Zero;
        }
        bool IsValid(Vector2 direction)
        {
            Vector2 futurePos = CameraPostition + (direction / EngineSettings.TileSize);

            CharacterActiveTileX = (int)((DisplayOutputSettings.CenterScreenTileOffsetX + 0.5 + (int)futurePos.X - futurePos.X) * EngineSettings.TileSize);
            CharacterActiveTileY = (int)((DisplayOutputSettings.CenterScreenTileOffsetY + 0.5 + (int)futurePos.Y - futurePos.Y) * EngineSettings.TileSize);

            tx = (int)(futurePos.X + DisplayOutputSettings.CenterScreenTileOffsetX);
            ty = (int)(futurePos.Y + DisplayOutputSettings.CenterScreenTileOffsetY + 1);
            return TilePalette.IsWalkable(tx, ty);
        }
        int stepsSinceLastBattle = 0;
        private bool BattleChance()
        {
            //check if battle occurs
            //(1/(log((((x+20)^2)/2)+5)-log(x-1)))*35
            if (stepsSinceLastBattle < 2 || GSS.NoRandomBattle)
                return false;

            double odds = (1 / (Math.Log(Math.Pow((stepsSinceLastBattle + 20), 2) + 5) - Math.Log(stepsSinceLastBattle - 1))) * 35;
            if (GSS.GlobalRand.NextDouble() < odds)
                return true;
            return false;
        }
        public Vector2 GetPosition()
        {
            return CameraPostition * EngineSettings.TileSize;
        }
        public Vector2 GetCenterTilePosition()
        {
            return CameraPostition + new Vector2(DisplayOutputSettings.CenterScreenTileOffsetX + 0.5f, DisplayOutputSettings.CenterScreenTileOffsetY + 0.5f);
        }
        private void UpdateCharacter(Vector2 direction)
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
        Texture2D GetCharacterTexture()
        {
            return BaseTexture;
        }
        public void Draw()
        {
            GSS.SpriteBatch.Draw(GetCharacterTexture(),
                new Rectangle(
                    CharacterX,
                    CharacterY,
                    EngineSettings.TileSize, EngineSettings.TileSize
                    ), Color.White);

            if (GSS.DebugMode)
            {
                GSS.SpriteBatch.Draw(GetCharacterTexture(),
                    new Rectangle(
                        CharacterActiveTileX,
                        CharacterActiveTileY,
                        EngineSettings.TileSize, EngineSettings.TileSize
                        ), Color.White);

            }
            DebugConsole.WriteLine($"GameTime {GSS.GetTime():dd-mmm-yyyy HH:mm:ss}");
        }
    }
}
