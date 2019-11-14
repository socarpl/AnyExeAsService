using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace AnyExeAsService
{
    public partial class Service1 : ServiceBase
    {
        Process p = null;
        ILog log = LogManager.GetLogger("applog");
        public Service1()
        {
            InitializeComponent();
            XmlConfigurator.Configure();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.Arguments = Service1.ExeParameters;
                psi.FileName = Service1.ExeToRun;
                if (String.IsNullOrEmpty(Service1.ExeWorkDir))
                {
                    try
                    {
                        FileInfo fi = new FileInfo(psi.FileName);
                        psi.WorkingDirectory = fi.Directory.FullName;
                    }
                    catch (Exception ex)
                    {
                        log.Error("OnStart_ExeWorkDir: " + ex.Message);
                    }

                }
                else
                    psi.WorkingDirectory = Service1.ExeWorkDir;

                p = new Process();
                p.StartInfo = psi;
                p.Start();
            }
            catch (Exception ex)
            {
                log.Error("OnStart: " + ex.Message);
            }
        }

        protected override void OnStop()
        {
            try
            {
                p.Kill();
            }
            catch(Exception ex)
            {
                log.Error("OnStop: " + ex.Message);
            }
        }

        public static string ExeToRun
        {
            get
            {
                return ConfigurationManager.AppSettings["exeToRun"].ToString();
            }
        }

        public static string ExeParameters
        {
            get
            {
                return ConfigurationManager.AppSettings["exeParameters"].ToString();
            }
        }

        public static string ExeWorkDir
        {
            get
            {
                return ConfigurationManager.AppSettings["exeWorkingDirectory"].ToString();
            }
        }
    }
}
