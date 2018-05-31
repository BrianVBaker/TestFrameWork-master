using log4net;
using System;
using System.IO;
using TestFramework.TestFramework;
using TestFrameWork.TestFramework;
using System.Windows.Forms;

namespace BatchRunner

{
    class Program
    {
        static void Main(string[] args)
        {
            Config.CSVfile = args[0];
            Config.Browser = args[1];
            Config.Delim = args[2];
            Config.Time = Convert.ToInt32(args[3]);
            Config.GUIurl = args[4];
            Config.BaseUrl = args[5];
            Config.LogFile = args[6];

            //SendKeys.Send("{ESC}");

            GlobalContext.Properties["LogName"] = Config.LogFile;
            ILog log = LogManager.GetLogger(typeof(Program));

            log.Info("Initialising........");

            //RunTesting Testlines = new RunTesting();

            string[] TestSteps;

            using (StreamReader sr = new StreamReader(args[0]))
            {
                string TestFile = sr.ReadToEnd();
                TestSteps = TestFile.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            }

            

            if (Config.Browser != "None")
                TestRun.Driver = SeleniumActions.GetDriver(Config.Browser);

            RunTesting.Steps(TestSteps, TestRun.Driver);

            log.Info("Testing Summary");
            log.Info("===============");
            int passct = 0;
            int failct = 0;
            int totalct = 0;
            foreach (var item in RunTesting.Results.Keys)
            {
                totalct++;
                if (RunTesting.Results[item] == true)
                    passct++;
                else
                    failct++;
            }
            log.Info("Total passes = " + passct);
            log.Info("Total fails  = " + failct);
            log.Info("Total Tests  = " + totalct);
            log.Info("Asta la vista!");


        }
    }
}
