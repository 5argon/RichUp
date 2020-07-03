using System;
using UnityEngine;

namespace E7.RichUp
{
    [Serializable]
    internal struct Config
    {
        [Tooltip("*5argon*")]
        [SerializeField] internal SurroundConfig italicSurround;
        [Tooltip("**5argon**")]
        [SerializeField] internal SurroundConfig boldSurround;
        [Tooltip("***5argon***")]
        [SerializeField] internal SurroundConfig boldItalicSurround;
        [Tooltip("`5argon`")]
        [SerializeField] internal SurroundConfig inlineCodeSurround;
        
        [Space]
        
        [Tooltip("# 5argon")]
        [SerializeField] internal SurroundConfig heading1Surround;
        [Tooltip("## 5argon")]
        [SerializeField] internal SurroundConfig heading2Surround;
        [Tooltip("### 5argon")]
        [SerializeField] internal SurroundConfig heading3Surround;
        [Tooltip("#### 5argon")]
        [SerializeField] internal SurroundConfig heading4Surround;
        [Tooltip("##### 5argon")]
        [SerializeField] internal SurroundConfig heading5Surround;
        [Tooltip("###### 5argon")]
        [SerializeField] internal SurroundConfig heading6Surround;
        
        [Space]
        
        [Tooltip("> 5argon")]
        [SerializeField] internal SurroundConfig blockquoteSurround;
        [Tooltip("- 5argon")]
        [SerializeField] internal SurroundConfig listSurround;
        
        [Space]
        
        [SerializeField] internal SymbolConfig[] symbolConfigs;
    }
}