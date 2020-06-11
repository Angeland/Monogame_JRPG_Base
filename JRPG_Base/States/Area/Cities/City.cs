using JRPG_Base.States.Characters;
using JRPG_Base.States.Characters.Items;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JRPG_Base.States.Area.Cities
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
        public Harbour getHarbor()
        {
            return _harbour;
        }
        public int getPrice(Item item)
        {
            int price = 0;
            foreach (var shop in Shops)
            {

            }
            return price;
        }
        public HarbourSpot getOpenHarbourPosition()
        {
            return _harbour.getParkingSpot();
        }
        public List<Item> itemsInDemand()
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
        Job _job;
    }
}
