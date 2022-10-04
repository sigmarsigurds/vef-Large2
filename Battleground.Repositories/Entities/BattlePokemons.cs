using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Battleground.Repositories.Entities;

public class BattlePokemons
{
    public string PokemonIdentifier { get; set; }
    public int BattleId { get; set; }

    // Navigation properties
    public Battles Battle { get; set; }
    public ICollection<Attacks> Attacks { get; set; }
}
