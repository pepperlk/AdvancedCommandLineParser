using System.Threading.Tasks;

namespace AdvancedCommandLineParser
{
    public abstract class Command
    {
        public abstract Task<int> Run();
    }
}