using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace McMDK2.Core
{
    /// <summary>
    /// ファイル操作の機能を提供します
    /// </summary>
    public static class FileController
    {
        /// <summary>
        /// ディレクトリを作成します。
        /// </summary>
        public static void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        /// <summary>
        /// 空のファイルを作成します。
        /// </summary>
        public static void CreateFile(string path)
        {
            if (!File.Exists(path))
            {
                File.CreateText(path);
            }
        }

        /// <summary>
        /// ファイル/ディレクトリを削除します。
        /// </summary>
        public static void Delete(string path)
        {
            if (path.EndsWith("/") || path.EndsWith("\\"))
            {
                return;
            }

            if (File.Exists(path))
            {
                File.Delete(path);
            }
            if (Directory.Exists(path))
            {
                DeleteDir(path);
            }
        }

        private static void DeleteDir(string path)
        {
            var dirInfo = new DirectoryInfo(path);
            foreach (var info in dirInfo.GetFiles())
            {
                if ((info.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                {
                    info.Attributes = FileAttributes.Normal;
                }
            }

            foreach (var info in dirInfo.GetDirectories())
            {
                DeleteDir(info.FullName);
            }

            if ((dirInfo.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                dirInfo.Attributes = FileAttributes.Directory;
            }
            dirInfo.Delete(true);
        }

        /// <summary>
        /// ファイル/ディレクトリの存在確認を行います。
        /// </summary>
        public static bool Exists(string path)
        {
            if (Directory.Exists(path) || File.Exists(path))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// ファイル/ディレクトリをコピーします。
        /// </summary>
        public static void Copy(string source, string dest)
        {
            string dirname = Path.GetDirectoryName(dest);
            if (dirname == null)
            {
                throw new ArgumentException("dest");
            }
            if (File.Exists(source))
            {
                if (!Directory.Exists(dest))
                {
                    Directory.CreateDirectory(dirname);
                }
                File.Copy(source, dest);
                return;
            }
            if (Directory.Exists(source))
            {
                CopyDirectory(source, dest);
            }
        }

        private static void CopyDirectory(string source, string dest)
        {
            if (!Directory.Exists(dest))
            {
                Directory.CreateDirectory(dest);
            }

            foreach (string file in Directory.GetFiles(source))
            {
                File.Copy(file, Path.Combine(dest, Path.GetFileName(file)), true);
            }

            foreach (string dir in Directory.GetDirectories(source))
            {
                CopyDirectory(dir, Path.Combine(dest, Path.GetFileName(dir)));
            }
        }

        /// <summary>
        /// ファイル/ディレクトリをリネームします。
        /// </summary>
        public static void Rename(string oldName, string newName)
        {
            if (File.Exists(oldName))
            {
                File.Move(oldName, newName);
                return;
            }
            if (Directory.Exists(oldName))
            {
                Directory.Move(oldName, newName);
            }
        }

        /// <summary>
        /// ファイルリストを取得します。
        /// </summary>
        public static IEnumerable<string> GetLists(string path, string filter = "*", bool isDirectory = false, bool isSearchAllDirectories = false)
        {
            if (Directory.Exists(path))
            {
                if (isDirectory)
                {
                    if (isSearchAllDirectories)
                        return Directory.GetDirectories(path, filter, SearchOption.AllDirectories);
                    return Directory.GetDirectories(path);
                }
                if (isSearchAllDirectories)
                    return Directory.GetFiles(path, filter, SearchOption.AllDirectories);
                return Directory.GetFiles(path);
            }
            throw new DirectoryNotFoundException(path);
        }

        /// <summary>
        /// ファイルを読み込みます。
        /// </summary>
        public static string LoadFile(string path)
        {
            if (File.Exists(path))
            {
                var sr = new StreamReader(path);
                return sr.ReadToEnd();
            }
            throw new FileNotFoundException(path);
        }

        /// <summary>
        /// ファイルのハッシュ値を計算します。
        /// </summary>
        /// <param name="path">ファイルパス</param>
        /// <param name="type">ハッシュタイプ</param>
        /// <returns></returns>
        public static string GetHashValue(string path, HashType type)
        {
            HashAlgorithm hashAlgo = null;
            switch (type)
            {
                case HashType.MD5:
                    hashAlgo = new MD5CryptoServiceProvider();
                    break;

                case HashType.RIPEMD160:
                    hashAlgo = new RIPEMD160Managed();
                    break;

                case HashType.SHA1:
                    hashAlgo = new SHA1CryptoServiceProvider();
                    break;

                case HashType.SHA256:
                    hashAlgo = new SHA256CryptoServiceProvider();
                    break;

                case HashType.SHA384:
                    hashAlgo = new SHA384CryptoServiceProvider();
                    break;

                case HashType.SHA512:
                    hashAlgo = new SHA512CryptoServiceProvider();
                    break;

                default:
                    hashAlgo = new MD5CryptoServiceProvider();
                    break;
            }

            byte[] hash = hashAlgo.ComputeHash(new FileStream(path, FileMode.Open, FileAccess.Read));
            hashAlgo.Clear();

            var sb = new StringBuilder();
            foreach (var b in hash)
                sb.Append(b.ToString("x2"));

            return sb.ToString();
        }

        public enum HashType
        {
            MD5,

            RIPEMD160,

            SHA1,

            SHA256,

            SHA384,

            SHA512
        }
    }
}
