﻿using CodeGeneration.Models.CodingUnits.Meta;

namespace CodeGeneration.Services.Naming
{
    public abstract class NamingProvider : INamingProvider
    {
        protected NamingProvider() : this("Base", "Generated")
        {
        }

        protected NamingProvider(string baseNamespace, string defaultNamespace)
        {
            BaseNamespace = baseNamespace;
            DefaultNamespace = defaultNamespace;
        }

        protected string DefaultNamespace { get; private set; }

        protected string BaseNamespace { get; private set; }

        public virtual string GetName(CodingUnit unit)
        {
            return unit.Name;
        }
        public virtual string GetNamespace(CodingUnit unit)
        {
            return new string[] { BaseNamespace, unit.Namespace ?? DefaultNamespace, GetDescriptor() }.
                Where(s => !string.IsNullOrWhiteSpace(s)).
                Aggregate((h, t) => h + "." + t);
        }

        protected abstract string GetDescriptor();
    }
}
