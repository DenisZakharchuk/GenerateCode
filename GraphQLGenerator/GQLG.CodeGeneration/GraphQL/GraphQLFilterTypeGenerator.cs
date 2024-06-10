using System;
using Microsoft.CodeAnalysis;
using GQLG.Models.Meta;
using GQLG.CodeGeneration.Base;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using System.Linq;
using System.Collections.Generic;

namespace GQLG.CodeGeneration.GraphQL
{
    public class GraphQLFilterTypeGenerator : SingleClassGenerator
    {
        public GraphQLFilterTypeGenerator(Func<ClassInfo, string> @namespace) : base(@namespace)
        {
        }

        protected override TypeSyntax GetBaseClass(ClassInfo classInfo)
        {
            return SyntaxFactory.ParseTypeName("InputObjectGraphType");
        }

        protected override string GetClassName(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentException($"\"{nameof(type)}\" не может быть пустым или содержать только пробел.", nameof(type));
            }

            return type + "GraphQLFilter";
        }

        protected override ConstructorDeclarationSyntax CreateConstructor(PropertyInfo[] properties, string graphQLTypeName)
        {
            var statements = new List<ExpressionStatementSyntax>(properties.Length + 1)
            {
                SyntaxFactory.ExpressionStatement(SyntaxFactory.AssignmentExpression(
                    SyntaxKind.SimpleAssignmentExpression, 
                    SyntaxFactory.IdentifierName("Name"),
                    SyntaxFactory.LiteralExpression(
                        SyntaxKind.StringLiteralExpression,
                        SyntaxFactory.Literal(graphQLTypeName)))
                    )
            };

            var invocationStatements = properties
                .Where(property => !property.IsCollection && property.IsPrimitive)
                .Select(property =>
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("Field")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SeparatedList(new[] { 
                                
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.LiteralExpression(
                                            SyntaxKind.StringLiteralExpression,
                                            SyntaxFactory.Literal(property.Name)))

                                })))))
                .ToList();

            statements.AddRange(invocationStatements);

            var triviaList = SyntaxFactory.TriviaList(
                SyntaxFactory.Space,
                SyntaxFactory.EndOfLine("\n"),
                SyntaxFactory.Comment("// Add fields for properties"));

            return SyntaxFactory.ConstructorDeclaration(graphQLTypeName)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .WithBody(SyntaxFactory.Block(statements))
                .WithLeadingTrivia(triviaList); // Add trivia to the constructor
        }
        public override string CodeKind()
        {
            return "GraphQLFilter";
        }
    }
}
