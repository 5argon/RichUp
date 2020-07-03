using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace E7.RichUp
{
    public class RichUpPreprocessor : MonoBehaviour, ITextPreprocessor
    {
        [Tooltip(
            "Instead of searching for TMP_Text in the same GameObject, use any TMP_Text assigned on target field.")]
        [SerializeField]
        bool referenceTarget;

        [Tooltip("Reference to the target if referenceTarget is checked.")] [SerializeField]
        TMP_Text target;

        [Tooltip("You need to call Apply public method manually with target variable in the code.")] [SerializeField]
        bool manualApply;

        [Space] [Tooltip("How original Markdown-like text would turns into rich text.")] [SerializeField]
        RichUpConfig config;

        void Start()
        {
            if (!manualApply)
            {
                TMP_Text findTarget = null;
                if (referenceTarget)
                {
                    findTarget = target;
                }
                else
                {
                    findTarget = GetComponent<TMP_Text>();
                }

                if (findTarget != null)
                {
                    Apply(findTarget);
                }
            }
        }

        public void Apply(TMP_Text textTarget)
        {
            textTarget.textPreprocessor = this;
        }

        string ITextPreprocessor.PreprocessText(string beforeText)
        {
            return RichUpTextProcessing.Process(beforeText, config);
        }
    }
}