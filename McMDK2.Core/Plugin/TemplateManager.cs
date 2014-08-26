using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using McMDK2.Core.Data.Project;

namespace McMDK2.Core.Plugin
{
    public class TemplateManager
    {
        private static List<IProjectTemplate> templates = new List<IProjectTemplate>();

        public static IEnumerable<IProjectTemplate> Templates
        {
            get
            {
                return templates.AsReadOnly();
            }
        }

        public static void Register(IProjectTemplate template)
        {
            templates.Add(template);
        }
    }
}
