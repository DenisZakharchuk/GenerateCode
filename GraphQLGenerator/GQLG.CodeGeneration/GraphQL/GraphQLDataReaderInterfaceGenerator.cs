using System;
using GQLG.CodeGeneration.Base;
using GQLG.Models.Meta;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GQLG
{
    public class GraphQLDataReaderInterfaceGenerator : SingleInterfaceGenerator
    {
        public GraphQLDataReaderInterfaceGenerator(Func<ClassInfo, string> @namespace) : base(@namespace)
        {
        }

        protected override TypeSyntax GetBaseInterface(ClassInfo classInfo)
        {
            return SyntaxFactory.ParseTypeName("IGraphQLDataReader");
        }

        protected override string GetInterfaceName(string type)
        {
            return $"I{type}DataReader";
        }

        public override string CodeKind()
        {
            return "IDataReader";
        }
    }
}
