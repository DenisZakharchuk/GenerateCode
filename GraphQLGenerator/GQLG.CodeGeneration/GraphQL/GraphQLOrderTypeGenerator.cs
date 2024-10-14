using System;
using GQLG.Models.Meta;
using GQLG.CodeGeneration.Base;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using System.Collections.Generic;

namespace GQLG.CodeGeneration.GraphQL
{
    public class GraphQLOrderTypeGenerator : SingleClassGenerator
    {
        public GraphQLOrderTypeGenerator(ClassInfo classInfo, Func<ClassInfo, string> @namespace) : base(classInfo, @namespace)
        {
        }

        protected override TypeSyntax GetBaseClass(ClassInfo classInfo)
        {
            return SyntaxFactory.ParseTypeName("GraphQLOrder");
        }

        public override string GetClassName(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentException($"\"{nameof(type)}\" не может быть пустым или содержать только пробел.", nameof(type));
            }

            return type + "GraphQLOrder";
        }
        protected override ConstructorDeclarationSyntax CreateConstructor(PropertyInfo[] properties, string graphQLTypeName)
        {
            var statements = new List<ExpressionStatementSyntax>(1)
            {
                SyntaxFactory.ExpressionStatement(SyntaxFactory.AssignmentExpression(
                    SyntaxKind.SimpleAssignmentExpression,
                    SyntaxFactory.IdentifierName("Name"),
                    SyntaxFactory.LiteralExpression(
                        SyntaxKind.StringLiteralExpression,
                        SyntaxFactory.Literal(graphQLTypeName)))
                    )
            };

            return SyntaxFactory.ConstructorDeclaration(graphQLTypeName)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .WithBody(SyntaxFactory.Block(statements));
        }
        public override string CodeKind()
        {
            return "GraphQLOrder";
        }
    }
}
