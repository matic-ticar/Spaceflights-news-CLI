using Newtonsoft.Json;

namespace Spaceflight_News
{
    class Program
    {
        static readonly string API_URL = "https://api.spaceflightnewsapi.net/v3";
        static int NEWS_LIMIT = 10;

        static void ChangeNewsLimit()
        {
            Console.Write("Enter news limit: ");
            var limit = int.Parse(Console.ReadLine()!);

            if (limit <= 0)
            {
                Console.WriteLine("Limit must be greater than 0. \n");
                return;
            }

            NEWS_LIMIT = limit;
        }

        class News
        {
            public int Id { get; set; }
            public string? Title { get; set; }
            public string? Url { get; set; }
            public string? Summary { get; set; }
            public string? PublishedAt { get; set; }
        }

        readonly HttpClient client = new();
        private async Task<List<News>> FetchNews(string path)
        {
            var url = API_URL + path + $"?_limit={NEWS_LIMIT}";
            string res = await client.GetStringAsync(url);
            List<News> news = JsonConvert.DeserializeObject<List<News>>(res)!;

            return news;
        }

        static async Task PrintNews(string kind)
        {
            Program program = new();

            var path = $"/{kind}";
            var news = await program.FetchNews(path);

            Console.WriteLine(kind.ToUpper());

            for (int i = 0; i < news.Count; i++)
            {
                Console.WriteLine($"[{i + 1}] {news[i].Title}");
            }

            SelectedNews(news);
        }

        static void SelectedNews(List<News> news)
        {
            Console.Write("\n#");
            var newsIndex = int.Parse(Console.ReadLine()!);

            if (newsIndex < 0 || newsIndex > news.Count)
            {
                Console.WriteLine("Index must be in range of shown news. \n");
                return;
            }

            newsIndex--;

            Console.WriteLine($"Title: {news[newsIndex].Title} \n");

            var publishedAt = DateTime.Parse(news[newsIndex].PublishedAt!);
            Console.WriteLine($"Published at: {publishedAt:MM/dd/yyyy HH:mm}");
            Console.WriteLine($"URL: {news[newsIndex].Url} \n");

            Console.WriteLine("Summary:");
            Console.WriteLine(news[newsIndex].Summary);

            Console.ReadLine();
        }

        static void NewsMenu()
        {
            Console.WriteLine("Choose type of news:");
            Console.WriteLine("[1] Articles");
            Console.WriteLine("[2] Blogs");
            Console.WriteLine("[3] Reports");
            Console.WriteLine("[4] News limit");
            Console.WriteLine("[q] Quit");

            Console.Write("\n#");
            var action = char.Parse(Console.ReadLine()!);

            string kind;

            switch (action)
            {
                case '1':
                    kind = "articles";
                    break;
                case '2':
                    kind = "blogs";
                    break;
                case '3':
                    kind = "reports";
                    break;
                case '4':
                    ChangeNewsLimit();
                    return;
                case 'q':
                    Environment.Exit(0);
                    return;
                default:
                    Console.WriteLine("Unknown action. \n");
                    return;
            }

            PrintNews(kind).Wait();
        }


        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Spaceflight News! \n");
            Run();
        }

        static void Run()
        {
            while (true)
            {
                NewsMenu();
            }
        }

    }
}