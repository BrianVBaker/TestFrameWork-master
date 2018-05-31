using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using log4net;
using System.Threading;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using TestFramework.TestFramework;

namespace TestFrameWork.TestFramework
{
    public class SeleniumActions
    {
        public static By Str2 { get; private set; }
        public static By Str3 { get; private set; }

        static ILog log = LogManager.GetLogger(typeof(SeleniumActions));
       
        public static bool RunTest(By str, int wait, string elem, IWebDriver driver)
        {
            GlobalContext.Properties["LogName"] = Config.LogFile;
            bool Rtn = false;

            if (!ObjectDisplayedOK(str, wait, driver))
            {
                log.Error("Error displaying Element " + elem);
                Rtn = false;
            }
            else
            {
                log.Info("Element " + elem + " displayed ok");
                Rtn = true;
            }


            return Rtn;
        }
        public static bool ActionClick(By Str, IWebDriver driver, int timer)
        {
            bool Rtn = false;
            try
            {
                
                int ct = 0;
                while (ct <= timer)
                {
                    if (ObjectIsEnabled(Str, timer, driver))
                    {
                        TestRun.Driver.FindElement(Str).Click();
                        
                        return true;
                    }
                    Thread.Sleep(1);
                    ct++;
                    if (ct > timer)
                    {
                        log.Error("Error clicking " + TestRun.Elem);
                        return Rtn;
                    }
                    
                }
            }
            catch (Exception Exp) { log.Error(Exp); Rtn = false; }
            
            return Rtn;
        }
        public static bool JavaClick(By Str, IWebDriver driver, int timer)
        {
            bool Rtn = false;
            try
            {

                int ct = 0;
                while (ct <= timer)
                {
                    if (ObjectIsEnabled(Str, timer, driver))
                    {
                        IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                        js.ExecuteScript("document.body.style.zoom='100%';");

                        IWebElement btn = driver.FindElement(Str);
                        js.ExecuteScript("arguments[0].click(); ", btn);

                        return true;
                    }
                    Thread.Sleep(1);
                    ct++;
                    if (ct > timer)
                    {
                        log.Error("Error java clicking " + TestRun.Elem);
                        return Rtn;
                    }
                    
                }
            }
            catch (Exception Exp) { log.Error(Exp); Rtn = false; }

            return Rtn;
        }
        public static bool ActionHover(By Str, IWebDriver driver, int timer)
        {
            bool Rtn = false;
            try
            {

                int ct = 0;
                while (ct <= timer)
                {
                    if (ObjectIsEnabled(Str, timer, driver))
                    {
                        Actions builder = new Actions(driver);
                        IWebElement element = driver.FindElement(Str);
                        builder.MoveToElement(element).Perform();
                        return true;
                    }
                    Thread.Sleep(1);
                    ct++;
                    if (ct > timer)
                    {
                        log.Error("Error Hovering over " + TestRun.Elem);
                        return Rtn;
                    }
                    
                }
            }
            catch (Exception Exp) { log.Error(Exp); Rtn = false; }

            return Rtn;
        }
        public static bool ActionSubmit(By Str, IWebDriver driver, int timer)
        {
            bool Rtn = false;
            try
            {

                int ct = 0;
                while (ct <= timer)
                {
                    if (ObjectIsEnabled(Str, timer, driver))
                    {
                        driver.FindElement(Str).Submit();
                        Rtn = true;
                        return Rtn;
                    }
                    
                    Thread.Sleep(1);
                    ct++;
                    if (ct > timer)
                    {
                        log.Error("Error Submitting Form " + TestRun.Elem);
                        return Rtn;
                    }
                }
            }

            catch (Exception Exp) { log.Error(Exp); Rtn = false; }

            return Rtn;
        }

        public static string GetText(By Str, IWebDriver driver, int timer)
        {
            string Rtn = "";
            try
            {

                int ct = 0;
                while (ct <= timer)
                {
                    if (ObjectIsEnabled(Str, timer, driver))
                    {
                        Rtn = driver.FindElement(Str).Text;
                        return Rtn;
                    }
                    Thread.Sleep(1);
                    ct++;
                    if (ct > timer)
                    {
                        log.Error("Error returning text property from HTML");
                        return null;
                    }
                }
                
            }
            catch (Exception Exp) { log.Error(Exp); Rtn = ""; }

            return Rtn;
        }
        public static bool SelectFromList(By Str, IWebDriver driver, int timer, string val)
        {
            bool Rtn = false;
            try
            {
                bool Notselected = false;
                int ct = 0;
                string itemText = "";

                while (ct <= timer && !Notselected)
                {
                    if (ObjectIsEnabled(Str, timer, driver))

                    {
                        IWebElement items = driver.FindElement(Str);
                        string[] vals = val.Split(':');
                        vals[0] = vals[0].Trim('"');
                        vals[0] = vals[0].Trim().Trim('"');
                        vals[2] = vals[2].Trim('"');
                        vals[2] = vals[2].Trim().Trim('"');

                        int ItemCt = items.FindElements(By.TagName(vals[0])).Count;
                        
                        if (vals[1] != "Text" && ItemCt < Convert.ToInt32(vals[2]) + 1)
                        {
                            log.Error("selection value " + Convert.ToInt32(vals[2]) + "  +1 greater than number in list " + ItemCt);
                            return false;
                        }
                        
                        foreach (IWebElement item in items.FindElements(By.TagName(vals[0])))
                        {
                            itemText = item.Text;
                            if (vals[1] == "Value" && ct == Convert.ToInt32(vals[2]))
                            {
                                log.Info(ItemCt + " items in list.");
                                item.Click();
                                log.Info("Item " + vals[2] + " selected.");
                                Notselected = true;
                                Rtn = true;
                                break;
                            }
                            if (vals[1] == "Text" && item.Text.Contains(vals[2]))
                            {
                                log.Info(ItemCt + " items in list.");
                                item.Click();
                                log.Info("Item containing '" + vals[2] + "' selected.");
                                Notselected = true;
                                Rtn = true;
                                break;
                            }
                            
                        }
                                            
                                             
                    }
                    Thread.Sleep(1);
                    ct++;
                    if (ct > timer)
                    {
                        log.Error("Error selecting from drop down list");
                        return false;
                    }
                }
                RunTesting.UpdateStepValues(TestRun.TestStep, "TextValue", itemText);
            }
            catch (Exception Exp) { log.Error(Exp); Rtn = false; }

            return Rtn;
        }

        public static bool LoadFromGrid(By Str, IWebDriver driver, int timer, string val)
        {
            bool Rtn = false;
            try
            {

                int ct = 0;
                while (ct <= timer)
                {
                    if (ObjectIsEnabled(Str, timer, driver))
                    {
                        IWebElement Tbl = driver.FindElement(Str);
                        string[] vals = val.Split('¦');
                        Str2 = GetStr(vals[0], vals[1]);
                                                
                        if (vals[2] != null && vals[3] != null)
                            Str3 = GetStr(vals[2], vals[3]);

                        IList<IWebElement> tableRow = Tbl.FindElements(Str2);
                        IList<IWebElement> rowTD;
                        int colct = 0;

                        foreach (var Col in tableRow)
                        {
                            rowTD = Col.FindElements(Str3);

                            foreach (var col in rowTD)
                            {
                                colct++;
                                if (RunTesting.UpdateStepValues(TestRun.TestStep, colct.ToString(), col.Text))
                                {
                                    log.Info("Grid Data for " + TestRun.TestStep + " : " + colct + " : " + col.Text + " added to dictionary");
                                                   
                                }
                            }
                            colct = 0;
                        }

                        return true;
                    }
                    Thread.Sleep(1);
                    ct++;
                    if (ct > timer)
                    {
                        log.Error("Error selecting from drop down list");
                        return false;
                    }
                }

            }
            catch (Exception Exp) { log.Error(Exp); Rtn = false; }

            return Rtn;
        }
        public static bool SendKeys(By Str, IWebDriver driver, int timer, string val)
        {
            
            int ct = 0;
            while (ct <= timer)
            {
                try
                {
                    driver.FindElement(Str).SendKeys(val);
                    return true;
                }
                catch (Exception) { }

                if (ct > timer)
                {
                    log.Error("Error sending keys to object " + Str);
                    return false;
                }
                Thread.Sleep(1);
                ct++;
            }
            return true;
        }
          
                
        public static bool ObjectDisplayedOK(By Str, int wait, IWebDriver driver)
        {
            bool Rtn = false;
            int secs = 0;
            string ExceptionLog = "";
            
            while (secs <= wait)
            {
                    try
                    {
                        // var elem = wait.Until(ExpectedConditions.ElementToBeClickable(Str));
                        if (driver.FindElement(Str).Displayed && driver.FindElement(Str).Enabled)
                        {
                            Rtn = true;
                            return Rtn;
                        }
                            
                    }
                        
                    catch (Exception exp)
                    {
                        ExceptionLog = exp.ToString();
                        continue;
                    }
                Thread.Sleep(1);
                secs++;
                   
            }
            
            if (secs > wait)
            {
                log.Error("Timeout for object : " + Str + "  : Timer value = " + wait);
                log.Debug(ExceptionLog);
               
            }
            return Rtn;               
        }
        public static IWebDriver GetDriver(string Browser)
        {
            IWebDriver driver = null;
            if (Browser != null && Browser != "")
            {
                log.Info("*** Choosing Browser : " + Browser + " ***");
                switch (Browser)
                {
                    case "Firefox":
                        driver = new FirefoxDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
                        //driver.Manage().Window.Maximize();
                        break;
                    case "Chrome":
                        driver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
                        driver.Manage().Window.Maximize();
                        break;
                    case "IE":
                        driver = new InternetExplorerDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
                        driver.Manage().Window.Maximize();
                        break;
                    case "Edge":
                        // driver = new EdgeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));  // Only valid under Windows 10
                        // driver.Manage().Window.Maximize();
                        break;
                    case "Phantom":
                    //    driver = new PhantomJSDriver();   No longer supported by Selenium
                        break;
                    case "Safari":
                        //    driver = new SafariDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));  Download Safari for Windows needed 
                        // driver.Manage().Window.Maximize();
                        break;

                }
            }

            //driver.Manage().Window.Maximize();

            return driver;
        }

        public static By GetStr(string Type, string Id)
        {
            By Str = null;

            switch (Type)
            {
                case "CssSelector":
                    Str = By.CssSelector(Id);
                    break;
                case "Id":
                    Str = By.Id(Id);
                    break;
                case "Name":
                    Str = By.Name(Id);
                    break;
                case "TagName":
                    Str = By.TagName(Id);
                    break;
                case "Xpath":
                    Str = By.XPath(Id);
                    break;
                case "LinkText":
                    Str = By.LinkText(Id);
                    break;
                case "ClassName":
                    Str = By.ClassName(Id);
                    break;
                case "PartialLinkText":
                    Str = By.PartialLinkText(Id);
                    break;
            }
         
            return Str;
        }
        public static bool ObjectIsEnabled(By Str, int wait, IWebDriver driver)
        {
            bool Rtn = false;
            int secs = 0;
            try
            {

                while (secs <= wait)
                {
                    // var elem = wait.Until(ExpectedConditions.ElementToBeClickable(Str));
                    if (driver.FindElement(Str).Enabled && driver.FindElement(Str).Displayed)
                    {
                        return true;
                    }
                    if (secs > wait)
                    {
                        log.Error("Timeout for object : " + Str + "  : Timer value = " + wait);
                        return Rtn;
                    }
                    Thread.Sleep(1);
                    secs++;
                }
            }



            catch (Exception Exp)
            {
                log.Debug(Exp);
                Rtn = false;
            }

            return Rtn;

        }
    }

}
