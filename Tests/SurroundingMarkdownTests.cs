using NUnit.Framework;

namespace E7.RichUp.Tests
{
    public class SurroundingMarkdownTests
    {
        [Test]
        public void ItalicMatchingBold()
        {
            var cf = new Config();
            cf.italicSurround.customSurround = true;
            cf.italicSurround.customOpening = "io";
            cf.italicSurround.customClosing = "ic";
            string quiz = @"
Should not **match** two times.
";
            string ans = @"
Should not *iomatchic* two times.
";
            var processed = TextProcessingLogic.Process(quiz, cf);
            //The wrong one results in "ioicmatchioic".
            Assert.That(processed, Is.EqualTo(ans));
        }
        
        [Test]
        public void BoldMatchingItalicBold()
        {
            var cf = new Config();
            cf.boldSurround.customSurround = true;
            cf.boldSurround.customOpening = "io";
            cf.boldSurround.customClosing = "ic";
            string quiz = @"
Should not ***match*** two times.
";
            string ans = @"
Should not *iomatchic* two times.
";
            var processed = TextProcessingLogic.Process(quiz, cf);
            Assert.That(processed, Is.EqualTo(ans));
        }
        
        [Test]
        public void AsteriskPriority()
        {
            var cf = new Config();
            cf.boldSurround.customSurround = true;
            cf.boldSurround.customOpening = "bo";
            cf.boldSurround.customClosing = "bc";
            cf.italicSurround.customSurround = true;
            cf.italicSurround.customOpening = "io";
            cf.italicSurround.customClosing = "ic";
            string quiz = @"
Should not ***match*** two times.
";
            string ans = @"
Should not iobomatchbcic two times.
";
            var processed = TextProcessingLogic.Process(quiz, cf);
            Assert.That(processed, Is.EqualTo(ans));
        }

        [Test]
        public void Asterisks()
        {
            var cf = new Config();
            cf.boldSurround.customSurround = true;
            cf.boldSurround.customOpening = "bo";
            cf.boldSurround.customClosing = "bc";

            cf.italicSurround.customSurround = true;
            cf.italicSurround.customOpening = "io";
            cf.italicSurround.customClosing = "ic";

            cf.boldItalicSurround.customSurround = true;
            cf.boldItalicSurround.customOpening = "xo";
            cf.boldItalicSurround.customClosing = "xc";
            string quiz = @"
**When** will **5argon** be able to *finish* his *music game* ***Mel Cadence***??
";
            string ans = @"
boWhenbc will bo5argonbc be able to iofinishic his iomusic gameic xoMel Cadencexc??
";
            var processed = TextProcessingLogic.Process(quiz, cf);
            Assert.That(processed, Is.EqualTo(ans));
        }
        
        [Test]
        public void InlineCodeTest()
        {
            var cf = new Config();
            cf.inlineCodeSurround.customSurround = true;
            cf.inlineCodeSurround.customOpening = "ico";
            cf.inlineCodeSurround.customClosing = "icc";

            string quiz = @"
**When** `will **5argon** be able to` *finish* his *music `game`* ***Mel Cadence***??
";
            string ans = @"
**When** icowill **5argon** be able toicc *finish* his *music icogameicc* ***Mel Cadence***??
";
            var processed = TextProcessingLogic.Process(quiz, cf);
            Assert.That(processed, Is.EqualTo(ans));
        }
    }
}