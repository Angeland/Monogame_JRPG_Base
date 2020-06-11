using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JRPG_Base.States.Characters.Items
{
    public class Item
    {
        string _name;
        string _description;
        public Item(string name, string description = "")
        {
            setName(name);
            setDescription(description);
        }
        public string getName()
        {
            return _name;
        }
        public void setName(string name)
        {
            _name = name;
        }
        public string getDescription()
        {
            return _description;
        }
        public void setDescription(string description)
        {
            _description = description;
        }
    }
}
