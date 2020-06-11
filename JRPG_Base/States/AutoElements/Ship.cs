using JRPG_Base.AI;
using JRPG_Base.Library;
using JRPG_Base.States.Area.Cities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace JRPG_Base.States.AutoElements
{
    public class Ship
    {
        private Pathfinder
            _pf = new Pathfinder();
        private TimeSpan
            _waitTime;
        private Texture2D
            _texture;
        private Vector2
            _size,
            _position,
            _cameraPos,
            _startPosition,
            _goalPosition,
            _fromOff = Vector2.Zero;
        private Harbour
            _home,
            _destination;
        private List<Vector2>
            _path = new List<Vector2>(),
            _pathAtoB = new List<Vector2>(),
            _pathBtoA = new List<Vector2>();
        private Cargo
            _cargo;
        private int
            _positionIndex = 0;
        private bool
            _pathCalculated = false,
            _forward = true;
        private float
            _speed = 22000f,
            _baseSpeed;
        //Timers for animation
        double _oldTime = 0;
        float _timer = 0;

        //Sprite Rotation
        float _rotationAngle = 0;
        Vector2 _origin = Vector2.Zero;

        public Ship(TimeSpan waitTime, Texture2D texture, Cargo cargo, Harbour Home, Harbour Destination, int speed)
        {
            this._fromOff = new Vector2((GSS.world.mapWidth / 2) - Home.getLocation().X, (GSS.world.mapHeight / 2) - Home.getLocation().Y);
            this._waitTime = waitTime;
            this._texture = texture;
            this._cargo = cargo;
            this._home = Home;
            this._destination = Destination;
            this._position = Home.getLocation() * GSS.TileSize;
            this._baseSpeed = speed * GSS.ClockSpeed;
            this._speed = (500f / _baseSpeed);
            this._size = new Vector2(texture.Width, texture.Height);
            _origin = _size / 2;
        }
        public Vector2 getPos()
        {
            return _position;
        }
        public void BuildPreRequisites()
        {
            new Thread(new ThreadStart(() =>
            {
                _pf = new Pathfinder();
                _pathAtoB = getPath(_home.getLocation(), _destination.getLocation());
                _pf = new Pathfinder();
                _pathBtoA = getPath(_destination.getLocation(), _home.getLocation());
                setPath();
                _pathCalculated = true;
            })).Start();
        }
        private void setPath()
        {
            if (_positionIndex >= _path.Count - 1)
            {
                _positionIndex = 0;
                if (_forward)
                {
                    _forward = !_forward;
                    _path = _pathBtoA;
                }
                else
                {
                    _forward = !_forward;
                    _path = _pathAtoB;
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            if (_pathCalculated)
            {
                //Noe funky må finne på bedre timer
                double storeTime = gameTime.TotalGameTime.TotalMilliseconds;
                _timer += (float)(storeTime - _oldTime);
                _oldTime = storeTime;

                if (_speed <= _timer)
                {
                    setPath();
                    _timer -= _speed;
                    _startPosition = _path[_positionIndex++] * GSS.TileSize;
                    _goalPosition = _path[_positionIndex] * GSS.TileSize;
                }
                placeShipOnPosition();
            }
            _cameraPos = GSS.world.worldCharacter.getPosition();
        }

        void placeShipOnPosition()
        {
            if (_path.Count > _positionIndex && _positionIndex != 0)
            {
                float animPart = _timer / _speed;
                float ProgressX = (_goalPosition.X - _startPosition.X) * animPart;
                float ProgressY = (_goalPosition.Y - _startPosition.Y) * animPart;
                
                Vector2 newPos= new Vector2(_startPosition.X + (ProgressX), _startPosition.Y + (ProgressY));
                getRotation();
                _position = newPos;
            }
        }
        private void getRotation()
        {
            _rotationAngle = (float)Math.Atan2(_goalPosition.Y - _startPosition.Y, _goalPosition.X - _startPosition.X);
            float circle = MathHelper.Pi * 2;
            _rotationAngle = _rotationAngle % circle;
        }
        List<Vector2> getPath(Vector2 A, Vector2 B)
        {
            return _pf.getPath(A, B, _size);
        }
        public bool WithinCamera(Vector2 cameraPos)
        {
            return CameraHelp.WithinCamera(cameraPos, _position, _size);
        }
        public void Draw()
        {
            if (WithinCamera(_cameraPos))
            {
                GSS.spriteBatch.Draw(_texture,
                     DrawHelp.getShape(_cameraPos, _position, _size),
                     null, Color.White, _rotationAngle, _origin, SpriteEffects.None, 0);
            }
            Debug();
        }
        private void Debug()
        {
            if (GSS.DebugMode)
            {
                if (_pathCalculated)
                {
                    DrawOnMap();
                }
                else
                {
                    _pf.Draw(_texture);
                }
            }
        }
        private void DrawOnMap()
        {
            GSS.spriteBatch.Draw(_texture,
                 new Rectangle((int)(_position.X / GSS.TileSize) - 5, (int)(_position.Y / GSS.TileSize) - 5, 10, 10), Color.Red);
        }
    }
}
