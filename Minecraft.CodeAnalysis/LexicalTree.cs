using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minecraft.CodeAnalysis
{
    public class LexicalTree
    {
        public string FilePath { private set; get; }

        public List<LexicalNode> Nodes { private set; get; }

        public LexicalTree(string file)
        {
            this.FilePath = file;
            this.Nodes = new List<LexicalNode>();
        }

        public void Run()
        {
            var text = new StreamReader(this.FilePath).ReadToEnd();

            int index = 0;

            string[] keywords = {
                "abstract", "assert", "boolean", "break", "byte",
                "case", "catch", "char", "class", "const",
                "continue", "default", "do", "double", "else",
                "enum", "extends", "final", "finally", "float",
                "for", "goto", "if", "implements", "import",
                "insanceof", "int", "interface", "long", "native",
                "new", "package", "private", "protected", "public",
                "return", "short", "static", "strictp", "super",
                "switch", "synchrnized", "this", "throw", "throws",
                "transient", "try", "void", "volatile", "while"
            };

            while (index < text.Length)
            {
                char c = text[index];
                // comments
                int bufIndex;
                if (c == '/')
                {
                    if (text[index + 1] == '/')
                    {
                        bufIndex = text.IndexOf(Environment.NewLine, index + 1, StringComparison.Ordinal) + Environment.NewLine.Length;
                        this.Nodes.Add(new LexicalNode(TokenKind.Comment, text.Substring(index + 2, bufIndex - index - 2)));
                        index = bufIndex;
                        continue;
                    }
                    if (text[index + 1] == '*')
                    {
                        bufIndex = text.IndexOf("*/", index + 1, StringComparison.Ordinal) + 2;
                        this.Nodes.Add(new LexicalNode(TokenKind.Comment, text.Substring(index + 2, bufIndex - index - 4)));
                        string h = text.Substring(index + 2, bufIndex - index - 4);
                        index = bufIndex;
                        continue;
                    }
                }

                // spaces
                if (c == ' ' || c == '\t' || c == '\f')
                {
                    index++;
                    continue;
                }
                // string
                if (c == '"')
                {
                    bufIndex = text.IndexOf("\"", index + 1, StringComparison.Ordinal);
                    while (true)
                    {
                        if (text[bufIndex - 1] == '\\')
                        {
                            int escIndex = bufIndex - 1;
                            int count = 1;
                            while (true)
                            {
                                if (text[escIndex - count] == '\\')
                                    count++;
                                else
                                    break;
                            }
                            if (count % 2 == 1)
                                bufIndex = text.IndexOf("\"", bufIndex + 1, StringComparison.Ordinal);
                            else
                                break;
                        }
                        else
                            break;
                    }
                    this.Nodes.Add(new LexicalNode(TokenKind.String, text.Substring(index + 1, bufIndex - index - 1)));
                    index = bufIndex + 1;
                    continue;
                }

                // character
                if (c == '\'')
                {
                    bufIndex = text.IndexOf("\'", index + 1, StringComparison.Ordinal);
                    while (true)
                    {
                        if (text[bufIndex - 1] == '\\' && text[bufIndex - 2] != '\\')
                            bufIndex = text.IndexOf("\'", bufIndex + 1, StringComparison.Ordinal);
                        else
                            break;
                    }
                    this.Nodes.Add(new LexicalNode(TokenKind.Character, text.Substring(index, bufIndex - index - 1)));
                    index = bufIndex + 1;
                    continue;
                }

                // numeric
                if ('0' <= c && c <= '9')
                {
                    bufIndex = index + 1;
                    string numeric = text.Substring(index, bufIndex - index);
                    int n;
                    while (this.IsDigit(numeric) > 0)
                    {
                        numeric = text.Substring(index, ++bufIndex - index);
                    }
                    n = this.IsDigit(text.Substring(index, --bufIndex - index - 1));
                    switch (n)
                    {
                        case 10:
                            this.Nodes.Add(new LexicalNode(TokenKind.DecimalInteger, text.Substring(index, bufIndex - index - 1)));
                            break;

                        case 16:
                            this.Nodes.Add(new LexicalNode(TokenKind.HexInteger, text.Substring(index, bufIndex - index - 1)));
                            break;

                        case 8:
                            this.Nodes.Add(new LexicalNode(TokenKind.OctalInteger, text.Substring(index, bufIndex - index - 1)));
                            break;

                        case 2:
                            this.Nodes.Add(new LexicalNode(TokenKind.BinaryInteger, text.Substring(index, bufIndex - index - 1)));
                            break;

                        case 64:
                            this.Nodes.Add(new LexicalNode(TokenKind.DecimalFloatingPoint, text.Substring(index, bufIndex - index - 1)));
                            break;

                        case 128:
                            this.Nodes.Add(new LexicalNode(TokenKind.HexadecimalFloatingPoint, text.Substring(index, bufIndex - index - 1)));
                            break;
                    }
                    index = bufIndex;
                    continue;
                }

                // letter, keyword, boolean, null
                if (Char.IsLetter(c) && c != '\'' && c != '"')
                {
                    var sb = new StringBuilder();
                    bufIndex = 0;
                    for (int i = 0; i < text.Length && (Char.IsLetter(text[index + i]) || Char.IsDigit(text[index + i]) || text[index + i] == '_'); i++, bufIndex++)
                    {
                        sb.Append(text[index + i]);
                    }
                    string l = sb.ToString();
                    if (l == "true" || l == "false")
                        this.Nodes.Add(new LexicalNode(TokenKind.Boolean, l));
                    else if (l == "null")
                        this.Nodes.Add(new LexicalNode(TokenKind.Null, l));
                    else if (keywords.Contains(l))
                        this.Nodes.Add(new LexicalNode(TokenKind.Keyword, l));
                    else
                        this.Nodes.Add(new LexicalNode(TokenKind.Letter, l));
                    index += bufIndex;
                    continue;
                }

                // operator, separators
                if ((Char.IsSymbol(c) || Char.IsPunctuation(c) || Char.IsSeparator(c)) && c != '\'' && c != '"')
                {
                    bufIndex = index;
                    char nc = index + 1 < text.Length ? text[index + 1] : ' ';
                    char nnc = index + 2 < text.Length ? text[index + 2] : ' ';
                    char nnnc = index + 3 < text.Length ? text[index + 3] : ' ';

                    // separators
                    if (c == '(' || c == ')' || c == '{' || c == '}' || c == ';' || c == '@' || c == ',')
                        bufIndex++;
                    else if (c == '.')
                    {
                        if (nc == '.' && nnc == '.')
                            bufIndex += 2;
                        bufIndex++;
                    }
                    else if (c == ':' && nc == ':')
                        bufIndex += 2;
                    if (index != bufIndex)
                    {
                        this.Nodes.Add(new LexicalNode(TokenKind.Separators, text.Substring(index, bufIndex - index)));
                        index = bufIndex;
                        continue;
                    }

                    bufIndex = index;
                    // operators
                    #region = Operator

                    if (c == '=')
                    {
                        // ==
                        if (nc == '=')
                            bufIndex++;
                        bufIndex++;
                    }

                    #endregion

                    #region > Operatpr

                    if (c == '>')
                    {
                        // >=
                        if (nc == '=')
                            bufIndex++;

                        // >>
                        if (nc == '>')
                        {
                            // >>>
                            if (nnc == '>')
                            {
                                // >>>=
                                if (nnnc == '=')
                                    bufIndex++;
                                bufIndex++;
                            }
                            // >>=
                            if (nnc == '=')
                                bufIndex++;
                            bufIndex++;
                        }
                        bufIndex++;
                    }

                    #endregion

                    #region < Operator

                    if (c == '<')
                    {
                        // <=
                        if (nc == '=')
                            bufIndex++;

                        // <<
                        if (nc == '<')
                        {
                            // <<=
                            if (nnc == '=')
                                bufIndex++;
                            bufIndex++;
                        }
                        bufIndex++;
                    }

                    #endregion

                    #region ! Operator

                    if (c == '!')
                    {
                        // !=
                        if (nc == '=')
                            bufIndex++;
                        bufIndex++;
                    }

                    #endregion

                    #region : Operator

                    if (c == ':')
                        bufIndex++;

                    #endregion

                    #region - Operator

                    if (c == '-')
                    {
                        // --, ->, -=
                        if (nc == '-' || nc == '>' || nc == '=')
                            bufIndex++;
                        bufIndex++;
                    }

                    #endregion

                    #region & Operator

                    if (c == '&')
                    {
                        if (nc == '&' || nc == '=')
                            bufIndex++;
                        bufIndex++;
                    }

                    #endregion

                    #region | Operator

                    if (c == '|')
                    {
                        if (nc == '|' || nc == '=')
                            bufIndex++;
                        bufIndex++;
                    }

                    #endregion

                    #region + Operator

                    if (c == '+')
                    {
                        if (nc == '+' || nc == '=')
                            bufIndex++;
                        bufIndex++;
                    }

                    #endregion

                    #region * Operator

                    if (c == '*')
                    {
                        if (nc == '=')
                            bufIndex++;
                        bufIndex++;
                    }

                    #endregion

                    #region / Operator

                    if (c == '/')
                    {
                        if (nc == '=')
                            bufIndex++;
                        bufIndex++;
                    }

                    #endregion

                    #region ^ Operator

                    if (c == '^')
                    {
                        if (nc == '=')
                            bufIndex++;
                        bufIndex++;
                    }

                    #endregion

                    #region % Operator

                    if (c == '%')
                    {
                        if (nc == '=')
                            bufIndex++;
                        bufIndex++;
                    }

                    #endregion

                    if (bufIndex != index)
                    {
                        this.Nodes.Add(new LexicalNode(TokenKind.Operators, text.Substring(index, bufIndex - index)));
                        index = bufIndex;
                        continue;
                    }
                }
                index++;
            }

        }

        private int IsDigit(string str)
        {
            if (str.Contains(" "))
                return 0;
            str = str.Replace("L", "").Replace("l", "").Replace("F", "").Replace("f", "");
            int i;
            // Integer
            if (int.TryParse(str, out i))
                return 10;
            string orig = str;

            // Hex Integer
            if (orig.StartsWith("0x"))
            {
                if (str.Contains("_"))
                    str = str.Replace("_", "");
                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                if (this.ToInt32(str, 16))
                    return 16;
            }

            // Binary Integer
            if (orig.StartsWith("0b"))
            {
                if (str.Contains("_"))
                    str = orig.Replace("_", "");
                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                if (this.ToInt32(str, 2))
                    return 2;
            }

            // Octa Integer
            if (orig.StartsWith("0"))
            {
                if (str.Contains("_"))
                    str = orig.Replace("_", "");
                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                if (this.ToInt32(str, 8))
                    return 8;
            }

            // Decimal Floating Point
            if (orig.Contains(".") || orig.EndsWith("f", StringComparison.OrdinalIgnoreCase) || orig.EndsWith("d", StringComparison.OrdinalIgnoreCase))
            {
                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                if (this.ToDouble(str))
                    return 64;
            }

            // Hex Floating Point
            if (orig.Contains(".") && orig.StartsWith("0x"))
            {
                string[] nums = orig.Replace("_", "").Split('.');
                if (this.ToInt32(nums[0], 16) && this.ToInt32(nums[1], 16))
                    return 128;
            }

            return 0;
        }

        private bool ToInt32(string str, int i)
        {
            try
            {
                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                Convert.ToInt32(str, i);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool ToDouble(string value)
        {
            try
            {
                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                Convert.ToDouble(value);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

}
