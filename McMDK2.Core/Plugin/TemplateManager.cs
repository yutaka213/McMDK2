﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using McMDK2.Plugin;
using McMDK2.Core;

namespace McMDK2.Core.Plugin
{
    public class TemplateManager
    {
        private static List<ITemplate> templates = new List<ITemplate>();

        public static IEnumerable<ITemplate> Templates
        {
            get
            {
                return templates.AsReadOnly();
            }
        }

        public static void Register(ITemplate template)
        {
            Define.GetLogger().Info(String.Format("Register Template : {0}({1}).", template.Name, template.Id));
            templates.Add(template);
        }
    }
}