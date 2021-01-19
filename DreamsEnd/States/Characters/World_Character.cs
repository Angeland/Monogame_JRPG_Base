using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using RPG.States.Configuration;
using RPG.States.DebugHelp;
using RPG.States.World;
using RPG.Textures;
using System;

namespace RPG.States.Characters
{
    public class World_Character : AnimationCollection, IWorldCharacter
    {
        private float MoveSpeed = 1;
        private int CharacterAnimationRow = 0;
        private Vector2 _cameraPostition;
        public Vector2 CameraPosition
        {
            get
            {
                return _cameraPostition;
            }
            set
            {
                _cameraPostition = value;
            }
        }
        public Point PositionOffset => new Point(
                                (int)((_cameraPostition.X - (int)_cameraPostition.X) * EngineSettings.TileSize),
                                (int)((_cameraPostition.Y - (int)_cameraPostition.Y) * EngineSettings.TileSize));
        int stepsSinceLastBattle = 0;
        int CharacterActiveTileX = 0;
        int CharacterActiveTileY = 0;
        int tx;
        int ty;

        Rectangle CharacterRectangle = new Rectangle(
            DisplayOutputSettings.CenterScreenTileOffsetX * EngineSettings.TileSize,
            DisplayOutputSettings.CenterScreenTileOffsetY * EngineSettings.TileSize,
            EngineSettings.TileSize, EngineSettings.TileSize);

        public Point WindowPosition => new Point(CharacterRectangle.X, CharacterRectangle.Y);


        public World_Character(ContentManager content, string[] texturePaths) : base(content, texturePaths)
        {
            //Initial "Start" position
            //TODO: Get this from Save
            _cameraPostition = new Vector2(550 - DisplayOutputSettings.CenterScreenTileOffsetX, 100 - DisplayOutputSettings.CenterScreenTileOffsetY);
        }

        public void Update(GameTime gameTime)
        {
            WalkSpeed();
            Vector2 CharacterMovement = UpdatePosition();
            if (CharacterMovement != Vector2.Zero)
            {
                _cameraPostition += CharacterMovement / EngineSettings.TileSize;
                if (_cameraPostition.X <= 0) _cameraPostition.X += WorldInformation.mapWidth;
                else if (_cameraPostition.X > WorldInformation.mapWidth) _cameraPostition.X -= WorldInformation.mapWidth;
                if (_cameraPostition.Y <= 0) _cameraPostition.Y += WorldInformation.mapHeight;
                else if (_cameraPostition.Y > WorldInformation.mapHeight) _cameraPostition.Y -= WorldInformation.mapHeight;

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
            Vector2 futurePos = _cameraPostition + (direction / EngineSettings.TileSize);

            CharacterActiveTileX = (int)((DisplayOutputSettings.CenterScreenTileOffsetX + 0.5 + (int)futurePos.X - futurePos.X) * EngineSettings.TileSize);
            CharacterActiveTileY = (int)((DisplayOutputSettings.CenterScreenTileOffsetY + 0.5 + (int)futurePos.Y - futurePos.Y) * EngineSettings.TileSize);

            tx = (int)(futurePos.X + DisplayOutputSettings.CenterScreenTileOffsetX);
            ty = (int)(futurePos.Y + DisplayOutputSettings.CenterScreenTileOffsetY + 1);
            return TilePalette.IsWalkable(tx, ty);
        }

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
            return _cameraPostition * EngineSettings.TileSize;
        }
        public Vector2 GetCenterTilePosition()
        {
            Vector2 location = _cameraPostition + new Vector2(
                DisplayOutputSettings.CenterScreenTileOffsetX,
                DisplayOutputSettings.CenterScreenTileOffsetY);
            return NormalizeAxis(location);
        }
        private Vector2 NormalizeAxis(Vector2 location)
        {
            while (location.X < 0)
            {
                location.X += WorldInformation.MapSize.X;
            }
            while (location.X >= WorldInformation.MapSize.X)
            {
                location.X -= WorldInformation.MapSize.X;
            }
            while (location.Y < 0)
            {
                location.Y += WorldInformation.MapSize.Y;
            }
            while (location.Y >= WorldInformation.MapSize.Y)
            {
                location.Y -= WorldInformation.MapSize.Y;
            }

            return location;
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
        public void Draw()
        {
            GSS.SpriteBatch.Draw(GetTexture(CharacterAnimationRow), CharacterRectangle, Color.White);

            if (GSS.DebugMode)
            {
                GSS.SpriteBatch.Draw(GetTexture(0),
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
