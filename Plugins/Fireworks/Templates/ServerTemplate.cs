using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using McMDK2.Plugin;

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
            get { return Id + ";Fireworks.Templates.ServerTemplate.zip"; }
        }
    }
}
