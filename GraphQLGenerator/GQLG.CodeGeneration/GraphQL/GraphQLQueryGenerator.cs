using System;
using System.Linq;
using GQLG.CodeGeneration.Base;
using GQLG.Models.Meta;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GQLG
{
    public class GraphQLQueryGenerator : SingleClassGenerator
    {
        public override string CodeKind()
        {
            return "GraphQLQuery";
        }

        public override string SubDir()
        {
            return "Query";
        }
        public GraphQLQueryGenerator(Func<ClassInfo, string> @namespace) : base(@namespace)
        {
            if (@namespace is null)
            {
                throw new ArgumentNullException(nameof(@namespace));
            }
        }

        protected override TypeSyntax GetBaseClass(ClassInfo classInfo)
        {
            return SyntaxFactory.ParseTypeName("MijDimGraphQLQuery");
        }

        public override string GetClassName(string type)
        {
            return $"IntegrationGraphQLQuery";
        }
        public static ExpressionStatementSyntax GenerateGenericMethodInvocation(string methodName, string genericTypeName, string methodArgument)
        {
            return SyntaxFactory.ExpressionStatement(
                SyntaxFactory.InvocationExpression(
                    SyntaxFactory.GenericName(methodName)
                        .WithTypeArgumentList(
                            SyntaxFactory.TypeArgumentList(
                                SyntaxFactory.SingletonSeparatedList<TypeSyntax>(
                                    SyntaxFactory.IdentifierName(genericTypeName)))))
                .WithArgumentList(
                    SyntaxFactory.ArgumentList(
                        SyntaxFactory.SingletonSeparatedList(
                            SyntaxFactory.Argument(
                                SyntaxFactory.LiteralExpression(
                                    SyntaxKind.StringLiteralExpression,
                                    SyntaxFactory.Literal(methodArgument)))))));
        }

        protected override ConstructorDeclarationSyntax CreateConstructor(PropertyInfo[] properties, string graphQLTypeName)
        {
            var contstructor = base.CreateConstructor(properties, graphQLTypeName);

            var constructorStatements = properties.Select(p => GenerateGenericMethodInvocation("Field", p.Type, p.Name)).ToArray();

            contstructor = contstructor.WithBody(SyntaxFactory.Block(constructorStatements));

            return contstructor;
        }
    }
}
