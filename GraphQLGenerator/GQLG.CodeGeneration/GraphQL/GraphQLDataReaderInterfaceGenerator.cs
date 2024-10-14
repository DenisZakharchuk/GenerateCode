using System;
using System.Collections.Generic;
using GQLG.CodeGeneration.Base;
using GQLG.Models.Meta;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace GQLG
{
    public class GraphQLDataReaderInterfaceGenerator : SingleInterfaceGenerator
    {
        public GraphQLDataReaderInterfaceGenerator(ClassInfo classInfo, Func<ClassInfo, string> @namespace) : base(classInfo, @namespace)
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
            return $"IDataReader";
        }

        public override string GetClassFileName()
        {
            return $"I{ClassInfo.Name}DataReader.cs";
        }

        public override IEnumerable<string> RequiredNamespaces()
        {
            foreach (var item in base.RequiredNamespaces())
            {
                yield return item;
            }

            yield return "MijDim.Web.GraphQL.DataReaders";
        }
    }
}
