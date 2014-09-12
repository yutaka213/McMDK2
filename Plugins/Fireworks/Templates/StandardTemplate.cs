using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using McMDK2.Plugin;

namespace Fireworks.Templates
{
    /// <summary>
    /// McMDK2 の標準的なプロジェクトのテンプレート
    /// </summary>
    public class StandardTemplate : ITemplate
    {
        public string Name
        {
            get { return "スタンダードプロジェクト(ユニバーサル)"; }
        }

        public string Id
        {
            get { return "43ECD3FD-7E29-4968-AF55-4C5ED437E7B3"; }
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
            get { return "標準的なModのプロジェクトです。通常はこのプロジェクトを使用してください。"; }
        }

        public string TemplateFile
        {
            get { return Id + ";Fireworks.Templates.StandardTemplate.zip"; }
        }
    }
}
