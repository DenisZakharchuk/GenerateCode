using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using GQLG.Models.Meta;
using GQLG.CodeGeneration.Base;

namespace GQLG.CodeGeneration.GraphQL
{
    public class GraphQLTypeGenerator : SingleClassGenerator
    {
        public GraphQLTypeGenerator(): base(classInfo => "GQLG.Generated")
        {
        }

        public GraphQLTypeGenerator(Func<ClassInfo, string> @namespace) : base(@namespace)
        {
        }

        public override SyntaxTree Generate(ClassInfo classInfo)
        {
            var typeName = classInfo.Name;
            var properties = classInfo.Properties;
            var @namespace = Namespace(classInfo);

            var classDeclaration = ClassDeclaration(classInfo);

            var namespaceDeclaration = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(@namespace))
                .AddMembers(classDeclaration);

            var compilationUnit = SyntaxFactory.CompilationUnit()
                .AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System")))
                .AddUsings(SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("GraphQL.Types")))
                .AddMembers(namespaceDeclaration)
                .NormalizeWhitespace();

            return SyntaxFactory.SyntaxTree(compilationUnit, encoding: System.Text.Encoding.UTF8);
        }

        protected override ClassDeclarationSyntax ClassDeclaration(ClassInfo classInfo)
        {
            return base.ClassDeclaration(classInfo)
                .AddMembers(CreateConstructor(classInfo.Properties, GetClassName(classInfo.Name)));;
        }

        protected override string GetClassName(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentException($"\"{nameof(type)}\" не может быть пустым или содержать только пробел.", nameof(type));
            }

            return type + "GraphQLType";
        }

        protected override TypeSyntax GetBaseClass(string baseTypeName)
        {
            return SyntaxFactory.GenericName(SyntaxFactory.Identifier("ObjectGraphType"))
                .AddTypeArgumentListArguments(SyntaxFactory.ParseTypeName(baseTypeName));
        }

        private ConstructorDeclarationSyntax CreateConstructor(PropertyInfo[] properties, string graphQLTypeName)
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

        private ArgumentSyntax[] GetArgumentsForFieldMethod(PropertyInfo property)
        {
            var arguments = new List<ArgumentSyntax>() {
                SyntaxFactory.Argument(
                    SyntaxFactory.LiteralExpression(
                        SyntaxKind.StringLiteralExpression,
                        SyntaxFactory.Literal(property.Name))),

                SyntaxFactory.Argument(
                    SyntaxFactory.SimpleLambdaExpression(
                        SyntaxFactory.Parameter(
                            SyntaxFactory.Identifier("x")),
                        SyntaxFactory.MemberAccessExpression(
                            SyntaxKind.SimpleMemberAccessExpression,
                            SyntaxFactory.IdentifierName("x"),
                            SyntaxFactory.IdentifierName(property.Name)))),

                SyntaxFactory.Argument(
                    SyntaxFactory.LiteralExpression(
                        property.IsNullable
                        ? SyntaxKind.TrueLiteralExpression
                        : SyntaxKind.FalseLiteralExpression)) 
            };

            if (!property.IsPrimitive)
            {
                arguments.Add(SyntaxFactory.Argument(
                    SyntaxFactory.TypeOfExpression(
                        SyntaxFactory.ParseTypeName(
                            GetGraphQLTypeName(property)
                            ))));
            }

            return arguments.ToArray();
        }

        private string GetGraphQLTypeName(PropertyInfo property)
        {
            if (property is null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            if (property.IsPrimitive)
            {
                return GetGraphQLPrimitiveType(property);
            }

            if (property.IsCollection && property.GenericArguments != null)
            {
                return GetClassName(property.GenericArguments.FirstOrDefault());
            }

            if (property.GenericArguments != null && property.GenericArguments.Count > 0)
            {
                return GetClassName(property.Type + "_" + property.GenericArguments.FirstOrDefault());
            }

            return GetClassName(property.Type);
        }

        private static string GetGraphQLPrimitiveType(PropertyInfo property)
        {
            return property.Type;
        }
    }
}
