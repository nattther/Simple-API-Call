internal class Program
{
    private async static Task Main(string[] args)
    {
        ThreadPool.QueueUserWorkItem(async state =>
        {
            await MakeresquestWord();
        });

        ThreadPool.QueueUserWorkItem(async state =>
        {
            await MakeresquestRiot();
        });

        await Task.Delay(10000);

        Console.WriteLine("Les deux requetes ont bien été faites.");
    }
    private static async Task MakeresquestWord()
    {
        Console.WriteLine("Début de requete Word");
        HttpClient client = new HttpClient();
        client.BaseAddress = new Uri("https://wordsapiv1.p.rapidapi.com/");


        client.DefaultRequestHeaders.Add("X-RapidAPI-Key", "3aa1e89fdbmsh3c8713a8d3b0981p128c54jsna2fc9089afad");
        client.DefaultRequestHeaders.Add("X-RapidAPI-Host", "wordsapiv1.p.rapidapi.com");


        var response = await client.GetAsync("words/Stop");


        Console.WriteLine($"Status Code: {response.StatusCode} Content: {await response.Content.ReadAsStringAsync()}");
        Console.WriteLine("fin de requete Word");
    }
    private static async Task MakeresquestRiot()
    {
        Console.WriteLine("Début de requete Riot ");
        HttpClient client = new HttpClient();
        client.BaseAddress = new Uri("https://euw1.api.riotgames.com/lol/platform/v3/");




        var response = await client.GetAsync("champion-rotations?api_key=RGAPI-a34c9265-d674-403a-b58c-c115810e1df6");


        Console.WriteLine($"Status Code: {response.StatusCode} Content: {await response.Content.ReadAsStringAsync()}");
        Console.WriteLine("fin de requete Riot");
    }
}


