using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Battleground.Api.Schema.Types;
using Battleground.Services.Interfaces;
using GraphQL;
using GraphQL.Types;

namespace Battleground.Api.Schema.Queries;

public class BattlegroundQuery : ObjectGraphType
{
    private IPokemonService _pokemonService;
    private IPlayerService _playerService;
    private IBattleService _battleService;

    public BattlegroundQuery(IPokemonService pokemonService, IPlayerService playerService, IBattleService battleService)
    {
        _pokemonService = pokemonService;
        _playerService = playerService;
        _battleService = battleService;

        Field<ListGraphType<PokemonType>>("allPokemons")
            .ResolveAsync(async context =>
            {
                var pokemons = await pokemonService.getAllPokemons();

                return pokemons;
            });

        Field<PokemonType>("pokemon")
            .Argument<StringGraphType>("name")
            .ResolveAsync(async context =>
            {
                var name = context.GetArgument<string>("name");
                var pokemon = await pokemonService.getPokemonByName(name);

                return pokemon;
            });

        Field<ListGraphType<PlayerType>>("allPlayers")
            .Resolve(context =>
            {
                var players = playerService.getAllPlayers();

                return players;
            });

        Field<PlayerType>("player")
            .Argument<IntGraphType>("Id")
            .Resolve(context =>
            {

                var idArgument = context.GetArgument<int>("Id");

                //System.Console.Write("\n", "idArgument", "\n");
                //System.Console.Write("\n", idArgument, "\n");

                var player = playerService.getPlayerById(idArgument);

                return player;
            });


        Field<ListGraphType<BattleType>>("allBattles")
            .ResolveAsync(async context =>
            {
                var battles = await battleService.GetAllBattles();
                return battles;
            });

        Field<BattleType>("battle")
            .Argument<IntGraphType>("Id")
            .ResolveAsync(async context =>
            {
                var battleIdArg = context.GetArgument<int>("Id");
                var battle = await battleService.GetBattleById(battleIdArg);
                return battle;
            });
    }
}
