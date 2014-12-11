using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using McMDK2.Core;
using McMDK2.Core.Utils;
using McMDK2.Py.Mcp;

namespace McMDK2.Py.Fml
{
    /// <summary>
    /// Imitate the forge/fml/fml.py's workings.
    /// </summary>
    internal static class FmlCommands
    {
        // 1.3.2-4.2.5.313
        private async static Task<bool> DownloadDeps(string mcp_dir)
        {
            string bin_dir = Path.Combine(mcp_dir, "runtime", "bin");
            string ff_path = Path.Combine(bin_dir, "fernflower.jar");
            if (!FileController.Exists(ff_path))
            {
                if (FileController.Exists(Path.Combine(Define.CacheDirectory, "fernflower.jar")))
                {
                    FileController.Copy(Path.Combine(Define.CacheDirectory, "fernflower.jar"), ff_path);
                }
                else
                {
                    var client = new WebClient();
                    await client.DownloadFileTaskAsync("https://www.dropbox.com/s/vgwgb3ahevs4dvv/fernflower.jar?dl=1", Path.Combine(Define.CacheDirectory, "fernflower.jar"));
                    FileController.Copy(Path.Combine(Define.CacheDirectory, "fernflower.jar"), ff_path);
                    // >> Downloaded Fernflower successfully
                }
            }

            var libs = new List<string>
            {
                "argo-2.25.jar",
                "guava-12.0.1.jar",
                "guava-12.0.1-sources.jar",
                "asm-all-4.0.jar",
                "asm-all-4.0-source.jar"
            };
            string libF = Path.Combine(mcp_dir, "lib");
            FileController.CreateDirectory(libF);
            foreach (var lib in libs)
            {
                if (FileController.Exists(Path.Combine(Define.CacheDirectory, lib)))
                {
                    FileController.Copy(Path.Combine(Define.CacheDirectory, lib), Path.Combine(libF, lib));
                }
                else
                {
                    var client = new WebClient();
                    await client.DownloadFileTaskAsync("http://u.tuyapin.net/" + lib, Path.Combine(Define.CacheDirectory, lib));
                    FileController.Copy(Path.Combine(Define.CacheDirectory, lib), Path.Combine(libF, lib));
                    // >> Download {lib} successfully
                }
            }

            return true;
        }

        private static void PreDecompile(string mcp_dir, string fml_dir)
        {
            string backup = Path.Combine(mcp_dir, "jars", "bin", "minecraft.jar.backup");
            FileController.Copy(backup, backup + ".backup");

            backup = Path.Combine(mcp_dir, "jars", "bin", "minecraft_server.jar");
            FileController.Copy(backup, backup + ".backup");

            string client_jar = Path.Combine(mcp_dir, "jars", "bin", "minecraft.jar.backup");
            string server_jar = Path.Combine(mcp_dir, "jars", "minecraft_server.jar.backup");
            if (!FileController.Exists(client_jar) || !FileController.Exists(server_jar))
                // >> Could not find Client jar, decompile requires both client and server.
                return;

            // TODO: MD5 Check.
        }

        private static void PostDecompile(string mcp_dir, string fml_dir)
        {
            string bin_dir = Path.Combine(mcp_dir, "jars", "bin");
            //string back_jar = Path.Combine(bin_dir, "minecraft.jar.backup");
            string src_jar = Path.Combine(bin_dir, "minecraft.jar");

            // >> Stripping META-INF from minecraft.jar
            using (var stream = File.Open(src_jar, FileMode.Open))
            {
                using (var archive = new ZipArchive(stream, ZipArchiveMode.Update))
                {
                    foreach (var entry in archive.Entries)
                    {
                        if (entry.FullName.StartsWith("META-INF"))
                        {
                            // >> Skipping entry.FullName
                            entry.Delete();
                        }
                    }
                }
            }
        }

        private static void CleanupSource(string path)
        {
            var regex_cases_before = new Regex("((case|default).+\r?\n)\r?\n'", RegexOptions.Multiline);
            var regex_cases_after = new Regex("\r?\n(\r?\n[ \t]+(case|default))", RegexOptions.Multiline);

            var filelist = FileController.GetLists(path, "*.java", isSearchAllDirectories: true);
            foreach (var cur_file in filelist)
            {
                // update_file
                string temp_file = cur_file + ".tmp";
                string buf = FileController.LoadFile(cur_file);
                buf = regex_cases_before.Replace(buf, m => m.Groups[1].Value);
                buf = regex_cases_after.Replace(buf, m => m.Groups[1].Value);
                using (var sw = new StreamWriter(temp_file))
                {
                    sw.WriteLine(buf);
                }
                FileController.Rename(temp_file, cur_file);
            }
        }

        public static void SetupMcp(string fml_dir, string mcp_dir)
        {
            string backup = Path.Combine(mcp_dir, "runtime", "commands.py.bck");
            string runtime = Path.Combine(mcp_dir, "runtime", "commands.py");
            string patch = Path.Combine(fml_dir, "commands.patch");

            // >> Setting up MCP
            if (FileController.Exists(backup))
            {
                // >> Restoring commands.py by backup
                FileController.Delete(runtime);
                FileController.Copy(backup, runtime);
            }
            else
            {
                // >> Backing up commands.py
                FileController.Copy(runtime, backup);
            }

            // >> Patching file
            Patcher.ApplyPatch(patch, mcp_dir, "-uf -i {0}", Path.Combine(mcp_dir, "runtime"));

            string mcp_conf = Path.Combine(mcp_dir, "conf");
            string fml_conf = Path.Combine(fml_dir, "conf");

            FileController.Delete(mcp_conf);

            // >> Copying FML conf
            FileController.Copy(fml_conf, mcp_conf);

            // >> Fixing MCP Workspace
            if (!FileController.Exists(Path.Combine(fml_dir, "eclipse", "Clean-Client")))
            {
                string mcp_eclipse = Path.Combine(mcp_dir, "eclipse");
                FileController.Delete(mcp_eclipse);
                FileController.Copy(Path.Combine(fml_dir, "eclipse"), mcp_eclipse);
            }
        }

        public async static void SetupFml(string fml_dir, string mcp_dir)
        {
            string src_dir = Path.Combine(mcp_dir, "src");
            if (FileController.Exists(src_dir))
            {
                McpCommands.Cleanup();
            }

            if (!(await DownloadDeps(mcp_dir)))
                return;

            PreDecompile(mcp_dir, fml_dir);
            McpCommands.Decompile();
            PostDecompile(mcp_dir, fml_dir);

            if (!FileController.Exists(src_dir))
                return;

            CleanupSource(src_dir);
            MergeClientServer(mcp_dir);
            UpdateMd5Side(mcp_dir);
        }

        // Recompile CLIENT and SERVER
        private static void UpdateMd5Side(string mcp_dir)
        {
            McpCommands.Recompile();
        }

        private static void MergeClientServer(string mcp_dir)
        {
            string client = Path.Combine(mcp_dir, "src", "minecraft");
            string server = Path.Combine(mcp_dir, "src", "minecraft_server");
            string shared = Path.Combine(mcp_dir, "src", "common");

            FileController.CreateDirectory(shared);
            if (!FileController.Exists(client) || !FileController.Exists(server))
                return;

            var files = FileController.GetLists(client, isSearchAllDirectories: true);
            foreach (var cur_file in files)
            {
                string _dn = Path.GetDirectoryName(cur_file).Replace(client, "");
                string _fn = Path.GetFileName(cur_file);

                string f_client = Path.Combine(client, _dn, _fn);
                string f_server = Path.Combine(server, _dn, _fn);
                string f_shared = Path.Combine(shared, _dn, _fn);
                if (!FileController.Exists(f_client) || !FileController.Exists(f_server))
                    continue;

                if (FileController.GetHashValue(f_client, FileController.HashType.MD5) !=
                    FileController.GetHashValue(f_server, FileController.HashType.MD5))
                    continue;

                string new_dir = Path.Combine(shared, _dn);
                FileController.CreateDirectory(new_dir);

                FileController.Rename(f_client, f_shared);
                FileController.Delete(f_server);
            }

            CleanDirs(server);
            CleanDirs(client);
        }

        public static void ApplyFmlPatches(string fml_dir, string mcp_dir, string src_dir)
        {

        }

        public static void FinishSetupFml(string fml_dir, string mcp_dir)
        {

        }

        private static void CleanDirs(string path)
        {
            var lists = FileController.GetLists(path, isDirectory: true);
            foreach (var list in lists)
            {
                if (Directory.Exists(list))
                    CleanDirs(list);
            }
            if (lists.ToArray().Length == 0)
                FileController.Delete(path);
        }
    }
}
