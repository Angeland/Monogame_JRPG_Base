using Microsoft.Xna.Framework;
using DreamsEnd.States.Characters;
using DreamsEnd.States.Characters.Items;
using System.Collections.Generic;

namespace DreamsEnd.States.Area.Cities
{
    public class City
    {
        Harbour _harbour;
        CityItemDemand _demand = new CityItemDemand();
        List<Citizen> Citizens = new List<Citizen>();
        List<Shop> Shops = new List<Shop>();
        public City(Vector2 HarbourLocation)
        {
            _harbour = new Harbour(HarbourLocation);
        }
        public Harbour GetHarbor()
        {
            return _harbour;
        }
        public int GetPrice(Item item)
        {
            int price = 0;
            foreach (var shop in Shops)
            {

            }
            return price;
        }
        public HarbourSpot GetOpenHarbourPosition()
        {
            return _harbour.getParkingSpot();
        }
        public List<Item> ItemsInDemand()
        {
            List<Item> inDemand = new List<Item>();
            foreach (var shop in Shops)
            {
            }
            return inDemand;
        }
    }
    public class CityItemDemand
    {
    }
    public class Citizen
    {
        string _firstName;
        string _surName;
        Citizen father;
        Citizen mother;
        List<Citizen> children = new List<Citizen>();
        IJob _job;
    }
}
