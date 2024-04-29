internal class Program
{
private async static Task Main(string[] args)
{
    HttpClient client = new HttpClient();
    client.BaseAddress = new Uri("https://wordsapiv1.p.rapidapi.com/");


    client.DefaultRequestHeaders.Add("X-RapidAPI-Key", "3aa1e89fdbmsh3c8713a8d3b0981p128c54jsna2fc9089afad");
    client.DefaultRequestHeaders.Add("X-RapidAPI-Host", "wordsapiv1.p.rapidapi.com");


    var response = await client.GetAsync("words/lasagne");


    Console.WriteLine($"Status Code: {response.StatusCode} Content: {await response.Content.ReadAsStringAsync()}");
    Console.ReadLine();
}

    }
