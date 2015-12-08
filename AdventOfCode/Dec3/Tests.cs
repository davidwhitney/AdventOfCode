using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode.Dec1;
using NUnit.Framework;

namespace AdventOfCode.Dec3
{
    [TestFixture]
    public class Tests
    {
        private SantaGps _gps;

        [SetUp]
        public void SetUp()
        {
            _gps = new SantaGps();
        }

        [TestCase(">", 2)]
        [TestCase("^>v<", 4)]
        [TestCase("^v^v^v^v^v", 2)]
        public void DeliverTo_GivenDirections_CountsDistinctLocationsVisited(string directions, int uniques)
        {
            _gps.DeliverTo(directions);

            Assert.That(_gps.DistinctLocationsDelivered, Is.EqualTo(uniques));
        }

        [Test]
        public void DeliverTo_DoChallange()
        {
            var contents = File.ReadAllText("c:\\dev\\AdventOfCode\\AdventOfCode\\Dec3\\Test.txt");

            _gps.DeliverTo(contents);

            Assert.That(_gps.DistinctLocationsDelivered, Is.EqualTo(2081));
        }
    }
}
