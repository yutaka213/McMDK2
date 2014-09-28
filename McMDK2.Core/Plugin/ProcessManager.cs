﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using McMDK2.Plugin;

namespace McMDK2.Core.Plugin
{
    public class ProcessManager
    {
        private static List<Setup> setupProcs = new List<Setup>();

        public static Setup GetSetupProcessFromId(string id)
        {
            return setupProcs.Single(w => w.Id == id);
        }

        /// <summary>
        /// プロセスを登録します。
        /// </summary>
        /// <param name="obj"></param>
        public static void RegisterProcess(object obj)
        {
            try
            {
                if (obj is Setup)
                {
                    var setup = (Setup)obj;
                    if (setupProcs.Where(w => w.Id == setup.Id).ToArray().Length != 0)
                    {
                        throw new Exception("既に同じIDを持つプロセスが登録されています。");
                    }
                    setupProcs.Add(setup);
                    Define.GetLogger().Info("Register setup process : ID " + setup.Id);
                    return;
                }
            }
            catch (Exception)
            {
                throw new Exception("プロセスの登録に失敗しました。");
            }
        }
    }
}
