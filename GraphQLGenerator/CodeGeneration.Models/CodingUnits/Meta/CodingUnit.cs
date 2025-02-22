﻿namespace CodeGeneration.Models.CodingUnits.Meta
{
    public class Interface : CodingUnit
    {

    }
    public class Model : CodingUnit
    {

    }
    public class CodingUnit
    {
        public PropertyInfo[]? Properties { get; set; }
        public required string Name { get; set; }
        public string? Namespace { get; set; }
    }
}
