using System;
using UnityEngine;

namespace E7.RichUp
{
    [Serializable]
    internal struct ManualBreakingConfig
    {
        [Tooltip("Manual breaking only activates if there is at least 1 of this character.")]
        [SerializeField] internal char symbol;
        
        internal const string replace = "<zwsp>";
        internal const string surroundOpen = "<nobr>";
        internal const string surroundClose = "</nobr>";
    }
}