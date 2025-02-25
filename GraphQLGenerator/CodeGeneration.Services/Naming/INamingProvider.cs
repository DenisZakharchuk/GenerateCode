﻿using CodeGeneration.Models.CodingUnits.Meta;
using CodeGeneration.Services.Base;

namespace CodeGeneration.Services.Naming
{
    public interface IInterfaceNamingProvider : INamingProvider, ICodingUnitService<Behaviour>;
    public interface INamingProvider
    {
        string GetName(CodingUnit unit);
        string GetNamespace(CodingUnit unit);
    }
}
