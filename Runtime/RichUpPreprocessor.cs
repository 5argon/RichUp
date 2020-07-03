using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace E7.RichUp
{
    public class RichUpPreprocessor : MonoBehaviour, ITextPreprocessor
    {
        [Tooltip(
            "When on, you need to call Apply public method manually with target variable in the code. TMP_Text on the same game object will not be the target. Custom target also has no effect.")]
        [SerializeField]
        bool manualApply;

        [Tooltip(
            "Instead of searching for TMP_Text in the same GameObject, use any TMP_Text assigned on target field.")]
        [SerializeField]
        bool customTarget;

        [Tooltip("Reference to the target if referenceTarget is checked.")] [SerializeField]
        TMP_Text customTargetReference;

        [Space] [Tooltip("How original Markdown-like text would turns into rich text.")] [SerializeField]
        Config config;

        IItemFormatter itemFormatter;

        void Start()
        {
            if (!manualApply)
            {
                TMP_Text findTarget = null;
                if (customTarget)
                {
                    findTarget = customTargetReference;
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

            itemFormatter = GetComponent<IItemFormatter>();
        }

        public void Apply(TMP_Text textTarget)
        {
            textTarget.textPreprocessor = this;
        }

        string ITextPreprocessor.PreprocessText(string beforeText)
        {
            var processed = TextProcessingLogic.Process(beforeText, config);
            var formatted = TextProcessingLogic.FormatItems(processed, itemFormatter);
            return formatted;
        }
    }
}