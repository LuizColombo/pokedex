// Services/PokedexService.cs
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using pokedex.Models;
using System.Collections.Generic;

namespace pokedex.Services
{
    public class PokedexService
    {
        private readonly HttpClient _httpClient;

        public PokedexService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<Pokemon> GetPokemonAsync(string name)
        {
            var response = await _httpClient.GetAsync($"https://pokeapi.co/api/v2/pokemon/{name.ToLower()}");
            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            var pokemon = JObject.Parse(json);

            var types = new List<string>();
            foreach (var t in pokemon["types"])
            {
                types.Add(t["type"]["name"].ToString());
            }

            return new Pokemon
            {
                Name = pokemon["name"].ToString(),
                Id = (int)pokemon["id"],
                Sprite = pokemon["sprites"]["front_default"].ToString(),
                Types = types,
                Height = (int)pokemon["height"],
                Weight = (int)pokemon["weight"]
            };
        }
    }
}
