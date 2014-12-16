using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using McMDK2.Plugin;
using McMDK2.Plugin.Process;

namespace Fireworks.Templates
{
    public class ServerTemplate : ITemplate
    {
        public string Name
        {
            get { return "スタンダードプロジェクト(サーバー)"; }
        }

        public string Id
        {
            get { return "D3200BAF-11AF-42EE-847B-0EEF7011FCDA"; }
        }

        public string Dependents
        {
            get { return null; }
        }

        public string IconPath
        {
            get { return Id + ";Fireworks.Resources.Contract_32xLG.png"; }
        }

        public string Description
        {
            get { return "標準的なサーバー向けのModのプロジェクトです。サーバー専用Modはこのプロジェクトを使用してください。"; }
        }

        public string TemplateFile
        {
            get { return "assembly;Fireworks.Templates.ServerTemplate.zip"; }
        }

        public string ProjectType
        {
            get { return "Mcp"; }
        }

        public void PreInitialization(PreInitializationArgs args)
        {
        }

        public void Initialization(InitializationArgs args)
        {
        }

        public void PostInitialization(PostInitializationArgs args)
        {
        }
    }
}
