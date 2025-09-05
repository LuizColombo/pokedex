// Models/Pokemon.cs
using System.Collections.Generic;

namespace pokedex.Models
{
    public class Pokemon
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string Sprite { get; set; }
        public List<string> Types { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
    }
}
