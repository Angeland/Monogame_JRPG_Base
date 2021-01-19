using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using DreamsEnd.AI;
using DreamsEnd.Library;
using DreamsEnd.States.Area.Cities;
using DreamsEnd.States.Configuration;
using DreamsEnd.States.World;
using System;
using System.Collections.Generic;
using System.Threading;

namespace DreamsEnd.States.AutoElements
{
    public class Ship
    {
        private Pathfinder _pf = new Pathfinder();
        private TimeSpan _waitTime;
        private readonly Texture2D _texture;
        private Vector2 _size;
        private Vector2 _halfSize;
        private Vector2 _position;
        private Vector2 _cameraPos;
        private Vector2 _startPosition;
        private Vector2 _goalPosition;
        private Vector2 _fromOff = Vector2.Zero;
        private readonly Harbour _home;
        private readonly Harbour _destination;
        private List<Vector2> _path = new List<Vector2>();
        private List<Vector2> _pathAtoB = new List<Vector2>();
        private List<Vector2> _pathBtoA = new List<Vector2>();
        private readonly Cargo _cargo;
        private int _positionIndex = 0;
        private bool _pathCalculated = false;
        private bool _forward = true;
        private readonly float _speed = 22000f;
        private readonly float _baseSpeed;

        //Timers for animation
        private double _oldTime = 0;
        private float _timer = 0;

        //Sprite Rotation
        private float _rotationAngle = 0;
        private Vector2 _origin = Vector2.Zero;

        public Ship(TimeSpan waitTime, Texture2D texture, Cargo cargo, Harbour Home, Harbour Destination, int speed)
        {
            _fromOff = new Vector2((WorldInformation.mapWidth / 2) - Home.getLocation().X, (WorldInformation.mapHeight / 2) - Home.getLocation().Y);
            _waitTime = waitTime;
            _texture = texture;
            _cargo = cargo;
            _home = Home;
            _destination = Destination;
            _position = Home.getLocation() * EngineSettings.TileSize;
            _baseSpeed = speed * GSS.ClockSpeed;
            _speed = (500f / _baseSpeed);
            _size = new Vector2(texture.Width, texture.Height);
            _halfSize = _size / 2;
            _origin = _size / 2;
        }
        public Vector2 GetPos()
        {
            return _position;
        }
        public void BuildPreRequisites()
        {
            new Thread(new ThreadStart(() =>
            {
                _pf = new Pathfinder();
                _pathAtoB = GetPath(_home.getLocation(), _destination.getLocation());
                _pf = new Pathfinder();
                _pathBtoA = GetPath(_destination.getLocation(), _home.getLocation());
                SetPath();
                _pathCalculated = true;
            })).Start();
        }
        private void SetPath()
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
                    SetPath();
                    _timer -= _speed;
                    _startPosition = _path[_positionIndex++] * EngineSettings.TileSize;
                    _goalPosition = _path[_positionIndex] * EngineSettings.TileSize;
                }
                PlaceShipOnPosition();
            }
            _cameraPos = GSS.World.worldCharacter.GetPosition();
        }

        void PlaceShipOnPosition()
        {
            if (_path.Count > _positionIndex && _positionIndex != 0)
            {
                float animPart = _timer / _speed;
                float ProgressX = (_goalPosition.X - _startPosition.X) * animPart;
                float ProgressY = (_goalPosition.Y - _startPosition.Y) * animPart;

                Vector2 newPos = new Vector2(_startPosition.X + (ProgressX), _startPosition.Y + (ProgressY));
                GetRotation();
                _position = newPos;
            }
        }
        private void GetRotation()
        {
            _rotationAngle = (float)Math.Atan2(_goalPosition.Y - _startPosition.Y, _goalPosition.X - _startPosition.X);
            float circle = MathHelper.Pi * 2;
            _rotationAngle %= circle;
        }
        List<Vector2> GetPath(Vector2 A, Vector2 B)
        {
            return _pf.GetPath(A, B, _size);
        }
        public bool WithinCamera(Vector2 cameraPos)
        {
            return CameraHelp.WithinCamera(cameraPos, _position, _size);
        }
        public void Draw()
        {
            if (WithinCamera(_cameraPos))
            {
                GSS.SpriteBatch.Draw(_texture,
                     DrawHelp.GetBoundary(_cameraPos, _position, _size),
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
            GSS.SpriteBatch.Draw(_texture,
                 new Rectangle((int)(_position.X / EngineSettings.TileSize) - 5, (int)(_position.Y / EngineSettings.TileSize) - 5, 10, 10), Color.Red);
        }
    }
}
