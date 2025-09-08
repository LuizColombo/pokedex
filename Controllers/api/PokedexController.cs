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
        private readonly PokedexTypeService _pokedexTypeService;

        public PokedexController(PokedexService pokedexService, PokedexHabitatService pokedexHabitatService, PokedexTypeService pokedexTypeService)
        {
            _pokedexService = pokedexService;
            _pokedexHabitatService = pokedexHabitatService;
            _pokedexTypeService = pokedexTypeService;
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

        public async Task<IActionResult> SearchPokemonsHabitat(string habitat, int page = 1)
        {
            int pageSize = 10;
            var (pokemons, totalCount) = await _pokedexHabitatService.GetPokemonsByHabitatAsync(habitat, page, pageSize);

            ViewBag.Habitat = habitat;
            ViewBag.Page = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            return View("PokemonsHabitat", pokemons);
        }

        #endregion

        #region Type

        public IActionResult PokemonsType()
        {
            return View();
        }

        public async Task<IActionResult> SearchPokemonsType(string type, int page = 1)
        {
            int pageSize = 10;
            var (pokemons, totalCount) = await _pokedexTypeService.GetPokemonsByTypeAsync(type, page, pageSize);

            ViewBag.Type = type;
            ViewBag.Page = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            return View("PokemonsType", pokemons);
        }
            
        #endregion
    }
}
