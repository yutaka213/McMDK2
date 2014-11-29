using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using McMDK2.Py.Fml;
using McMDK2.Py.Mcp;

namespace McMDK2.Py.Forge
{
    /// <summary>
    /// Imitate the forge/install.py's workings.
    /// </summary>
    public class InstallCommands
    {
        private readonly string mcpDir;
        private readonly string srcDir;
        private readonly string forgeDir;
        private readonly string fmlDir;

        public InstallCommands(string mcp_dir)
        {
            this.mcpDir = mcp_dir;
            this.srcDir = Path.Combine(mcpDir, "src");
            this.forgeDir = Path.Combine(mcpDir, "forge");
            this.fmlDir = Path.Combine(forgeDir, "fml");
        }

        public void Install()
        {
            // >> Forge ModLoader Setup Start
            FmlCommands.SetupMcp(this.fmlDir, this.mcpDir);
            FmlCommands.SetupFml(this.fmlDir, this.mcpDir);
            FmlCommands.ApplyFmlPatches(this.fmlDir, this.mcpDir, this.srcDir);
            FmlCommands.FinishSetupFml(this.fmlDir, this.mcpDir);

            // >> Forge ModLoader Setup End
            // >> Minecraft Forge Setup Start
            // >> Applying forge patches
            ForgeCommands.ApplyForgePatches(this.fmlDir, this.mcpDir, this.forgeDir, this.srcDir);
            McpCommands.UpdateNames();
            McpCommands.UpdateMd5();
            // >> Minecraft Forge Setup End
        }
    }
}
