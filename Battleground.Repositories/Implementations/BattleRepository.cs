using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Battleground.Repositories.Interfaces;
using Battleground.Repositories.Contexts;
using Battleground.Models.Dtos;
using Battleground.Repositories.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Battleground.Models.InputModels;

namespace Battleground.Repositories.Implementations
{
    public class BattleRepository : IBattleRepository
    {
        private BattlegroundDbContext _dbContext;
        private readonly IMapper _mapper;
        private IBattleRepository _battleRepositoryImplementation;

        public BattleRepository(BattlegroundDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<IEnumerable<BattleDto>> GetAllBattles()
        {
            var battles = await _dbContext
                                .Battles
                                .Include(b => b.Status)
                                .Include(b => b.Winner)
                                .Include(b => b.PlayersInMatch)
                                .Include(b => b.BattlePokemons)
                                    .ThenInclude(b => b.Attacks)
                                .Select(b => new BattleDto
                                {
                                    Id = b.Id,
                                    Status =
                                        b.Status.Name == Battleground.Models.Enums.BattleStatus.NOT_STARTED.ToString()
                                            ? Battleground.Models.Enums.BattleStatus.NOT_STARTED
                                            : b.Status.Name == Battleground.Models.Enums.BattleStatus.STARTED.ToString()
                                            ? Battleground.Models.Enums.BattleStatus.STARTED
                                            : b.Status.Name == Battleground.Models.Enums.BattleStatus.FINISHED.ToString()
                                            ? Battleground.Models.Enums.BattleStatus.FINISHED
                                            : Battleground.Models.Enums.BattleStatus.FINISHED,

                                    Winner = b.Winner.Id != null ? new PlayerDto
                                    {
                                        Id = b.Winner.Id,
                                        Name = b.Winner.Name
                                    } : null,
                                    PlayersInMatch = b.PlayersInMatch.Select(p => new PlayerDto
                                    {
                                        Id = p.PlayerInMatch.Id,
                                        Name = p.PlayerInMatch.Name
                                    }),
                                    BattlePokemons = b.BattlePokemons.Select(bp => new BattlePokemonsDto
                                    {
                                        PokemonIdentifier = bp.PokemonIdentifier,
                                        BattleId = bp.BattleId
                                    }),
                                    Attacks = b.BattlePokemons.Select(bp => bp.Attacks.Select(a => new AttackDto
                                    {
                                        DamageDealt = a.Damage,
                                        SuccessfulHit = a.Success,
                                        CriticalHit = a.CriticalHit,
                                        BattlePokemons = new BattlePokemonsDto
                                        {
                                            PokemonIdentifier = a.Battle.PokemonIdentifier,
                                            BattleId = a.Battle.BattleId
                                        }

                                    })).SelectMany(r => r)
                                }).ToListAsync();
            return battles;
        }

        public async Task<BattleDto> GetBattleById(int Id)
        {
            var battle = await _dbContext
                                .Battles
                                .Include(b => b.Status)
                                .Include(b => b.Winner)
                                .Include(b => b.PlayersInMatch)
                                .Include(b => b.BattlePokemons)
                                    .ThenInclude(b => b.Attacks)
                                .Select(b => new BattleDto
                                {
                                    Id = b.Id,
                                    Status =
                                        b.Status.Name == Battleground.Models.Enums.BattleStatus.NOT_STARTED.ToString()
                                            ? Battleground.Models.Enums.BattleStatus.NOT_STARTED
                                            : b.Status.Name == Battleground.Models.Enums.BattleStatus.STARTED.ToString()
                                            ? Battleground.Models.Enums.BattleStatus.STARTED
                                            : b.Status.Name == Battleground.Models.Enums.BattleStatus.FINISHED.ToString()
                                            ? Battleground.Models.Enums.BattleStatus.FINISHED
                                            : Battleground.Models.Enums.BattleStatus.FINISHED,

                                    Winner = b.Winner.Id != null ? new PlayerDto
                                    {
                                        Id = b.Winner.Id,
                                        Name = b.Winner.Name
                                    } : null,
                                    PlayersInMatch = b.PlayersInMatch.Select(p => new PlayerDto
                                    {
                                        Id = p.PlayerInMatch.Id,
                                        Name = p.PlayerInMatch.Name
                                    }),
                                    BattlePokemons = b.BattlePokemons.Select(bp => new BattlePokemonsDto
                                    {
                                        PokemonIdentifier = bp.PokemonIdentifier,
                                        BattleId = bp.BattleId
                                    }),
                                    Attacks = b.BattlePokemons.Select(bp => bp.Attacks.Select(a => new AttackDto
                                    {
                                        DamageDealt = a.Damage,
                                        SuccessfulHit = a.Success,
                                        CriticalHit = a.CriticalHit,
                                        BattlePokemons = new BattlePokemonsDto
                                        {
                                            PokemonIdentifier = a.Battle.PokemonIdentifier,
                                            BattleId = a.Battle.BattleId
                                        }

                                    })).SelectMany(r => r)
                                }).FirstOrDefaultAsync(x => x.Id == Id);
            return battle;
        }

        public async Task<BattleDto> CreateBattle(BattleInputModel battle)
        {
            var battles = _dbContext.Battles;

            var battleStatusNotStarted = await _dbContext.BattleStatus.FirstOrDefaultAsync(b => b.Name == "NOT_STARTED");

            var newBattle = new Battles
            {
                Status = battleStatusNotStarted,
            };

            battles.Add(newBattle);
            await _dbContext.SaveChangesAsync();

            var newBattleId = newBattle.Id;


            System.Console.WriteLine("newBattleId");
            System.Console.WriteLine(newBattleId);


            throw new NotImplementedException();
        }
    }
}