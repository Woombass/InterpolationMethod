using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace InterpolationMethod
{
    static class Program

    {
        internal static bool admin { get; set; }
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWIndow());

             admin = false;
        }

        internal static void ChangePerm(bool newPerm)
        {
            admin = newPerm;
        }
    }
}
