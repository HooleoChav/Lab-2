namespace VideoGame2
{
    internal class Program
    {
        // VideoGame class remains unchanged
        class VideoGame : IComparable<VideoGame>
        {
            public string Name { get; set; }
            public string Platform { get; set; }
            public int Year { get; set; }
            public string Genre { get; set; }
            public string Publisher { get; set; }
            public float NA_Sales { get; set; }
            public float EU_Sales { get; set; }
            public float JP_Sales { get; set; }
            public float Other_Sales { get; set; }
            public float Global_Sales { get; set; }

            public int CompareTo(VideoGame other)
            {
                return string.Compare(Name, other.Name, StringComparison.Ordinal);
            }

            public override string ToString()
            {
                return $"Name: {Name}, Year: {Year}, Genre: {Genre}, Publisher: {Publisher}, " + $"Global_Sales: {Global_Sales}";
            }
        }

        // Grouping by platform (LINQ i suck at this)
        static Dictionary<string, List<VideoGame>> groupByPlatform(List<VideoGame> games)
        {
            Dictionary<string, List<VideoGame>> platformDictionary = games
                .GroupBy(game => game.Platform)
                .ToDictionary(group => group.Key, group => group.ToList());

            return platformDictionary;
        }

        // LAMBA FUNTION that returns the top 5 video games
        static Func<List<VideoGame>, List<VideoGame>> topFiveGames = games =>
            games.OrderByDescending(game => game.Global_Sales).Take(5).ToList();

        static void Main(string[] args)
        {
            string filePath = "videogames.csv";
            List<VideoGame> videoGames = new List<VideoGame>();

            using (StreamReader reader = new StreamReader(filePath))
            {
                reader.ReadLine(); // Skip the header line with all the column names n stuff (why this take me 2 hours to figure out).
                while (!reader.EndOfStream)
                {
                    string[] columns = reader.ReadLine().Split(',');
                    VideoGame videoGame = new VideoGame
                    {
                        Name = columns[0],
                        Platform = columns[1],
                        Year = int.Parse(columns[2]),
                        Genre = columns[3],
                        Publisher = columns[4],
                        NA_Sales = float.Parse(columns[5]),
                        EU_Sales = float.Parse(columns[6]),
                        JP_Sales = float.Parse(columns[7]),
                        Other_Sales = float.Parse(columns[8]),
                        Global_Sales = float.Parse(columns[9])
                    };
                    videoGames.Add(videoGame);
                }
            }

            // Group by platform and display top 5 games for each platform
            Dictionary<string, List<VideoGame>> platformDictionary = groupByPlatform(videoGames);

            foreach (var platform in platformDictionary.Keys)
            {
                Console.WriteLine($"\nTop 5 Games on the '{platform}':");
                var top5Games = topFiveGames(platformDictionary[platform]);

                foreach (var game in top5Games)
                {
                    Console.WriteLine(game);
                }
            }
        }
    }
}