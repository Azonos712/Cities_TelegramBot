namespace CitiesTelegramBot
{
    class User
    {
        public string Nick { get; set; }
        public long Id { get; set; }
        public User(string name, long id)
        {
            Nick = name;
            Id = id;
        }
    }
}
