using System.Collections.Generic;

namespace CitiesTelegramBot
{
    class Game
    {
        public User Player { get; set; }
        public string LastCity { get; set; }
        public char LastLetter
        {
            get
            {
                return Utility.GetLastLetter(LastCity);
            }
        }

        public List<uint> Cities { get; set; }

        public Game(string name, long id)
        {
            Player = new User(name, id);
            Cities = new List<uint>();
        }

        public bool AddCity(uint id)
        {
            if (Cities.Contains(id))
                return false;

            Cities.Add(id);
            return true;
        }
    }
}
