using System;
using UnityEngine;

namespace E7.RichUp
{
    [Serializable]
    internal struct PositionalVariableConfig
    {
        [SerializeField] internal string variableName;
        [SerializeField] internal uint mapsTo;
    }
}