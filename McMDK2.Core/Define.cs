using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McMDK2.Core
{
    public class Define
    {

        public static string FilePath
        {
            get
            {
                return System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            }
        }

        public static string Version
        {
            get
            {
                return VersionNo + "." + ReleaseNo;
            }
        }

        private static Log.Logger _logger;
        public static Log.Logger GetLogger()
        {
            if (_logger == null)
            {
                _logger = new Log.Logger("McMDK - Main");
            }
            return _logger;
        }

        public static bool IsDebug
        {
            get
            {
#if DEBUG
                return true;
#else
                return false;
#endif
            }
        }

        private static readonly string VersionNo = "2.0.0";

        private static readonly long ReleaseNo = 25;

        public static readonly string CurrentDirectory = System.IO.Directory.GetCurrentDirectory();

        /// <summary>
        /// CurrentDirectory/plugins
        /// </summary>
        public static readonly string PluginDirectory = CurrentDirectory + "\\plugins";

        /// <summary>
        /// CurrentDirectory/projects
        /// </summary>
        public static readonly string ProjectsDirectory = CurrentDirectory + "\\projects";

        /// <summary>
        /// CurrentDirectory/logs
        /// </summary>
        public static readonly string LogDirectory = CurrentDirectory + "\\logs";

        /// <summary>
        /// CurrentDirectory/assets
        /// </summary>
        public static readonly string AssetsDirectory = CurrentDirectory + "\\assets";

        /// <summary>
        /// CurrentDirectory/cache
        /// </summary>
        public static readonly string CacheDirectory = CurrentDirectory + "\\cache";


        public static readonly string UpdateVersionUrl = "https://api.tuyapin.net/mcmdk/2/latest";

        public static readonly string NewsFeedUrl = "http://blog.tuyapin.net/feed/?cat=10";
    }
}
