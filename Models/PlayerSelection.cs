namespace DotNetAuthentication.DB
{
    public class PlayerSelection
    {
        public PlayerSelection()
        {
        }

        public PlayerSelection(string teamName, int player_Key, int userId)
        {
            TeamName = teamName;
            Player_key = player_Key;
            UserId = userId;
        }

        public string TeamName { get; set; }
        public int Player_key { get; set; }
        public int UserId { get; set; }

    }
}