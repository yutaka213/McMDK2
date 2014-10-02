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

        private static ApplicationInternalSettings _internalSettings;
        public static ApplicationInternalSettings GetInternalSettings()
        {
            if (_internalSettings == null)
            {
                _internalSettings = new ApplicationInternalSettings();
                _internalSettings.Reload();
            }
            return _internalSettings;
        }

        private static ApplicationSettings _settings;
        public static ApplicationSettings GetSettings()
        {
            if (_settings == null)
            {
                _settings = new ApplicationSettings();
                _settings.Reload();
            }
            return _settings;
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

        private static bool? _FoundNewVersion = null;

        public static bool FoundNewVersion
        {
            get
            {
                if (_FoundNewVersion == null)
                    return false;
                return (bool)_FoundNewVersion;
            }
            set
            {
                if (_FoundNewVersion == null)
                    _FoundNewVersion = value;
            }
        }

        private static readonly string VersionNo = "2.0.0";

        private static readonly long ReleaseNo = 27;

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


        public static readonly string SettingFile = AssetsDirectory + "\\settings.xml";

        private const string ApiEndPoint = "https://api.tuyapin.net/mcmdk/2/";

        public static readonly string ApiUpdate = ApiEndPoint + "client/update.json";
        public static readonly string ApiCreateClientToken = ApiEndPoint + "client/token/create/{0}.json";
        public static readonly string ApiValidateClientToken = ApiEndPoint + "client/token/validation/{0}.json";

        public static readonly string ApiVersionsList = ApiEndPoint + "versions/list.php";
        public static readonly string ApiForgeVerList = ApiEndPoint + "versions/all_versions/{0}";
        public static readonly string ApiForgeRecommendedVerList = ApiEndPoint + "versions/recommended/{0}";


        public static readonly string UpdateUrl = "http://tuyapin.net/mcmdk/update.xml";

        public static readonly string NewsFeedUrl = "http://blog.tuyapin.net/feed/?cat=10";
    }
}
