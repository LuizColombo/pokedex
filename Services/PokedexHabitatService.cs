using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using pokedex.Models;
using System.Collections.Generic;

namespace pokedex.Services
{
    public class PokedexHabitatService
    {
        private readonly HttpClient _httpClient;
        private readonly PokedexService _pokedexService;

        public PokedexHabitatService(IHttpClientFactory httpClientFactory, PokedexService pokedexService)
        {
            _httpClient = httpClientFactory.CreateClient();
            _pokedexService = pokedexService;
        }

        public async Task<(List<Pokemon> Pokemons, int TotalCount)> GetPokemonsByHabitatAsync(
            string habitatName, int page = 1, int pageSize = 10)
        {
            var response = await _httpClient.GetAsync(
                $"https://pokeapi.co/api/v2/pokemon-habitat/{habitatName.ToLower()}");

            if (!response.IsSuccessStatusCode)
                return (new List<Pokemon>(), 0);

            var json = await response.Content.ReadAsStringAsync();
            var data = JObject.Parse(json);

            var speciesList = data["pokemon_species"].ToList();
            var totalCount = speciesList.Count;

            // paginação manual
            int skip = (page - 1) * pageSize;
            var pagedSpecies = speciesList.Skip(skip).Take(pageSize);

            var pokemons = new List<Pokemon>();

            foreach (var species in pagedSpecies)
            {
                var name = species["name"].ToString();

                var pokemon = await _pokedexService.GetPokemonAsync(name);
                if (pokemon != null)
                {
                    pokemons.Add(pokemon);
                }
            }

            return (pokemons, totalCount);
        }

    }
}
