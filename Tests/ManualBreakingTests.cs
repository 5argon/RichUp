using NUnit.Framework;
using UnityEngine;

namespace E7.RichUp.Tests
{
    public class ManualBreakingTests
    {
        const string question = "AAAAA|AA BBBBBB CCC DD";
        const string question2 = "AAAAA|AA BBBBBB CCC DD ";
        const string question3 = "AAAAA|AA BBBBBB CCC DD |";
        const string question4 = "AAAAA||||AA BBBBBB CCC DD ||";

        const string answer =
            "<nobr>AAAAA</nobr><zwsp><nobr>AA</nobr><zwsp><nobr> </nobr><zwsp><nobr>BBBBBB</nobr><zwsp><nobr> </nobr><zwsp><nobr>CCC</nobr><zwsp><nobr> </nobr><zwsp><nobr>DD</nobr>";

        const string answer2 =
            "<nobr>AAAAA</nobr><zwsp><nobr>AA</nobr><zwsp><nobr> </nobr><zwsp><nobr>BBBBBB</nobr><zwsp><nobr> </nobr><zwsp><nobr>CCC</nobr><zwsp><nobr> </nobr><zwsp><nobr>DD</nobr><zwsp><nobr> </nobr>";

        const string answer3 =
            "<nobr>AAAAA</nobr><zwsp><nobr>AA</nobr><zwsp><nobr> </nobr><zwsp><nobr>BBBBBB</nobr><zwsp><nobr> </nobr><zwsp><nobr>CCC</nobr><zwsp><nobr> </nobr><zwsp><nobr>DD</nobr><zwsp><nobr> </nobr>";

        const string answer4 =
            "<nobr>AAAAA</nobr><zwsp><nobr>AA</nobr><zwsp><nobr> </nobr><zwsp><nobr>BBBBBB</nobr><zwsp><nobr> </nobr><zwsp><nobr>CCC</nobr><zwsp><nobr> </nobr><zwsp><nobr>DD</nobr><zwsp><nobr> </nobr>";

        [Test]
        public void ManualBreaking() => QA(question, answer);

        [Test]
        public void ManualBreakingWithTrailingSpace() => QA(question2, answer2);

        [Test]
        public void ManualBreakingWithTrailingToken() => QA(question3, answer3);

        [Test]
        public void ManualBreakingWithBunchOfTokens() => QA(question4, answer4);

        void QA(string question, string answer)
        {
            var config = new Config();
            var mbc = new ManualBreakingConfig();
            mbc.symbol = '|';
            config.manualBreakingConfig = mbc;
            var done = TextProcessingLogic.Process(question, config);
            Assert.That(done, Is.EqualTo(answer));
        }
    }
}