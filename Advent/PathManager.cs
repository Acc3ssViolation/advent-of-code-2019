using System.Runtime.InteropServices;

namespace Advent
{
    internal static class PathManager
    {
#if DEBUG
        private static string BaseDir
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    return "D:/Projects/advent-of-code-2019/Advent/Advent/";
                else
                    return AppContext.BaseDirectory;
            }
        }
#else
        private static readonly string BaseDir = AppContext.BaseDirectory;
#endif

        public static readonly string DataDirectory = Path.Combine(BaseDir, "Data");
    }
}
