using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Aoc2018
{
    [TestFixture]
    public class Day2
    {
        public List<string> Input =>
            _input.Split(new[] {Environment.NewLine}, StringSplitOptions.RemoveEmptyEntries).ToList();

        [Test]
        public void Example()
        {
            var checksum = Counter(new[]
            {
                "abcdef",
                "bababc",
                "abbcde",
                "abcccd",
                "aabcdd",
                "abcdee",
                "ababab"
            });

            Assert.That(checksum, Is.EqualTo(12));
        }

        [Test]
        public void Example2()
        {
            var close = FindClose(new []
            {
                "abcde",
                "fghij",
                "klmno",
                "pqrst",
                "fguij",
                "axcye",
                "wvxyz"
            });

            Assert.That(close, Is.EqualTo("fgij"));
        }

        [Test]
        public void Star1()
        {
            var checksum = Counter(Input);

            Assert.That(checksum, Is.EqualTo(6642));
        }

        [Test]
        public void Star2()
        {
            var close = FindClose(Input);

            Assert.That(close, Is.EqualTo("cvqlbidheyujgtrswxmckqnap"));
        }

        private static string FindClose(IEnumerable<string> shelf)
        {
            foreach (var item in shelf)
            {
                var otherItems = shelf.Except(new[] {item});
                foreach (var other in otherItems)
                {
                    var different = item.Length;
                    var common = new List<char>();
                    for (var index = 0; index < item.Length; index++)
                    {
                        var myLetter = item[index];
                        var otherLetter = other[index];

                        if (myLetter == otherLetter)
                        {
                            common.Add(myLetter);
                            different--;
                        }
                    }

                    if (different == 1)
                    {
                        return string.Join("", common);
                    }
                }
            }

            throw new Exception("Nope!");
        }

        private static int Counter(IEnumerable<string> shelf)
        {
            var containsPairs = 0;
            var containsTriplets = 0;

            foreach (var item in shelf)
            {
                var grouped = item.GroupBy(x => x).ToList();
                containsPairs = grouped.Count(group => @group.Count() == 2) > 0 
                    ? containsPairs + 1 
                    : containsPairs;

                containsTriplets = grouped.Count(group => @group.Count() == 3) > 0
                    ? containsTriplets + 1
                    : containsTriplets;
            }

            return containsPairs * containsTriplets;
        }

        private string _input = @"ovfclbidieyujztrswxmckgnaw
pmfqlbimheyujztrswxmckgnap
ovfqlbidhetujztrswxmcfgnas
gvfqebddheyujztrswxmckgnap
ovfqlbidheyejztrswxqekgnap
ovzqlbiqheyujztsswxmckgnap
oofqlbidhoyujztoswxmckgnap
ovfqlbicqeyujztrswxmckgncp
ovfelbidheyujltrswxmcwgnap
ovfqlbidheyujzerswxmchgnaf
bvfqlbidheyxjztnswxmckgnap
ovfqlbidheyugztrswamnkgnap
ovfqxbidheyujrtrswxmckgbap
ovfqlbidheyujztrdwxqckgjap
ovfqebiqheyujztrscxmckgnap
avfqlbidheyvjztkswxmckgnap
ovfqlbidheyujktrswxvskgnap
ovfqlbidheeujztrswrmckgnae
ovaqlbidheydjztrswxmchgnap
ovfqlbodweyujztpswxmckgnap
xvfqlbidhmyujztrswxmykgnap
ovfqlnidheyujztxswumckgnap
ovfqlbidhexujztrswxyckgeap
ovfqlkidhekubztrswxmckgnap
ovfqlbidheysjzkrsxxmckgnap
oxfqebidheyujzprswxmckgnap
ovfqlbidhetujztrswmmckghap
ovfclbidhuyujztrswrmckgnap
ovfqlbijhdyujztrswxmcvgnap
ovfqlkidhyyujztrswxvckgnap
ovfqlbiehlyujztrswxhckgnap
ovfqlbidheyxjjtrsdxmckgnap
jvfqlbidheyujztrvwxmckcnap
ovfvlbidheyujzhrswxmckgnzp
ovfqnbidhuyujztrswfmckgnap
ovfrlbidheyujztpswxmckgnat
ovfqpbidheyujztrywxmcngnap
ovfqlbidheyumrtrswpmckgnap
ovfqlbidhoyzjztrswxmckgkap
ovfqlbibheyuhztrswxmskgnap
ovfqlbidheybjzfrswxkckgnap
ovfqnbinheyujztrawxmckgnap
ovfqlbidheyujztryxxmckgnao
ovfqzbidheyujztrsuxmckgnpp
ovfqlbidherujztrswxmckgjsp
ovfqlbidheyujhtrywxmckgtap
oofmlbidheyujftrswxmckgnap
ovfqlbidhhyujztrawxmckgnbp
ovfqlbidheyujztrswxeckmnae
lvfqlbidheyujztrswxzckvnap
ovfqlbidheyujztrswxmckqnfr
offqlbidheyrjztrswxmwkgnap
ovnqlbidzeyujztmswxmckgnap
ovfxlbxdheyujztrawxmckgnap
ovfqmbidheyujztrsaxwckgnap
ovfqlbidhryzjztrswxmckgcap
offqlbidheyujnthswxmckgnap
ogmqlbimheyujztrswxmckgnap
ovfqlbidheyulztkswxockgnap
ovfqlbidheyujjtrswxmckypap
ovfqibidheypjztrswxmskgnap
ovfqlbwdhyyujztrswxmckgnnp
ovfqlbidheyujztsvwxmckgkap
odfqlbidoeyujztrswxjckgnap
ovfqlbodpeyujztrswxmcggnap
ovfqlbicheyujztrhwxmcagnap
ovfqlbidheyuaztrgwxmckhnap
ovfwlbidhyyujztrswtmckgnap
ovfqlbidgzyujztrswxmckgcap
ovnqlbcdheyujztrswxmckgnup
ovfqlbieheyujrtrsdxmckgnap
ovfqlbidkeyujztrswfmckgnqp
ovfqlbidtekujztrswxsckgnap
ovfklbedheyujztrscxmckgnap
ovfqltivhnyujztrswxmckgnap
ovfqlbidheyuvuyrswxmckgnap
ovfqlbidheyjjrtrcwxmckgnap
ojfqlbidheyujztrswxmckguvp
ovfqlbidheiqjqtrswxmckgnap
ivfqlfidheyujatrswxmckgnap
cvfqlbidheyujgtrswxmckgnrp
ovfclbidheeujztrswxmckgnaw
ovfqlbhdheyujztrswvmcklnap
ovfqcbidheyvjztaswxmckgnap
ovgqlbijheyujztrswxvckgnap
gvfqlbidheyujvtrswxmckgnaj
ovfqlbidheyujztrdwxmcggnvp
cvfqlbidheyujgtrswxmckqnap
ovfqlbrdheyqjztrswxmckgnaj
ovfqlbidheyujzjrswbmcrgnap
ovfqlbvdheyujxtrswxvckgnap
ovaqlbidheyujctrswxmbkgnap
ovfqlbidheyujztgdwxvckgnap
ovfqlbidhevujztrssxmwkgnap
rvfqlbidheyujztrzwxmckhnap
ovfqmbidheysjztrswxmikgnap
ovfqlbidheiujztrsdxuckgnap
ovfqlbidheyveztrswxmckgnah
ovfqnbiaheytjztrswxmckgnap
ovfqlbidnayujhtrswxmckgnap
ovfqlbidheyujztnswxdckgnag
ovfqlbidheyuyztrswxmzzgnap
ovfqlbiohexujzthswxmckgnap
lvfqlbidheyujztcswxxckgnap
ovuqlbidhfxujztrswxmckgnap
ovfqluidheyujotrswxmrkgnap
ovfalbidheyujztrswxhckgngp
ohjqlbidheyujztrswumckgnap
ovfqxbidhecujztrspxmckgnap
ovfqcbidheyusztrpwxmckgnap
fvfwlbidheyujztrswxmcxgnap
ovfqlbidhxyplztrswxmckgnap
ovfqlbidheyujftrswxdckgrap
ovfqlepdheyujztrswxmckgnjp
ojjqlbidhuyujztrswxmckgnap
ovfqlbwdheyujztrswxmcggeap
ovfqlbidheyujltrscxkckgnap
oifqibidheyujztrswxjckgnap
ovfqlbigheyujztrswdmcqgnap
ovfqlbieheyujztrswxzzkgnap
ovfqlbidheyujztrswmmcgbnap
ovfqlbidhnyujzerswxmkkgnap
ovfqdbinheyujztrswxeckgnap
oveqlbidheyujztrswhmckgnab
ovfqkbytheyujztrswxmckgnap
ovfqlbidheyujstsswxmcklnap
ovfimbidheyujztrewxmckgnap
ovfqebidhequjztrnwxmckgnap
ovfqlbidheyukztrswxmckunwp
oifqlbidheyuwztrswxmckgnao
ovfqlbidweyufztrswxmckgtap
evfqlbidheyujztrswxsckvnap
svbqlbidheyujztrsaxmckgnap
ovfqlbidheyaoztrswxmckjnap
ovfqllidheyujztrswxmckynhp
ohfqlbidheyujatrswtmckgnap
omfjlfidheyujztrswxmckgnap
xvfqlbidheyujutrswxvckgnap
ovfqlbidheyukztsswxmzkgnap
ovfqibidhehujztrswxeckgnap
ovfqlbydheyuoztrswxmcygnap
ovfqlbidheyufztrmwxmckvnap
ovfqrbkdheyujztrswxmckgnaq
ovfqlbigheyuyztrswxmckgzap
ovfqlbidheyujztrsjxmcnnnap
uvfqlbipheyujztrswxmckgnay
ovfqlbddneyujbtrswxmckgnap
tvfqlbidheyujztrswxpckgeap
ovfqlbidheyuiztrhwxmckznap
ovfqlbidheyujzteswxvckgvap
avfqlbidheyijzlrswxmckgnap
oqfqmbidheyujvtrswxmckgnap
ovnqlbidneyujztrswxmckxnap
ovfqlbidfeyujztrswxqckgncp
ovfaybidheyujztrswxrckgnap
ovfqlbidhemujzarnwxmckgnap
ovfqlwidheyujctrsfxmckgnap
ovtelbidheysjztrswxmckgnap
ovfqlbidheyujztrswsmchunap
pvfqlbidheyulztrswxmckynap
ovfqlbzdhezujztfswxmckgnap
ozfqibidheyujztrhwxmckgnap
ovfqlbioheycjztmswxmckgnap
ovfqleidheyujztoswxmckgnhp
ovfqlcidhejujztrswnmckgnap
eqfqlbidheyujztrswxmrkgnap
ovfqlbitheywjztrmwxmckgnap
ovfqlbidheyujptrswnmcggnap
oofqlbidhjyujztrswxmcegnap
ovfqlbidmeyujztrswxmcxgnxp
ovjhlbidhefujztrswxmckgnap
ovfqlbidkeyujzarswxmcugnap
hvyqlridheyujztrswxmckgnap
ovfqlbidheyujxtrswqmckgnpp
ovfqlbidheyuiztrtsxmckgnap
ovfqlbidqeyuuztrbwxmckgnap
ovfqpbidheyujztrswxwekgnap
ovfqltidheyujztrbwxmckgnab
okfxluidheyujztrswxmckgnap
ovfplbidheyujztrsaxmckgnax
wvfqlbidheiujztrswxjckgnap
ovfqlbidheyqjzlrsqxmckgnap
ovfqlbadheyujztrsxxmckgnop
ovfqliidheyujzerswvmckgnap
ovlrlbidheyujztaswxmckgnap
cvzqlbidheyujgtrswxmckqnap
ovfqlbidheyujzuqswxmckgnnp
ovfqlsjdheyujztrswxmcklnap
ovrqlbidheyujztrssrmckgnap
ovcqlbidheyujztrsmxmcognap
ovcqlbidheyjjztrswxmckunap
ovfilbrdhnyujztrswxmckgnap
ovfqlbodheyujztrswxeckqnap
ovfqlbidhuyujqtrswxdckgnap
ovmqlbidheyujderswxmckgnap
ovfylbidheyajzrrswxmckgnap
ovfklbidhtyujzjrswxmckgnap
rvfqlbidheyujztcswxcckgnap
ovfnlyidheyuwztrswxmckgnap
ovfqlbidhexujztrswxmpktnap
ovfplbidheyfjztrswhmckgnap
oyfqlbidmexujztrswxmckgnap
mvfqlbidhcyujztrawxmckgnap
ovfqlbidhqyujdtrswxmcbgnap
ovfqjbidheybjrtrswxmckgnap
ozfqlbidhbyujztrswxmckgpap
okfqlbidheyujztrmwxmckhnap
ovfqlbydheyujzrrswxnckgnap
ovfqtbidheycjztrswxmckgnah
avfqloidheyujztrswxyckgnap
ovfqlbldteyujzyrswxmckgnap
ovfqlbpdhedujztpswxmckgnap
ovfqlbidheyujztrswxucrvnap
ocnqlbidheyujztrswxmwkgnap
ovfqlvidheyujztrswkmckgnlp
evfqlbidheyujzmrswqmckgnap
ovfqlbidhryujztrcwxmekgnap
ovfqlbvdheyujztrzwxmcxgnap
ovfqlridgeyujztrswxmkkgnap
yvfqlbidheyujzthswzmckgnap
ovfqlbidheyujmtrswxyukgnap
ovfqlbidheqgjztrswdmckgnap
dvfzlcidheyujztrswxmckgnap
jvfqlbidheyujztrswxmczgnae
ovfqlbzdheyujztrswxyckgnjp
ovfqlbxdheyujatrswxmcqgnap
ovftlbldheyujztrewxmckgnap
owfqlbitheyujzyrswxmckgnap
ovfqlbidheyujztrswxmchgtvp
ovfqibidheyujzttswxmkkgnap
ovkqlbodheyujztvswxmckgnap
onfqlbbdheyujztrxwxmckgnap
ovfqlbidyeysgztrswxmckgnap
ovfqlbixherujztrswxmcngnap
ovlqlbidfeyujztrswxgckgnap
ovfqlbfdheyujztwswumckgnap
ovfqlbidheeujztrswxmckgkah
ovfqtbqdheyujztrswmmckgnap
bbfqlbigheyujztrswxmckgnap
ovfqljidheyujztrswxmwkgnip
ovfqobidheyujztrsvxmmkgnap
ovfqlbidheydjvtrswxmckanap
ovfqlxidheyujztrswgmckgnep
jvfqlbidhzyujztrswxmckgnay
ovfqlbidhkyujztrswxmlkenap
ovfqobidheyujztrswxmckjnaf
ovfxlbidheyujztrswxmcbgnac
ovfqcbidhtyujztrswxmckqnap
ozfglbidheyujzvrswxmckgnap
ovfqlbidheyujztoswxyckcnap";
    }
}
