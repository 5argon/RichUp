using System;
using UnityEngine;

namespace E7.RichUp
{
    [Serializable]
    internal struct Config
    {
        [SerializeField] internal SurroundConfig italicSurround;
        [SerializeField] internal SurroundConfig boldSurround;
        [SerializeField] internal SurroundConfig boldItalicSurround;
        [SerializeField] internal SurroundConfig inlineCodeSurround;
        
        [Space]
        
        [SerializeField] internal SurroundConfig heading1Surround;
        [SerializeField] internal SurroundConfig heading2Surround;
        [SerializeField] internal SurroundConfig heading3Surround;
        [SerializeField] internal SurroundConfig heading4Surround;
        [SerializeField] internal SurroundConfig heading5Surround;
        [SerializeField] internal SurroundConfig heading6Surround;
        
        [Space]
        
        [SerializeField] internal SurroundConfig blockquoteSurround;
        [SerializeField] internal SurroundConfig listSurround;
        
        [Space]
        
        [SerializeField] internal SymbolConfig[] symbolConfigs;
    }
}