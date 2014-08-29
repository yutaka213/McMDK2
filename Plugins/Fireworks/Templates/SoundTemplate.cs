using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using McMDK2.Plugin;

namespace Fireworks.Templates
{
    /// <summary>
    /// McMDK2 で、音声ファイルを用いたプロジェクトのテンプレート
    /// </summary>
    public class SoundTemplate : ITemplate
    {
        public string Name
        {
            get { return "音声を含むプロジェクト"; }
        }

        public string Author
        {
            get { return "tuyapin"; }
        }

        public string Id
        {
            get { return "29E1D308-F120-4D12-8F00-341EAD92BC82"; }
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
            get { return "標準的なModのプロジェクトに加え、音声ファイルを簡単に追加できるプロジェクトです。"; }
        }

        public string TemplateFile
        {
            get { return Id + ";Fireworks.Templates.SoundTemplate.zip"; }
        }
    }
}
