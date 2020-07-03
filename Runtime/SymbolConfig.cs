using System;
using UnityEngine;

namespace E7.RichUp
{
    [Serializable]
    internal struct SymbolConfig
    {
        [SerializeField] internal char symbol;
        [Space]
        [SerializeField] internal bool replace;
        [SerializeField] internal string replaceWith;
        
        [Space] 
        
        [SerializeField] internal SurroundConfig remainingSurroundConfig;
    }
}