using NUnit.Framework;

namespace E7.RichUp.Tests
{
    public class LineStartTests
    {
        [Test]
        public void Headings()
        {
            var cf = new RichUpConfig();
            cf.heading4Surround.customSurround = true;
            cf.heading4Surround.customOpening = "o4";
            cf.heading4Surround.customClosing = "f4";
            cf.heading2Surround.customSurround = true;
            cf.heading2Surround.customOpening = "o2";
            cf.heading2Surround.customClosing = "f2";

            string quiz = @"
# Heading 1
# Heading 1
## Heading 2
## Heading 2
nope## Heading 2
### Heading 3
### Heading 3
nope#### Heading 4
#### Heading 4
#### Heading 4
";

            string ans = @"
# Heading 1
# Heading 1
o2Heading 2f2
o2Heading 2f2
nope## Heading 2
### Heading 3
### Heading 3
nope#### Heading 4
o4Heading 4f4
o4Heading 4f4
";
            var processed = RichUpTextProcessing.Process(quiz, cf);
            Assert.That(processed, Is.EqualTo(ans));
        }

        [Test]
        public void ListsAndBlockquotes()
        {
            var cf = new RichUpConfig();
            cf.listSurround.customSurround = true;
            cf.listSurround.customOpening = "ol";
            cf.listSurround.customClosing = "fl";
            cf.blockquoteSurround.customSurround = true;
            cf.blockquoteSurround.customOpening = "ob";
            cf.blockquoteSurround.customClosing = "fb";

            string quiz = @"
# Heading 1
# Heading 1
## Heading 2
## Heading 2
- Hi
- Yo
My - Name - Is - 5argon
- Nested - Inside
## Heading 2
### Heading 3
> Hey
### Heading 3
> Yo
> Nested > Inside
My > Name > Is > 5argon
#### Heading 4
#### Heading 4
#### Heading 4
";

            string ans = @"
# Heading 1
# Heading 1
## Heading 2
## Heading 2
olHifl
olYofl
My - Name - Is - 5argon
olNested - Insidefl
## Heading 2
### Heading 3
obHeyfb
### Heading 3
obYofb
obNested > Insidefb
My > Name > Is > 5argon
#### Heading 4
#### Heading 4
#### Heading 4
";
            var processed = RichUpTextProcessing.Process(quiz, cf);
            Assert.That(processed, Is.EqualTo(ans));
        }
    }
}