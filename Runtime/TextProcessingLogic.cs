using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;

namespace E7.RichUp
{
    internal static class TextProcessingLogic
    {
        internal static string FormatItems(string original, IItemFormatter formatter)
        {
            if (string.IsNullOrEmpty(original))
            {
                return original;
            }

            return new Regex("{([^{}]+?)}", RegexOptions.Multiline).Replace(original, Replacer);

            string Replacer(Match match)
            {
                return formatter?.FormatItem(match.Groups[1].Value) ?? "";
            }
        }

        internal static string Process(string original, Config config)
        {
            if (string.IsNullOrEmpty(original))
            {
                return original;
            }

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

            var symbolRule = (config.symbolConfigs != null && config.symbolConfigs.Length > 0);
            var manualBreakingRule = config.manualBreakingConfig.symbol != '\0';
            if (symbolRule || manualBreakingRule)
            {
                var lineSplitted = r.Split('\n');
                var sbArray = new StringBuilder[lineSplitted.Length];
                for (int i = 0; i < sbArray.Length; i++)
                {
                    sbArray[i] = new StringBuilder(lineSplitted[i]);
                    if (manualBreakingRule)
                    {
                        ManualLineBreakProcessing(sbArray[i], config);
                    }

                    if (symbolRule)
                    {
                        SymbolProcessing(sbArray[i], config);
                    }
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

        private static bool IsWhiteSpace(char c)
        {
            bool isWhiteSpace = char.IsWhiteSpace(c);
            var charCode = c;
            //This is copied from TMP source code.
            if ((isWhiteSpace || charCode == 0x200B || charCode == 0x2D || charCode == 0xAD) && charCode != 0xA0 &&
                charCode != 0x2007 && charCode != 0x2011 && charCode != 0x202F && charCode != 0x2060)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// https://docs.microsoft.com/en-us/dotnet/api/system.char.iswhitespace?view=netcore-3.1
        /// </summary>
        static char[] spaces = new char[]
        {
            '\u0020',
            '\u00A0',
            '\u1680',
            '\u2000',
            '\u2001',
            '\u2002',
            '\u2003',
            '\u2004',
            '\u2005',
            '\u2006',
            '\u2007',
            '\u2008',
            '\u2009',
            '\u200A',
            '\u202F',
            '\u205F',
            '\u3000',
            '\u2028',
            '\u2029',
            '\u0009',
            '\u000A',
            '\u000B',
            '\u000C',
            '\u000D',
            '\u0085',
            'x'
        };

        static List<string> splitList = new List<string>(64);

        private static void ManualLineBreakProcessing(StringBuilder sb, Config config)
        {
            var symbol = config.manualBreakingConfig.symbol;
            var stringVersion = sb.ToString();
            if (stringVersion.IndexOf(symbol) == -1)
            {
                return;
            }

            spaces[spaces.Length - 1] = symbol;
            var splitted = stringVersion.Split(symbol);
            //If we wrap <nobr> now it will also wrap over spaces and make them unbreakable.
            //Split more according to white space characters which make it more breakable
            //in addition to the symbols. But unlike the symbols, keep those characters.
            splitList.Clear();
            splitList.AddRange(splitted);
            for (int i = 0; i < splitList.Count; i++)
            {
                var theString = splitList[i];
                if (string.IsNullOrEmpty(theString))
                {
                    splitList.RemoveAt(i);
                    i--;
                    continue;
                }

                var find = theString.IndexOfAny(spaces);
                //If we have "AAAA BBBBBB"
                //Then we have to split to [AAAA][ ][BBBBBB]
                //If more, then let that process the next round.
                //e.g. AAAA BBBBBB CCC -> [AAAA][ ][BBBBBB CCC] -> [AAAA][ ][BBBBBB][ ][CCC]
                //TODO : Make other symbols only valid at beginning or ending of line only.
                //e.g. AAAA(BBBBBB -> [AAAA][(BBBBBB]
                if (find != -1)
                {
                    bool final = find == theString.Length - 1;
                    string left = theString.Substring(0, find);
                    char mid = theString[find];
                    splitList.RemoveAt(i);
                    splitList.Insert(i, left);
                    splitList.Insert(i + 1, mid.ToString());
                    if (!final)
                    {
                        string right = theString.Substring(find + 1);
                        splitList.Insert(i + 2, right);
                    }

                    i += 1; //One more is added on the next round
                }
            }

            sb.Clear();
            var surr = new SurroundConfig
            {
                customSurround = true,
                customOpening = "<nobr>",
                customClosing = "</nobr>",
            };
            for (int j = 0; j < splitList.Count; j++)
            {
                UseSb(sb, surr, splitList[j]);
                if (j < splitList.Count - 1)
                {
                    sb.Append("<zwsp>");
                }
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