using SQLite;

namespace Mastermind
{
    class ScoresItem
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        public string Time { get; set; }
    }
}
