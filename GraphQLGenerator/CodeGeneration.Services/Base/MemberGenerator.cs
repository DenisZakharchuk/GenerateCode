using CodeGeneration.Models.CodingUnits.Meta.Members;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGeneration.Services.Base
{
    public abstract class MemberGenerator<TMember>: Generator<TMember, MemberDeclarationSyntax>, IMemberGenerator<TMember>
        where TMember: BaseMember
    {
        protected MemberGenerator() { }

        public MemberDeclarationSyntax GenerateMember()
        {
            var property = CreateMemberDeclaration(CodingUnit);

            return property;
        }

        protected abstract MemberDeclarationSyntax CreateMemberDeclaration(TMember memberInfo);

    }
}
