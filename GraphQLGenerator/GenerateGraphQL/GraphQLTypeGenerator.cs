using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;

namespace GQLG
{
    public static class GraphQLTypeGenerator
    {
        public static SyntaxTree Generate(PropertyInfo[] properties, string typeName)
        {
            var graphQLTypeName = typeName + "GraphQLType";

            var classDeclaration = SyntaxFactory.ClassDeclaration(graphQLTypeName)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .AddBaseListTypes(
                    SyntaxFactory.SimpleBaseType(
                        SyntaxFactory.GenericName(
                            SyntaxFactory.Identifier("ObjectGraphType"))
                        .AddTypeArgumentListArguments(SyntaxFactory.ParseTypeName(typeName))))
                .AddMembers(CreateConstructor(properties, graphQLTypeName));

            var namespaceDeclaration = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName("GQLG.Generated"))
                .AddMembers(classDeclaration);

            var compilationUnit = SyntaxFactory.CompilationUnit()
                .AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System")))
                .AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("GraphQL.Types")))
                .AddMembers(namespaceDeclaration)
                .NormalizeWhitespace();

            return SyntaxFactory.SyntaxTree(compilationUnit, encoding: System.Text.Encoding.UTF8);
        }

        private static ConstructorDeclarationSyntax CreateConstructor(PropertyInfo[] properties, string graphQLTypeName)
        {
            var statements = properties.Select(property =>
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.InvocationExpression(
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.ThisExpression(),
                            SyntaxFactory.GenericName(
                                SyntaxFactory.IdentifierName("Field").Identifier,
                                SyntaxFactory.TypeArgumentList(
                                    SyntaxFactory.SingletonSeparatedList(
                                        SyntaxFactory.ParseTypeName(GetGraphQLTypeName(property)))))))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SeparatedList(
                                new[] {
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.LiteralExpression(
                                            SyntaxKind.StringLiteralExpression,
                                            SyntaxFactory.Literal(property.Name))),
                                    SyntaxFactory.Argument(
                                        SyntaxFactory.LiteralExpression(
                                            SyntaxKind.StringLiteralExpression,
                                            SyntaxFactory.Literal(property.Name)))
                                })))))
            .ToArray();

            var triviaList = SyntaxFactory.TriviaList(
                SyntaxFactory.Space,
                SyntaxFactory.EndOfLine("\n"),
                SyntaxFactory.Comment("// Add fields for properties"));

            return SyntaxFactory.ConstructorDeclaration(graphQLTypeName)
                .AddModifiers(SyntaxFactory.Token(SyntaxKind.PublicKeyword))
                .WithBody(SyntaxFactory.Block(statements))
                .WithLeadingTrivia(triviaList); // Add trivia to the constructor
        }

        private static string GetGraphQLTypeName(PropertyInfo property)
        {
            return property.Type;
        }
    }
}
