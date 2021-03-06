﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DxLibDLL;


namespace Battle_AI
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());

            Form1 form = new Form1();
            form.Show();
            while (form.Created)
            {
                //Application.Runしないで自分でループを作る
                form.MainLoop();
                Application.DoEvents();
            }
        }
    }
}
