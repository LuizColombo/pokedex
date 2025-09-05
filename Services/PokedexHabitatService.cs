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

        public async Task<List<Pokemon>> GetPokemonsByHabitatAsync(string habitatName)
        {
            var response = await _httpClient.GetAsync($"https://pokeapi.co/api/v2/pokemon-habitat/{habitatName.ToLower()}");
            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            var data = JObject.Parse(json);

            var speciesList = data["pokemon_species"];
            var pokemons = new List<Pokemon>();

            foreach (var species in speciesList)
            {
                var name = species["name"].ToString();

                // Usa o serviço já existente para pegar o Pokémon
                var pokemon = await _pokedexService.GetPokemonAsync(name);
                if (pokemon != null)
                {
                    pokemons.Add(pokemon);
                }
            }

            return pokemons;
        }
    }
}
