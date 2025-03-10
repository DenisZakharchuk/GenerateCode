﻿using CodeGeneration.Models.CodingUnits.Meta;

namespace CodeGeneration.Services.Base
{
    public interface ICodingUnitService<TCodingUnit>
        where TCodingUnit : CodingUnit
    {
        internal void Init(TCodingUnit codingUnit);
    }
}
