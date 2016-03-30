using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;
using System.IO;
using System.Web.Security;
using System.Data.SqlClient;
using System.Xml;

namespace EsodaPasswordReset
{
    class Program
    {
        static void Main(string[] args)
        {
            WindowsIdentity wi = WindowsIdentity.GetCurrent();
            WindowsPrincipal wp = new WindowsPrincipal(wi);
            if (wp.IsInRole(WindowsBuiltInRole.Administrator))
            {
                if (File.Exists("EsodaPasswordReset.exe.config"))
                {
                    try
                    {
                        MembershipUser user = Membership.GetUser("admin");
                        user.ChangePassword(user.ResetPassword(), "12345678");
                        user.UnlockUser();

                        if (Membership.ValidateUser("admin", "12345678"))
                        {
                            Console.WriteLine("Hasło admina zresetowane pomyślnie.");
                        }
                        else
                        {
                            Console.WriteLine("Nie udało się zresetować hasła admina.");
                        }
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("Wystąpił problem przy resetowaniu hasła.");
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                    Console.WriteLine(string.Format("Brak pliku konfiguracyjnego."));
                }
            }
            else
            {
                Console.WriteLine("Nie masz uprawnień administratora !!!");
            }

            if (File.Exists("EsodaPasswordReset.exe.config"))
                File.Delete("EsodaPasswordReset.exe.config");
        }
    }
}
