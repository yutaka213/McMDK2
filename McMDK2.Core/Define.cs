using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using McMDK2.Core.Log;

namespace McMDK2.Core
{
    public static class Define
    {

        public static string FilePath
        {
            get
            {
                return Process.GetCurrentProcess().MainModule.FileName;
            }
        }

        public static string Version
        {
            get
            {
                return VersionNo + "." + ReleaseNo;
            }
        }

        private static Logger _logger;
        public static Logger GetLogger()
        {
            if (_logger == null)
            {
                _logger = new Logger("McMDK - Main");
            }
            return _logger;
        }

        private static ApplicationInternalSettings _internalSettings;
        public static ApplicationInternalSettings GetInternalSettings()
        {
            if (_internalSettings == null)
            {
                _internalSettings = new ApplicationInternalSettings();
                _internalSettings.Upgrade();
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
                _settings.Upgrade();
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

        private static bool? _IsOfflineMode = null;

        public static bool IsOfflineMode
        {
            get
            {
                if (_IsOfflineMode == null)
                    return false;
                return (bool)_IsOfflineMode;
            }
            set
            {
                if (_IsOfflineMode == null)
                    _IsOfflineMode = value;
            }
        }

        private const string VersionNo = "2.0.0";

        private const long ReleaseNo = 27;

        private static readonly string CurrentDirectory = Directory.GetCurrentDirectory();

        /// <summary>
        /// CurrentDirectory/plugins
        /// </summary>
        public static readonly string PluginDirectory = Path.Combine(CurrentDirectory, "plugins");

        /// <summary>
        /// CurrentDirectory/projects
        /// </summary>
        public static readonly string ProjectsDirectory = Path.Combine(CurrentDirectory, "projects");

        /// <summary>
        /// CurrentDirectory/logs
        /// </summary>
        public static readonly string LogDirectory = Path.Combine(CurrentDirectory, "logs");

        /// <summary>
        /// CurrentDirectory/assets
        /// </summary>
        public static readonly string AssetsDirectory = Path.Combine(CurrentDirectory, "assets");

        /// <summary>
        /// CurrentDirectory/cache
        /// </summary>
        public static readonly string CacheDirectory = Path.Combine(CurrentDirectory, "cache");

        /// <summary>
        /// AssetsDirectory/settings.xml
        /// </summary>
        public static readonly string SettingFile = Path.Combine(AssetsDirectory, "settings.xml");

        private const string ApiEndPoint = "https://api.tuyapin.net/mcmdk/2/";

        /// <summary>
        /// https://api.tuyapin.net/mcmdk/2/client/update.json
        /// </summary>
        public const string ApiUpdate = ApiEndPoint + "client/update.json";

        [Obsolete]
        public static readonly string ApiCreateClientToken = ApiEndPoint + "client/token/create/{0}.json";

        [Obsolete]
        public static readonly string ApiValidateClientToken = ApiEndPoint + "client/token/validation/{0}.json";

        /// <summary>
        /// https://api.tuyapin.net/mcmdk/2/minecraft/version.json
        /// </summary>
        public const string ApiVersionsList = ApiEndPoint + "minecraft/version.json";

        [Obsolete]
        public static readonly string UpdateUrl = "http://tuyapin.net/mcmdk/update.xml";

        public const string NewsFeedUrl = "http://blog.tuyapin.net/feed/?cat=10";

        public const string IdentifierDirectory = "DIRECTORY";

        public const string IdentifierMods = "Mod";
    }
}
