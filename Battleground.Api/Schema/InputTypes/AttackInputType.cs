using GraphQL.Types;
using Battleground.Models.InputModels;

namespace Battleground.Api.Schema.InputTypes;

public class AttackInputType : InputObjectGraphType<AttackInputModel>
{
    public AttackInputType()
    {
        Field(x => x.Attacker).Description("The name of the attacker");
        Field(x => x.BattleId).Description("the Id of the battle");
    }
}