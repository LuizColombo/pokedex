using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using pokedex.Models;
using System.Collections.Generic;

namespace pokedex.Services
{
    public class PokedexTypeService
    {
        private readonly HttpClient _httpClient;
        private readonly PokedexService _pokedexService;

        public PokedexTypeService(IHttpClientFactory httpClientFactory, PokedexService pokedexService)
        {
            _httpClient = httpClientFactory.CreateClient();
            _pokedexService = pokedexService;
        }

        // public async Task<(List<Pokemon> Pokemons, int TotalCount)> GetPokemonsByTypeAsync(
        //     string typeName, int page = 1, int pageSize = 10)
        // {
        //     var response = await _httpClient.GetAsync(
        //         $"https://pokeapi.co/api/v2/type/{typeName.ToLower()}");

        //     if (!response.IsSuccessStatusCode)
        //         return (new List<Pokemon>(), 0);

        //     var json = await response.Content.ReadAsStringAsync();
        //     var data = JObject.Parse(json);

        //     var typeList = data["pokemon"].ToList();
        //     var totalCount = typeList.Count;

        //     // paginação manual
        //     int skip = (page - 1) * pageSize;
        //     var pagedTypes = typeList.Skip(skip).Take(pageSize);

        //     var pokemons = new List<Pokemon>();

        //     foreach (var type in pagedTypes)
        //     {
        //         var name = type["pokemon"]?["name"]?.ToString();

        //         var pokemon = await _pokedexService.GetPokemonAsync(name);
        //         if (pokemon != null)
        //         {
        //             pokemons.Add(pokemon);
        //         }
        //     }

        //     return (pokemons, totalCount);
        // }
        // public async Task<(List<Pokemon> Pokemons, int TotalCount)> GetPokemonsByTypeAsync(
        //     string typeName, int page = 1, int pageSize = 10)
        // {
        //     if (string.IsNullOrWhiteSpace(typeName))
        //         return (new List<Pokemon>(), 0);

        //     var response = await _httpClient.GetAsync(
        //         $"https://pokeapi.co/api/v2/type/{typeName.ToLower()}");

        //     if (!response.IsSuccessStatusCode)
        //         return (new List<Pokemon>(), 0);

        //     var json = await response.Content.ReadAsStringAsync();
        //     var data = JObject.Parse(json);

        //     // Aqui pegamos o array de pokemons
        //     var pokemonArray = data["pokemon"] as JArray;
        //     if (pokemonArray == null || !pokemonArray.Any())
        //         return (new List<Pokemon>(), 0);

        //     var totalCount = pokemonArray.Count;

        //     // paginação
        //     int skip = (page - 1) * pageSize;
        //     var pagedPokemons = pokemonArray.Skip(skip).Take(pageSize);

        //     var pokemons = new List<Pokemon>();

        //     foreach (var item in pagedPokemons)
        //     {
        //         // pega o nome corretamente
        //         var name = item["pokemon"]?["name"]?.ToString();
        //         if (string.IsNullOrWhiteSpace(name))
        //             continue;

        //         var pokemon = await _pokedexService.GetPokemonAsync(name);
        //         if (pokemon != null)
        //             pokemons.Add(pokemon);
        //     }

        //     return (pokemons, totalCount);
        // }
        public async Task<(List<Pokemon> Pokemons, int TotalCount)> GetPokemonsByTypeAsync(
            string typeName, int page = 1, int pageSize = 10)
        {
            if (string.IsNullOrWhiteSpace(typeName))
                return (new List<Pokemon>(), 0);

            var response = await _httpClient.GetAsync($"https://pokeapi.co/api/v2/type/{typeName.ToLower()}");
            if (!response.IsSuccessStatusCode)
                return (new List<Pokemon>(), 0);

            var json = await response.Content.ReadAsStringAsync();
            var data = JObject.Parse(json);

            var pokemonArray = data["pokemon"] as JArray;
            if (pokemonArray == null || !pokemonArray.Any())
                return (new List<Pokemon>(), 0);

            var totalCount = pokemonArray.Count;

            // paginação
            int skip = (page - 1) * pageSize;
            var pagedPokemons = pokemonArray.Skip(skip).Take(pageSize);

            var pokemons = new List<Pokemon>();

            foreach (var item in pagedPokemons)
            {
                var pokeObj = item["pokemon"];
                if (pokeObj == null) continue;

                var name = pokeObj["name"]?.ToString();
                var url = pokeObj["url"]?.ToString();
                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(url)) continue;

                // Busca a informação completa direto da URL
                var pokeResponse = await _httpClient.GetAsync(url);
                if (!pokeResponse.IsSuccessStatusCode) continue;

                var pokeJson = await pokeResponse.Content.ReadAsStringAsync();
                var pokeData = JObject.Parse(pokeJson);

                var pokemon = new Pokemon
                {
                    Id = (int)pokeData["id"],
                    Name = pokeData["name"]?.ToString(),
                    Sprite = pokeData["sprites"]?["front_default"]?.ToString(),
                    Types = pokeData["types"]?
                                .Select(t => t["type"]?["name"]?.ToString())
                                .Where(t => !string.IsNullOrWhiteSpace(t))
                                .ToList() ?? new List<string>()
                };

                pokemons.Add(pokemon);
            }

            return (pokemons, totalCount);
        }


    }
}
