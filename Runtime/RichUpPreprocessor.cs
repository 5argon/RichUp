using TMPro;
using UnityEngine;

namespace E7.RichUp
{
    public class RichUpPreprocessor : MonoBehaviour, ITextPreprocessor
    {
#pragma warning disable 0649
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
#pragma warning restore 0649

        private IItemFormatter itemFormatter;
        private TMP_Text textTarget;

        void OnEnable()
        {
            if (!manualApply)
            {
                if (customTarget)
                {
                    textTarget = customTargetReference;
                }
                else
                {
                    textTarget = GetComponent<TMP_Text>();
                }

                Apply(textTarget);
            }

            itemFormatter = GetComponent<IItemFormatter>();
        }

        void OnDisable()
        {
            if (textTarget != null && ReferenceEquals(textTarget.textPreprocessor, this))
            {
                textTarget.textPreprocessor = null;
            }
        }

        public void Apply(TMP_Text target)
        {
            if (target != null)
            {
                target.textPreprocessor = this;
                target.ForceMeshUpdate(true, true);
            }
        }

        public string PreprocessText(string beforeText)
        {
            if (string.IsNullOrEmpty(beforeText))
            {
                return beforeText;
            }
            var processed = TextProcessingLogic.Process(beforeText, config);
            var formatted = TextProcessingLogic.FormatItems(processed, itemFormatter);
            return formatted;
        }
    }
}