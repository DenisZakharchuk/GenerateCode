using CodeGeneration.Models.CodingUnits.Meta.Members;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGeneration.Services.Base
{
    public abstract class MemberGenerator<TMember>: IMemberGenerator<TMember>
        where TMember: BaseMember
    {
        private TMember? _memberInfo;

        protected virtual TMember MemberInfo => _memberInfo
            ?? throw new ApplicationException($"{nameof(MemberInfo)} is not initialized. Call ${Init} method first");

        protected MemberGenerator() { }

        public MemberDeclarationSyntax GenerateMember()
        {
            var property = CreateMemberDeclaration(MemberInfo);

            return property;
        }

        public void Init(TMember codingUnit)
        {
            _memberInfo = codingUnit;
        }

        protected abstract MemberDeclarationSyntax CreateMemberDeclaration(TMember memberInfo);        
    }
}
