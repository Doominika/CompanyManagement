using System;
using System.Windows.Forms;
using WindowsFormsAppMySql.Database;
using WindowsFormsAppMySql.Database.Entities;
using WindowsFormsAppMySql.Forms;


namespace WindowsFormsAppMySql
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            /*
            var loginForm = new FormLogin();

            if (loginForm.ShowDialog() == DialogResult.OK)
            {
                string login = loginForm.login;
                string password = loginForm.password;
                Employee employee = loginForm.employee;


                if (employee != null)
                {
                    Form formMain = new Forms.f_Main(employee);
                    Application.Run(formMain);
                }
            }
            */
            using(var context = new MyDbConnection())
            {
                Employee employee = context.Employees.Find(1);
                Form formMain = new Forms.f_Main(employee);
                Application.Run(formMain);
            }

            

        }
    }
}

