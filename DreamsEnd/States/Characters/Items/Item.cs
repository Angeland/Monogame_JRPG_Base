namespace DreamsEnd.States.Characters.Items
{
    public class Item
    {
        string _name;
        string _description;
        public Item(string name, string description = "")
        {
            SetName(name);
            SetDescription(description);
        }
        public string GetName()
        {
            return _name;
        }
        public void SetName(string name)
        {
            _name = name;
        }
        public string GetDescription()
        {
            return _description;
        }
        public void SetDescription(string description)
        {
            _description = description;
        }
    }
}
