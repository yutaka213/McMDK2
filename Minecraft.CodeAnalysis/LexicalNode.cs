using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minecraft.CodeAnalysis
{
    public class LexicalNode
    {
        public TokenKind TokenKind { private set; get; }

        public string Value { private set; get; }

        public LexicalNode(TokenKind kind, string value)
        {
            this.TokenKind = kind;
            this.Value = value;
        }

    }
}
