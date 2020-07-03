using NUnit.Framework;

namespace E7.RichUp.Tests
{
    public class ItemFormatterTests
    {
        private class ItemFormatter : IItemFormatter
        {
            public string FormatItem(string itemName)
            {
                switch (itemName)
                {
                    case "one": return "ONE";
                    case "two": return "TWO";
                    case "three": return "THREE";
                }
                return "";
            }
        }

        [Test]
        public void ItemFormatting()
        {
            var quiz = "{one} {one} {one{two}} {two} {{three}}";
            var ans = "ONE ONE {oneTWO} TWO {THREE}";
            var formatted = TextProcessingLogic.FormatItems(quiz, new ItemFormatter());
            Assert.That(formatted, Is.EqualTo(ans));
        }
    }
}