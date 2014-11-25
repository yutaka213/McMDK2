using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

#pragma warning disable 1573

namespace McMDK2.Core.Utils
{
    /// <summary>
    /// ApplyDiffを扱う機能を提供します。
    /// </summary>
    public static class Patcher
    {
        /// <param name="command">command must contain {0}.</param>
        public static void ApplyPatch(string file, string mcp_dir, string command = "-p1 -u -i \"{0}\"", string workingDir = "")
        {
            if (!FileController.Exists(file))
            {
                return;
            }
            FileController.Copy(file, Path.Combine(Define.CacheDirectory, "temp.patch"));

            Define.GetLogger().Info(String.Format("Applying patch from {0}.", file));

            var process = new Process();
            process.StartInfo.FileName = Path.Combine(mcp_dir, "runtime", "bin", "applydiff.exe");
            process.StartInfo.Arguments = String.Format(command, Path.Combine(Define.CacheDirectory, "temp.patch"));
            process.StartInfo.WorkingDirectory = workingDir != "" ? mcp_dir : workingDir;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.OutputDataReceived += (a, b) => Define.GetLogger().Info(b.Data);
            process.ErrorDataReceived += (a, b) => Define.GetLogger().Error(b.Data);
            process.Start();
            process.BeginOutputReadLine();
            process.BeginErrorReadLine();
            process.WaitForExit();
        }

        /// <summary>
        /// 指定したディレクトリ内の全ての*.patchに対して、処理を実行します。
        /// </summary>
        public static void ApplyPatches(string dir, string works)
        {
            if (!FileController.Exists(dir))
            {
                return;
            }
            List<string> files = Directory.GetFiles(dir, "*.patch", SearchOption.AllDirectories).ToList();
            foreach (var file in files)
            {
                var sr = new StreamReader(file);
                string line, diff = "";
                while ((line = sr.ReadLine()) != null)
                {
                    diff += line + Environment.NewLine;
                }
                sr.Close();
                sr.Dispose();

                var sw = new StreamWriter(Path.Combine(works, "temp.patch"));
                sw.WriteLine(diff);
                sw.Close();
                sw.Dispose();

                string args = "-p1 -u -i \"{0}\"";
                Define.GetLogger().Info("Apply patch from " + file);
                Define.GetLogger().Debug("Arguments - " + String.Format(args, Path.Combine(works, "temp.patch")));

                var process = new Process();
                process.StartInfo.FileName = Path.Combine(works, "runtime", "bin", "applydiff.exe");
                process.StartInfo.Arguments = String.Format(args, Path.Combine(works, "temp.patch"));
                process.StartInfo.WorkingDirectory = works;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.OutputDataReceived += (sender, eventArgs) => Define.GetLogger().Info(eventArgs.Data);
                process.ErrorDataReceived += (sender, eventArgs) => Define.GetLogger().Error(eventArgs.Data);
                process.Start();
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();
                process.WaitForExit();
            }
        }
    }
}
