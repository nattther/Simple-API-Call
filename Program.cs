using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http.Json;
using System.Reflection.Metadata;


internal class Program
{
    private async static Task Main(string[] args)
    {
        string[] mots = { "love", "stop", "eat" };


        ThreadPool.QueueUserWorkItem(async state =>
        {
            await MakeresquestWord("love");
        });
        ThreadPool.QueueUserWorkItem(async state =>
        {
            await MakeresquestWord("eat");
        });
        ThreadPool.QueueUserWorkItem(async state =>
        {
            await MakeresquestRiot();
        });

        await Task.Delay(5000);

        Console.WriteLine("Les requetes ont bien été faites.");
    }
    private static async Task MakeresquestWord(string mot)
    {
        Console.WriteLine($"Début de requête {mot}");
        HttpClient client = new HttpClient();
        client.BaseAddress = new Uri("https://wordsapiv1.p.rapidapi.com/");

        client.DefaultRequestHeaders.Add("X-RapidAPI-Key", "3aa1e89fdbmsh3c8713a8d3b0981p128c54jsna2fc9089afad");
        client.DefaultRequestHeaders.Add("X-RapidAPI-Host", "wordsapiv1.p.rapidapi.com");

        var response = await client.GetAsync($"words/{mot}");


        string jsonContent = await response.Content.ReadAsStringAsync();

        string filePath = $"{mot}.json";


        await File.WriteAllTextAsync(filePath, jsonContent);

        Console.WriteLine($"Le contenu a été enregistré avec succès dans {filePath}");


        WordApiResponse resultat = await response.Content.ReadFromJsonAsync<WordApiResponse>();
        Console.WriteLine("Résultat");
        foreach (Result result in resultat.Results)
        {
            Console.WriteLine($"Définition : {result.Definition}");
        }



        Console.WriteLine($"Fin de requête {mot}");
    }
    private static async Task MakeresquestRiot()
    {
        {
            Console.WriteLine("Début de requete Riot ");
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://euw1.api.riotgames.com/lol/platform/v3/");

            var response = await client.GetAsync("champion-rotations?api_key=RGAPI-a34c9265-d674-403a-b58c-c115810e1df6");

            Console.WriteLine($"Status Code: {response.StatusCode}");


            ChampionRotation rotation = await response.Content.ReadFromJsonAsync<ChampionRotation>();

            Console.WriteLine("Free Champion IDs:");
            foreach (int id in rotation.FreeChampionIds)
            {
                Console.Write(id + " ");
            }

            Console.WriteLine("Fin de requete Riot");
        }
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

