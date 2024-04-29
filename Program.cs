using System.Net.Http.Json;
using System.Text.Json.Serialization;


internal class Program
{
    private async static Task Main(string[] args)
    {
        string[] mots = { "love", "stop", "eat" };

        foreach (var mot in mots)
        {
            ThreadPool.QueueUserWorkItem(async state =>
            {
                await MakeRequestForWord(mot);
            });
        }

        ThreadPool.QueueUserWorkItem(async state =>
        {
            await MakeRequestForRiot();
        });

        await Task.Delay(5000);

        Console.WriteLine("Les requêtes ont bien été faites.");
    }

    private static async Task MakeRequestForWord(string word)
    {
        Console.WriteLine($"Début de la requête pour {word}");
        HttpClient client = new HttpClient();
        client.BaseAddress = new Uri("https://wordsapiv1.p.rapidapi.com/");
        client.DefaultRequestHeaders.Add("X-RapidAPI-Key", "3aa1e89fdbmsh3c8713a8d3b0981p128c54jsna2fc9089afad");
        client.DefaultRequestHeaders.Add("X-RapidAPI-Host", "wordsapiv1.p.rapidapi.com");

        var response = await client.GetAsync($"words/{word}");
        string jsonContent = await response.Content.ReadAsStringAsync();
        string filePath = $"{word}.json";
        await File.WriteAllTextAsync(filePath, jsonContent);

        WordApiResponse result = await response.Content.ReadFromJsonAsync<WordApiResponse>();
        Console.WriteLine("Résultat");
        foreach (Result item in result.Results)
        {
            Console.WriteLine($"Définition : {item.Definition}");
        }

        Console.WriteLine($"Fin de la requête pour {word}");
    }

    private static async Task MakeRequestForRiot()
    {
        Console.WriteLine("Début de la requête pour Riot ");
        HttpClient client = new HttpClient();
        client.BaseAddress = new Uri("https://euw1.api.riotgames.com/lol/platform/v3/");

        var response = await client.GetAsync("champion-rotations?api_key=RGAPI-a34c9265-d674-403a-b58c-c115810e1df6");

        Console.WriteLine($"Code de statut : {response.StatusCode}");

        ChampionRotation rotation = await response.Content.ReadFromJsonAsync<ChampionRotation>();

        Console.WriteLine("Champions gratuits:");
        foreach (int id in rotation.FreeChampionIds)
        {
            Console.Write(id + " ");
        }

        Console.WriteLine("Fin de la requête pour Riot");
    }

    public class ChampionRotation
    {
        [JsonPropertyName("freeChampionIds")]
        public int[] FreeChampionIds { get; set; }

        [JsonPropertyName("freeChampionIdsForNewPlayers")]
        public int[] FreeChampionIdsForNewPlayers { get; set; }

        [JsonPropertyName("maxNewPlayerLevel")]
        public int MaxNewPlayerLevel { get; set; }
    }

    public class WordApiResponse
    {
        [JsonPropertyName("word")]
        public string Word { get; set; }

        [JsonPropertyName("results")]
        public List<Result> Results { get; set; }

        [JsonPropertyName("syllables")]
        public Syllables Syllables { get; set; }

        [JsonPropertyName("pronunciation")]
        public Pronunciation Pronunciation { get; set; }

        [JsonPropertyName("frequency")]
        public double Frequency { get; set; }
    }

    public class Result
    {
        [JsonPropertyName("definition")]
        public string Definition { get; set; }

        [JsonPropertyName("partOfSpeech")]
        public string PartOfSpeech { get; set; }

        [JsonPropertyName("synonyms")]
        public List<string> Synonyms { get; set; }
    }

    public class Syllables
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }

        [JsonPropertyName("list")]
        public List<string> List { get; set; }
    }

    public class Pronunciation
    {
        [JsonPropertyName("all")]
        public string All { get; set; }
    }
}
