using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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
        public async static Task<bool> DownloadDeps(string mcp_dir)
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

            // pre_decomple
        }

        public static void ApplyFmlPatches(string fml_dir, string mcp_dir, string src_dir)
        {

        }

        public static void FinishSetupFml(string fml_dir, string mcp_dir)
        {

        }
    }
}
