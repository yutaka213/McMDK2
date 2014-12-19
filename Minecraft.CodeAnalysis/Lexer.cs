using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minecraft.CodeAnalysis
{
    public class Lexer
    {
        private readonly string _baseDirectory;
        private readonly string[] _additionalReferencePaths;

        public Dictionary<string, LexicalTree> Trees { private set; get; }

        /// <summary>
        /// Java root directry
        /// </summary>
        public Lexer(string baseDirectory, string[] additionalReferencePaths)
        {
            this._baseDirectory = baseDirectory;
            this._additionalReferencePaths = additionalReferencePaths;
            this.Trees = new Dictionary<string, LexicalTree>();
        }

        /// <summary>
        /// Parse Java Sources
        /// </summary>
        public void Run()
        {
            var classLists = Directory.GetFiles(this._baseDirectory, "*.java", SearchOption.AllDirectories);
            foreach (var _class in classLists)
            {
                var ps = new LexicalTree(_class);
                ps.Run();
                this.Trees.Add(_class.Replace(this._baseDirectory + "\\", "").Replace("\\", "."), ps);
            }
        }
    }

}
