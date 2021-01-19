using Microsoft.Xna.Framework;
using DreamsEnd.States.AutoElements;
using System.Collections.Generic;
using System.Linq;

namespace DreamsEnd.States.Area.Cities
{
    public class Harbour
    {
        Vector2 _location = Vector2.Zero;
        public Harbour(Vector2 Location)
        {
            _location = Location;
        }
        public Vector2 getLocation()
        {
            return _location;
        }
        List<HarbourSpot> _spots = new List<HarbourSpot>();
        public void addSpot(Vector2 position)
        {
            _spots.Add(new HarbourSpot(position));
        }
        public HarbourSpot getParkingSpot()
        {
            return _spots.First(a => !a.Filled());
        }
    }
    public class HarbourSpot
    {
        Ship _ship = null;
        Vector2 _position = Vector2.Zero;
        public HarbourSpot(Vector2 Position)
        {
            _position = Position;
        }
        public void ParkShip(Ship ship)
        {
            _ship = ship;
        }
        public void UnParkShip(Ship ship)
        {
            _ship = null;
        }
        public bool Filled()
        {
            return _ship == null ? false : true;
        }
    }
}
