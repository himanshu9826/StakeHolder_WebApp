using StakeHolder.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StakeHolder.Common.Helper
{
    public class LibLogging
    {
        #region Declaration of variables



        #endregion
        public static void WriteErrorToDB(string Source, string Code, Exception ex = null)
        {
            string ErrorFolder = ConfigurationManager.AppSettings["LogPath"].ToString();                 
            if (!Directory.Exists(ErrorFolder))
            {
                Directory.CreateDirectory(ErrorFolder);
            }
            string filePath = @"\" + Convert.ToString(DateTime.Now.Date.Year) + "_" + Convert.ToString(DateTime.Now.Date.Month) + "_" + Convert.ToString(DateTime.Now.Date.Day) + ".txt";
            filePath = ErrorFolder + filePath;            
            if(!File.Exists(filePath))
            {
                FileStream fs1 = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
                fs1.Close();
            }

            try
            {
                int addLog = Convert.ToInt32(ConfigurationManager.AppSettings["AddLog"]);
                //int addLog = 1;
                if (addLog == 1)
                {
                    using (StreamWriter writer = new StreamWriter(filePath, true))
                    {
                        string message = ex != null ? ex.Message : string.Empty;
                        string stackTrace = ex != null ? ex.StackTrace : string.Empty;
                        writer.WriteLine("Methodl: " + Code + " | Source: " + Source + " | Message :" + message + "<br/>" + Environment.NewLine + "StackTrace :" + stackTrace +
                           "" + Environment.NewLine + "Date :" + DateTime.Now.ToString());
                        writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
                    }
                }

            }
            catch (Exception ex1)
            {
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine("Methodl: WriteErrorToDB | Source: LibLogging | Message :" + ex1.Message + "<br/>" + Environment.NewLine + "StackTrace :" + ex1.StackTrace +
                       "" + Environment.NewLine + "Date :" + DateTime.Now.ToString());
                    writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
                }
            }
        }

    }
}
