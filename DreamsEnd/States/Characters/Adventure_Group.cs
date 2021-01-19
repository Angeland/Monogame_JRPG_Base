using DreamsEnd.States.Characters.Items;
using System.Collections.Generic;

namespace DreamsEnd.States.Characters
{
    class Adventure_Group
    {
        int Krown = 0;
        List<Item> ItemList = new List<Item>();
        List<Character> GroupList = new List<Character>();
        public Adventure_Group()
        {
            //Start with 2 potions, Canoe & 500 Krowns

            Krown += 500;
        }

    }
}
