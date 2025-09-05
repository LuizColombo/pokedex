// Controllers/PokedexController.cs
using Microsoft.AspNetCore.Mvc;
using pokedex.Services;
using System.Threading.Tasks;

namespace pokedex.Controllers
{
    public class PokedexController : Controller
    {
        private readonly PokedexService _pokedexService;
        private readonly PokedexHabitatService _pokedexHabitatService;

        public PokedexController(PokedexService pokedexService, PokedexHabitatService pokedexHabitatService)
        {
            _pokedexService = pokedexService;
            _pokedexHabitatService = pokedexHabitatService;
        }

        #region Pokemon By Name or ID

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Search(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return View("Index");

            var pokemon = await _pokedexService.GetPokemonAsync(name);
            if (pokemon == null)
            {
                ViewBag.Error = "Pokémon não encontrado.";
                return View("Index");
            }

            return View("Index", pokemon);
        }

        #endregion

        #region Pokemons By Habitat
            public IActionResult PokemonsHabitat()
            {
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> SearchPokemonsHabitat(string habitat)
            {
                if (string.IsNullOrWhiteSpace(habitat))
                    return View("PokemonsHabitat");

                var pokemons = await _pokedexHabitatService.GetPokemonsByHabitatAsync(habitat);
                if (pokemons == null || pokemons.Count == 0)
                {
                    ViewBag.Error = "Habitat não encontrado.";
                    return View("PokemonsHabitat");
                }

                return View("PokemonsHabitat", pokemons); // passa a lista para a View
            }
            
        #endregion
    }
}
