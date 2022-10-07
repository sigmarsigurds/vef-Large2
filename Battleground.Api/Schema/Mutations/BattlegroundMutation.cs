using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Battleground.Api.Schema.Types;
using Battleground.Services.Interfaces;
using Battleground.Api.Schema.InputTypes;

using GraphQL;
using GraphQL.Types;
using AutoMapper;
using Battleground.Models.InputModels;
using Battleground.Repositories.Contexts;
using Battleground.Repositories.Entities;
using Battleground.Models.Dtos;

namespace Battleground.Api.Schema.Mutations
{
    public class BattlegroundMutation : ObjectGraphType
    {
        private IPokemonService _pokemonService;
        private IPlayerService _playerService;
        private IBattleService _battleService;
        private IAttackService _attackService;
        private IInventoryService _inventoryService;

        public BattlegroundMutation(IPokemonService pokemonService, IPlayerService playerService, IBattleService battleService, IInventoryService inventoryService, IAttackService attackService)
        {
            _pokemonService = pokemonService;
            _playerService = playerService;
            _attackService = attackService;
            _battleService = battleService;
            _inventoryService = inventoryService;


            Field<PlayerType>("addPlayer")
                .Argument<PlayerInputType>("player")
                .Resolve(context =>
            {
                var player = context.GetArgument<PlayerInputModel>("player");
                return _playerService.CreatePlayer(player);

            });

            Field<PlayerType>("removePlayer")
                .Argument<IntGraphType>("id")
                .Resolve(context =>
                {
                    var idArgument = context.GetArgument<int>("id");
                    return _playerService.RemovePlayer(idArgument);
                });

            Field<BattleType>("addBattle")
                .Argument<BattleInputType>("battle")
                .ResolveAsync(async context =>
                {
                    var battle = context.GetArgument<BattleInputModel>("battle");
                    // TODO: Validate input, does pokemon exist with the given id?
                    var newBattle = await _battleService.CreateBattle(battle);

                    return null;
                });

            Field<AttackType>("attack")
                .Argument<AttackInputType>("attack")
                .ResolveAsync(async context =>
                {

                    var attack = context.GetArgument<AttackInputModel>("attack");
                    var ret_val = await _attackService.CreateAttack(attack);
                    return ret_val;
                });

            Field<BooleanGraphType>("addPokemonToInventory")
                .Argument<InventoryInputType>("input")
                .Resolve(context =>
                {
                    var inputInventory = context.GetArgument<InventoryInputModel>("input");

                    var newPokemonInventory = _inventoryService.AddPokemonToInventory(inputInventory);
                    return true;
                });

            Field<BooleanGraphType>("removePokemonFromInventory")
                .Argument<InventoryInputType>("input")
                .Resolve(context =>
                {
                    var inputInventory = context.GetArgument<InventoryInputModel>("input");

                    var bruh = _inventoryService.RemovePokemonToInventory(inputInventory);
                    return true;
                });

        }
    }
}