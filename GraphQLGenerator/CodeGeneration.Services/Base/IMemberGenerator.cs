using CodeGeneration.Models.CodingUnits.Meta.Members;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGeneration.Services.Base
{

    public interface IMemberGenerator<TMember> : IMemberGenerator, ICodingUnitService<TMember>
        where TMember : BaseMember
    {
    }

    public interface IMemberGenerator
    {
        MemberDeclarationSyntax GenerateMember();
    }
}
