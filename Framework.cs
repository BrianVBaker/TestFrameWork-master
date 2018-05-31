using OpenQA.Selenium;
using System.Threading;
using log4net;
using TestFrameWork.TestFramework;
using System.Net.Http;
using System.Collections.Generic;
using System;

namespace TestFramework.TestFramework
{
    public class RunTesting
    {
        public static ILog log = LogManager.GetLogger(typeof(RunTesting));
        public static Dictionary<string, Dictionary<string, string>> StepValues { get; private set; }
        public static Dictionary<string, bool> Results { get; private set; }

        public static bool Steps(string[] TestSteps, IWebDriver driver)
        {
            
            int ct = 1;
            bool teststatus = true;
            bool overallPass = true;

            StepValues = new Dictionary<string, Dictionary<string, string>>();
            Results = new Dictionary<string, bool>();

            foreach (string TestStep in TestSteps)
            {
                if (ct == 1)
                {
                    ct++;
                    continue;
                }

                teststatus = TestRun.Runstep(TestStep);

                if (!Results.ContainsKey(TestStep))
                    Results.Add(TestStep, teststatus);
                else
                    log.Warn("Test Step '" + TestStep + "' aleady encountered in test set. Possible duplicate Test step");

                if (!teststatus)
                {
                    log.Error("Test Step " + TestRun.TestStep + " has completed with errors");
                    overallPass = false;
                }
                else
                    log.Info("test Step " + TestRun.TestStep + " has completed!");

                if (!overallPass)
                {
                    log.Fatal("Test Set has Errors!!");

                }

            }
            

            return overallPass;
        }
        //string oldtest = null
                               
        public static bool RunStep(string test, string dep, string Type, string Page, string URL, string load, string reload, string Elem, By str, string disp, string act, string val, string Exp, int pause, int waitParm)
        {
            bool Rtn = true;
                      
                  
            int timer = waitParm;
                        
            if (load == "Yes")
                TestRun.Driver.Navigate().GoToUrl(Config.GUIurl + URL);
            if (reload == "Yes")
                TestRun.Driver.Navigate().GoToUrl(TestRun.Driver.Url);

            if (disp == "Yes")
            {
                if (!SeleniumActions.RunTest(str, timer, Elem, TestRun.Driver))
                {
                    log.Error("Error finding element " + Elem);
                    return false;
                }
            }
            switch (act)
            {
                case "None":
                    return true;

                case "Click":
                    Rtn = SeleniumActions.ActionClick(str, TestRun.Driver, Config.Time);
                    break;

                case "SendKeys":
                    Rtn = SeleniumActions.SendKeys(str, TestRun.Driver, Config.Time, val);
                    break;

                case "Submit":
                    Rtn = SeleniumActions.ActionSubmit(str, TestRun.Driver, Config.Time);
                    break;
                
                case "Compare Text":
                    var eText = SeleniumActions.GetText(str, TestRun.Driver, Config.Time);
                    if (eText != Exp)
                    {
                        log.Warn("Element Text : " + eText + " : Not equal to expected : " + Exp);
                    }
                    else
                        log.Info("Text content :'" + eText + "' was expected");
                    Rtn = true;
                    break;

                case "Hover":
                    Rtn = SeleniumActions.ActionHover(str, TestRun.Driver, Config.Time);
                    break;

                case "JavaClick":
                    Rtn = SeleniumActions.JavaClick(str, TestRun.Driver, Config.Time);
                    break;

                case "HttpPost":
                    // Http Post is still under construction
                    List<KeyValuePair<string, string>> pairstr = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>(TestRun.APIuser, TestRun.APIpass),
                        new KeyValuePair<string, string>("Content", val)
                    };
                    Rtn = APIFunctions.HttpPost(Config.BaseUrl + TestRun.URL, pairstr);
                    break;

                case "GetText":
                    string Textval = SeleniumActions.GetText(str, TestRun.Driver, Config.Time);
                    UpdateStepValues(TestRun.TestStep, "TextValue", Textval);
                    break;

                case "SelectList":
                    Rtn = SeleniumActions.SelectFromList(str, TestRun.Driver, Config.Time, val);
                    break;

                case "loadGrid":
                    Rtn = SeleniumActions.LoadFromGrid(str, TestRun.Driver, Config.Time, val);
                    break;

                case "Close":
                    if (Config.Browser != "None")
                    {
                        TestRun.Driver.Quit();
                    }
                    break;
            }
                                    
            if (pause > 0)
                Thread.Sleep(pause);

            return Rtn;

        }
        public static bool UpdateStepValues(string TestStep, string Key, string Value)
        {
            bool success = true;
            if (!StepValues.ContainsKey(TestRun.TestStep))
                StepValues[TestRun.TestStep] = new Dictionary<string, string>();
            else
                success = false;

            if (!StepValues[TestRun.TestStep].ContainsKey("TextValue"))
                StepValues[TestRun.TestStep]["TextValue"] = Value;
            else
                success = false;

            if (success)
                log.Info("Text value '" + StepValues[TestRun.TestStep]["TextValue"] +
                    "' for test step '" + TestRun.TestStep + "', HTML Text Value stored in dictionary");
            else
                log.Warn("Text value '" + StepValues[TestRun.TestStep]["TextValue"] +
                    "' for test step '" + TestRun.TestStep + "', HTML Text already in dictionary");

            return success;

        }
        public static string ResolveVariables(string testline)
        {
            string newline = testline;
            string start = "";
            string end = "";
            string parms = "";
            string[] parm = null;

            while (newline.Contains("<<<"))
            {
                start = newline.Substring(1, newline.IndexOf("<<<") - 1);
                end = newline.Substring(newline.IndexOf(">>>") + 3);
                parms = newline.Substring(newline.IndexOf("<<<") + 3, newline.IndexOf(">>>") - 1);
                parm = parms.Split(',');
                
                if (parm[0] == "$StepValue")
                {
                    newline = start + StepValues[parm[1]][parm[2]] + end;
                }
                if (parm[0] == "$DateTime")
                {
                    newline = start + DateTime.Now.AddDays(Convert.ToInt32(parm[2])).ToString(parm[1]) + end;
                }
                

            }

            return newline;
        }
    }
    

 }




