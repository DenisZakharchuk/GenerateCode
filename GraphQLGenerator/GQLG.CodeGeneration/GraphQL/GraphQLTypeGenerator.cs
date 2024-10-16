﻿using System;
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
        public GraphQLTypeGenerator(ClassInfo classInfo) : base(classInfo, c => "GQLG.Generated")
        {
        }

        public GraphQLTypeGenerator(ClassInfo classInfo, Func<ClassInfo, string> @namespace) : base(classInfo, @namespace)
        {
        }

        public override string GetClassName(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentException($"\"{nameof(type)}\" не может быть пустым или содержать только пробел.", nameof(type));
            }

            return type + "GraphQLType";
        }

        protected override TypeSyntax GetBaseClass(ClassInfo classInfo)
        {
            return SyntaxFactory.GenericName(SyntaxFactory.Identifier("ObjectGraphType"))
                .AddTypeArgumentListArguments(SyntaxFactory.ParseTypeName(classInfo.Name));
        }

        protected override ConstructorDeclarationSyntax CreateConstructor(PropertyInfo[] properties, string graphQLTypeName)
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
