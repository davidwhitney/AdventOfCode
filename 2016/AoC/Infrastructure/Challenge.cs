using System.IO;

namespace AoC.Infrastructure
{
    public abstract class Challenge
    {
        protected string[] Input => File.ReadAllLines(GetType().Name + ".txt");
    }
}
