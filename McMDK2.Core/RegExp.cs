using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace McMDK2.Core
{
    /// <summary>
    /// 正規表現の簡易的な機能を提供します。
    /// </summary>
    public static class RegExp
    {
        /// <summary>
        /// 正規表現パターンにマッチしたもののうち、Group Nameが設定されているもののみを返します。
        /// </summary>
        /// <param name="target">正規表現で検索を行う対象の文字列</param>
        /// <param name="regex">正規表現パターン</param>
        /// <param name="regexOptions">オプション</param>
        public static List<KeyValuePair<string, string>> Do(string target, string regex, RegexOptions regexOptions = RegexOptions.None)
        {
            var reg = new Regex(regex, regexOptions);
            string[] groupnames = reg.GetGroupNames();
            var list = new List<KeyValuePair<string, string>>();
            MatchCollection collection = reg.Matches(target);
            foreach (Match match in collection)
            {
                foreach (string group in groupnames)
                {
                    int i;
                    if (!int.TryParse(group, out i) && !String.IsNullOrEmpty(group))
                    {
                        list.Add(new KeyValuePair<string, string>(group, match.Groups[group].Value));
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 正規表現パターンにマッチしたものを返します。
        /// </summary>
        /// <param name="target">正規表現で検索を行う対象の文字列</param>
        /// <param name="regex">正規表現パターン</param>
        /// <param name="regexOptions">オプション</param>
        public static List<KeyValuePair<string, string>> DoWithNumbers(string target, string regex, RegexOptions regexOptions = RegexOptions.None)
        {
            var reg = new Regex(regex, regexOptions);
            var list = new List<KeyValuePair<string, string>>();
            MatchCollection collection = reg.Matches(target);
            foreach (Match match in collection)
            {
                foreach (var group in match.Groups)
                {
                    list.Add(new KeyValuePair<string, string>(group.ToString(), match.Groups[group.ToString()].Value));
                }
            }
            return list;
        }

        /// <summary>
        /// 正規表現パターンにマッチする物があった場合、trueを返します。
        /// </summary>
        /// <param name="target">正規表現で検索を行う対象の文字列</param>
        /// <param name="regex">正規表現パターン</param>
        /// <param name="regexOptions">オプション</param>
        public static bool Test(string target, string regex, RegexOptions regexOptions = RegexOptions.None)
        {
            var reg = new Regex(regex, regexOptions);
            return reg.IsMatch(target);
        }
    }
}
