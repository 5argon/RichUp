using System;
using UnityEngine;

namespace E7.RichUp
{
    [Serializable]
    internal struct SurroundConfig
    {
        [SerializeField] internal bool styleSurround;
        [SerializeField] internal string styleName;
        [Space]
        [SerializeField] internal bool customSurround;
        [SerializeField] internal string customOpening;
        [SerializeField] internal string customClosing;

        internal bool Activated => styleSurround || customSurround;
    }
}