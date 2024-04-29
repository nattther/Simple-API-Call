using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Net.Http.Json;


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
        Console.WriteLine("Début de requête Word");
        HttpClient client = new HttpClient();
        client.BaseAddress = new Uri("https://wordsapiv1.p.rapidapi.com/");

        client.DefaultRequestHeaders.Add("X-RapidAPI-Key", "3aa1e89fdbmsh3c8713a8d3b0981p128c54jsna2fc9089afad");
        client.DefaultRequestHeaders.Add("X-RapidAPI-Host", "wordsapiv1.p.rapidapi.com");

        var response = await client.GetAsync("words/Stop");


        string jsonContent = await response.Content.ReadAsStringAsync();

        string filePath = "word.json";


        await File.WriteAllTextAsync(filePath, jsonContent);

        Console.WriteLine($"Le contenu a été enregistré avec succès dans {filePath}");



        Console.WriteLine("Fin de requête Word");
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
}

