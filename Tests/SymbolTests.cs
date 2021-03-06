﻿using NUnit.Framework;

namespace E7.RichUp.Tests
{
    public class SymbolTests
    {
        Config replaceConfig;
        Config remainingSurroundConfig;

        [SetUp]
        public void Before()
        {
            replaceConfig = new Config();
            var symbolConfig = new SymbolConfig();
            symbolConfig.symbol = '|';
            symbolConfig.replace = true;
            symbolConfig.replaceWith = "haha";
            replaceConfig.symbolConfigs = new SymbolConfig[] {symbolConfig};

            symbolConfig.remainingSurroundConfig.styleSurround = true;
            symbolConfig.remainingSurroundConfig.styleName = "mystyle";

            remainingSurroundConfig = new Config();
            remainingSurroundConfig.symbolConfigs = new SymbolConfig[] {symbolConfig};
        }

        [Test]
        public void SymbolNotFound()
        {
            var processed = TextProcessingLogic.Process("Hello, no symbol", replaceConfig);
            Assert.That(processed, Is.EqualTo("Hello, no symbol"));

            var processed2 = TextProcessingLogic.Process("Hello, no symbol", remainingSurroundConfig);
            Assert.That(processed2, Is.EqualTo("Hello, no symbol"));
        }

        [Test]
        public void Replace()
        {
            var processed = TextProcessingLogic.Process("Hello| there is a | symbol", replaceConfig);
            Assert.That(processed, Is.EqualTo("Hellohaha there is a haha symbol"));
        }

        [Test]
        public void Surround()
        {
            var processed = TextProcessingLogic.Process("Hello| there is a | symbol", remainingSurroundConfig);
            Assert.That(processed,
                Is.EqualTo(
                    "<style=\"mystyle\">Hello</style>haha<style=\"mystyle\"> there is a </style>haha<style=\"mystyle\"> symbol</style>"));
        }

        [Test]
        public void MultilineEverything()
        {
            var multilineEverything = new Config();
            var symbolConfig = new SymbolConfig();
            symbolConfig.replace = true;
            symbolConfig.symbol = '|';
            symbolConfig.remainingSurroundConfig.customSurround = true;
            symbolConfig.replaceWith = "z";
            symbolConfig.remainingSurroundConfig.customOpening = "<";
            symbolConfig.remainingSurroundConfig.customClosing = ">";
            multilineEverything.symbolConfigs = new[] {symbolConfig};

            var processed =
                TextProcessingLogic.Process("Mu|lti|line line 1.\nMul|tiline li|ne 2.", multilineEverything);
            Assert.That(processed,
                Is.EqualTo(
                    "<Mu>z<lti>z<line line 1.>\n<Mul>z<tiline li>z<ne 2.>"));
        }
    }
}