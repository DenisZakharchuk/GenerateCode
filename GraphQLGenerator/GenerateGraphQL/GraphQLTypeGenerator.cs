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
            var graphQLTypeName = GetGraphQLTypeName(typeName);

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
            var statements = properties
                .Where(property => !property.IsCollection)
                .Select(property =>
                    SyntaxFactory.ExpressionStatement(
                        SyntaxFactory.InvocationExpression(
                            SyntaxFactory.MemberAccessExpression(
                                SyntaxKind.SimpleMemberAccessExpression,
                                SyntaxFactory.ThisExpression(),
                                SyntaxFactory.IdentifierName("Field")))
                        .WithArgumentList(
                            SyntaxFactory.ArgumentList(
                                SyntaxFactory.SeparatedList(
                                    GetArgumentsForFieldMethod(property))))))
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

        private static ArgumentSyntax[] GetArgumentsForFieldMethod(PropertyInfo property)
        {
            return new[] {
                SyntaxFactory.Argument(
                    SyntaxFactory.LiteralExpression(
                        SyntaxKind.StringLiteralExpression,
                        SyntaxFactory.Literal(property.Name))),
                SyntaxFactory.Argument(SyntaxFactory.SimpleLambdaExpression(
                    SyntaxFactory.Parameter(SyntaxFactory.Identifier("x")),
                    SyntaxFactory.MemberAccessExpression(
                        SyntaxKind.SimpleMemberAccessExpression,
                        SyntaxFactory.IdentifierName("x"),
                        SyntaxFactory.IdentifierName(property.Name)
                    ))),
                SyntaxFactory.Argument(
                    SyntaxFactory.LiteralExpression(
                        property.IsNullable 
                        ? SyntaxKind.TrueLiteralExpression
                        : SyntaxKind.FalseLiteralExpression)),
                SyntaxFactory.Argument(
                    SyntaxFactory.TypeOfExpression(
                            SyntaxFactory.ParseTypeName(GetGraphQLTypeName(property))
                        ))

            };
        }

        private static string GetGraphQLTypeName(PropertyInfo property)
        {
            if (property is null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            if (property.IsPrimitive)
            {
                return property.Type;
            }

            if (property.IsCollection && property.GenericArguments != null)
            {
                return GetGraphQLTypeName(property.GenericArguments.FirstOrDefault());
            }

            if (property.GenericArguments != null && property.GenericArguments.Count > 0)
            {
                return GetGraphQLTypeName(property.Type + "_" + property.GenericArguments.FirstOrDefault());
            }

            return GetGraphQLTypeName(property.Type);
        }

        private static string GetGraphQLTypeName(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentException($"\"{nameof(type)}\" не может быть пустым или содержать только пробел.", nameof(type));
            }

            return type + "GraphQLType";
        }
    }
}
