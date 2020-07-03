using System;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;

namespace E7.RichUp
{
    internal static class TextProcessingLogic
    {
        internal static string FormatItems(string original, IItemFormatter formatter)
        {
            return new Regex("{([^{}]+?)}", RegexOptions.Multiline).Replace(original, Replacer);

            string Replacer(Match match)
            {
                return formatter?.FormatItem(match.Groups[1].Value) ?? "";
            }
        }

        internal static string Process(string original, Config config)
        {
            string r = original;
            r = RegexWith(config.heading6Surround, r, "^###### (.*)", RegexOptions.Multiline);
            r = RegexWith(config.heading5Surround, r, "^##### (.*)", RegexOptions.Multiline);
            r = RegexWith(config.heading4Surround, r, "^#### (.*)", RegexOptions.Multiline);
            r = RegexWith(config.heading3Surround, r, "^### (.*)", RegexOptions.Multiline);
            r = RegexWith(config.heading2Surround, r, "^## (.*)", RegexOptions.Multiline);
            r = RegexWith(config.heading1Surround, r, "^# (.*)", RegexOptions.Multiline);

            r = RegexWith(config.blockquoteSurround, r, "^> (.*)", RegexOptions.Multiline);
            r = RegexWith(config.listSurround, r, "^- (.*)", RegexOptions.Multiline);

            r = RegexWith(config.boldItalicSurround, r, @"\*\*\*([^*]+?)\*\*\*", RegexOptions.None);
            r = RegexWith(config.boldSurround, r, @"\*\*([^*]+?)\*\*", RegexOptions.None);
            r = RegexWith(config.italicSurround, r, @"\*([^*]+?)\*", RegexOptions.None);
            r = RegexWith(config.inlineCodeSurround, r, "`(.+?)`", RegexOptions.None);

            if (config.symbolConfigs != null && config.symbolConfigs.Length > 0)
            {
                var lineSplitted = r.Split('\n');
                var sbArray = new StringBuilder[lineSplitted.Length];
                for (int i = 0; i < sbArray.Length; i++)
                {
                    sbArray[i] = new StringBuilder(lineSplitted[i]);
                    SymbolProcessing(sbArray[i], config);
                }

                r = String.Join("\n", sbArray.Select(x => x.ToString()).ToArray());
            }

            return r;
        }

        static StringBuilder openingClosingBuilder = new StringBuilder();

        static string RegexWith(SurroundConfig config, string input, string pattern, RegexOptions options)
        {
            if (config.Activated)
            {
                openingClosingBuilder.Clear();
                UseSb(openingClosingBuilder, config, "$1");
                return new Regex(pattern, options).Replace(input, openingClosingBuilder.ToString());
            }
            else
            {
                return input;
            }
        }

        const string styleClosing = "</style>";


        private static void UseSb(StringBuilder sb, SurroundConfig conf, string middle)
        {
            if (conf.styleSurround)
            {
                string styleOpening = $"<style=\"{conf.styleName}\">";
                sb.Append(styleOpening);
            }

            if (conf.customSurround)
            {
                sb.Append(conf.customOpening);
            }

            sb.Append(middle);
            if (conf.customSurround)
            {
                sb.Append(conf.customClosing);
            }

            if (conf.styleSurround)
            {
                sb.Append(styleClosing);
            }
        }

        /// <summary>
        /// Generates tons of garbage.
        /// </summary>
        private static void SymbolProcessing(StringBuilder sb, Config config)
        {
            for (int i = 0; i < config.symbolConfigs.Length; i++)
            {
                var conf = config.symbolConfigs[i];
                var stringVersion = sb.ToString();
                if (stringVersion.IndexOf(conf.symbol) == -1)
                {
                    continue;
                }

                var splitted = stringVersion.Split(conf.symbol);
                sb.Clear();
                for (int j = 0; j < splitted.Length; j++)
                {
                    UseSb(sb, conf.remainingSurroundConfig, splitted[j]);
                    if (conf.replace && j < splitted.Length - 1)
                    {
                        sb.Append(conf.replaceWith);
                    }
                }
            }
        }
    }
}