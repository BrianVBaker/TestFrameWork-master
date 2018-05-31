using System;
using System.Collections.Generic;
using System.Text;
using log4net;
using OpenQA.Selenium;
using TestFramework.TestFramework;
using TestFrameWork.TestFramework;

namespace TestFrameWork.TestFramework
{
    public static class TestRun
    {
        
        static ILog log = LogManager.GetLogger(typeof(TestRun));
        
        public static By Str { get; private set; }
        
        public static string TestStep { get; set; }
        public static string Dep { get; set; }
        public static string Type { get; set; }
        public static string Page { get; set; }
        public static string URL { get; set; }
        public static string Load { get; set; }
        public static string Reload { get; set; }
        public static string Elem { get; set; }
        public static string Disp { get; set; }
        public static string Act { get; set; }
        public static string Val { get; set; }
        public static string Exp { get; set; }
        public static int Pause { get; set; }
        public static int WaitParm { get; set; }
        public static string OldTest { get; set; }
        public static IWebDriver Driver { get; set; }
        public static string APIuser { get; set; }
        public static string APIpass { get; set; }
        public static bool Runstep(string steps)            
        {
            
            bool Rtn = true;
            if (steps == null || steps == "") return true;

            if (steps.Contains("<<<"))
                steps = RunTesting.ResolveVariables(steps);
                                    
            string[] v = steps.Split(Convert.ToChar(Config.Delim));

            TestStep = v[0];
            Dep = v[1];
            if (RunTesting.Results.ContainsKey(Dep) && RunTesting.Results[Dep] == false)
            {
                log.Warn("Test Step '" + TestStep + "' skipped as dependent step '" + Dep + "' failed.");
                return true;
            }
            Type = v[2];
            Page = v[3];
            URL = v[4];
            Load = v[5];
            Reload = v[6];
            Elem = v[7];
            if (v[9] != null && v[9] != "" && v[8] != null && v[8] != "")
                Str = SeleniumActions.GetStr(v[8], v[9]);

            Disp = v[10];
            Act = v[11];
            Val = v[12];
            Exp = v[13];
            Pause = Config.Time;
            if (v[14] != null && v[14] != "")
                WaitParm = Convert.ToInt32(v[14]);
            APIuser = v[15];
            APIpass = v[16];
            
            if (v[0].Substring(0, 1) == "!")
            {
                log.Info("Test Step" +
                    " " + v[0].Substring(1) + " : " + v[6] + " skipped!");
                return true;
            }

            
            Rtn = RunTesting.RunStep(TestStep, Dep, Type, Page, URL, Load, Reload, Elem, Str, Disp, Act, Val, Exp, Pause, WaitParm);

            if (TestStep != "Close")
            {

                log.Info(" ***** Running test " + TestStep + " : GUI Page: " + Page + " *****");
                OldTest = TestStep;

            }
            else
            {
                log.Info("**** Test Set completed ****");
            }

            return Rtn;
        }
    }
}
