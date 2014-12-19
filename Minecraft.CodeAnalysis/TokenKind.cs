using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minecraft.CodeAnalysis
{
    public enum TokenKind
    {
        /// <summary>
        /// Comments
        /// </summary>
        Comment,

        /// <summary>
        /// Letters <para/>
        /// e.g. String, BufferedReader, br
        /// </summary>
        Letter,

        /// <summary>
        /// Keywords <para/>
        /// e.g. new, int, abstract, class
        /// </summary>
        Keyword,

        /// <summary>
        /// Decimal Integer<para />
        /// e.g. 12345, 67890
        /// </summary>
        DecimalInteger,

        /// <summary>
        /// Hex Integer<para />
        /// e.g. 0x1234, 0x5678, 0xAC, 0x8F9C_129F
        /// </summary>
        HexInteger,

        /// <summary>
        /// Octal Integer<para />
        /// e.g. 01234, 01, 0111_1234_5676
        /// </summary>
        OctalInteger,

        /// <summary>
        /// Binary Integer<para />
        /// e.g. 0b00001111, 0b1001_1000_1111
        /// </summary>
        BinaryInteger,

        /// <summary>
        /// Decimal Floating Point<para />
        /// e.g. 1e1[df], 2.[df], .3[df], 0[df], 3.14[df], 6.022137e+23[df], 1e-9d
        /// </summary>
        DecimalFloatingPoint,

        /// <summary>
        /// Hexadecimal Floating Point<para/>
        /// 0x1234.5678
        /// </summary>
        HexadecimalFloatingPoint,

        /// <summary>
        /// true or false
        /// </summary>
        Boolean,

        /// <summary>
        /// 'h', '\u03a9'
        /// </summary>
        Character,

        /// <summary>
        /// "hogz", "\""
        /// </summary>
        String,

        /// <summary>
        /// null
        /// </summary>
        Null,

        /// <summary>
        /// Separators<para />
        /// (, ), {, }, [, ], ;, ,, ., ..., @, ::
        /// </summary>
        Separators,

        /// <summary>
        /// Operators<para />
        /// =, &#60;, &#62;, !, ~, ?, :, -&#60;, ==, &#60;=, &#62;=, !=, &#38;&#38;, ||, ++, --<para/>
        /// +, -, *, /, &#38;, |, ^, %, &#62;&#62;, &#60;&#60;, &#60;&#60;&#60;, +=, -=, *=, /=, &#38;=<para/>
        /// |=, ^=, %=, &#62;&#62;=, &#60;&#60;=, &#60;&#60;&#60;=
        /// </summary>
        Operators
    }

}
