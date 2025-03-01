using CodeGeneration.Services.Base;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CodeGeneration.Services.Generators
{
    public class MethodGenerator : MemberGenerator<Models.CodingUnits.Meta.Members.MethodInfo>, IMethodGenerator
    {
        protected override MethodDeclarationSyntax CreateMemberDeclaration(Models.CodingUnits.Meta.Members.MethodInfo memberInfo)
        {
            var returnType = memberInfo.Type.Name;
            var methodName = memberInfo.Name;

            // Parse the return type
            var returnTypeSyntax = SyntaxFactory.ParseTypeName(returnType);

            // Create the method signature (name and return type)
            var methodDeclaration = SyntaxFactory.MethodDeclaration(returnTypeSyntax, methodName)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword));

            var methodBody = GenerateBody(memberInfo);

            // Attach the body to the method declaration
            methodDeclaration = methodDeclaration.WithBody(methodBody);

            return methodDeclaration;
        }

        protected virtual BlockSyntax GenerateBody(Models.CodingUnits.Meta.Members.MethodInfo methodInfo)
        {
            // Create the method body (throwing a new exception)
            return SyntaxFactory.Block(
                SyntaxFactory.ThrowStatement(SyntaxFactory.ObjectCreationExpression(
                    SyntaxFactory.ParseTypeName("System.Exception"))
                    .AddArgumentListArguments(SyntaxFactory.Argument(SyntaxFactory.ParseExpression("\"An error occurred.\"")))
                )
            );
        }
    }
}
