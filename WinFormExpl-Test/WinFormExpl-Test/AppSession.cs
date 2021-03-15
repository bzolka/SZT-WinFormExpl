using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

// TODO-BZ: kódkomment ellenőrzés, de azt egy Roslyn-os ellenőrzőben kellene

namespace WinFormExpl_Test
{
    public class AppSession
    {
        protected static bool FastAndUnsafe = false;
        protected const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";
        protected static TimeSpan DefaultSessionImplicitWaitSec ;
        // This setting can be overridden by the runsettings file
        //private static string AppId = @"d:\Tanszek\BZ\SZT-WinForms-Test\Feladatok\WindowsFormsApp\bin\Debug\netcoreapp3.1\ProjectFileForTest.exe";
        private static string AppId = @"..\..\..\..\SampleSolution\WinFormExpl\bin\Debug\netcoreapp3.1\ProjectFileForTest.exe";
        // private static string AppId = @"..\Feladatok\WindowsFormsApp\bin\Debug\netcoreapp3.1\publish\ProjectFileForTest.exe";
        // This setting can be overridden by the runsettings file
        protected static string RootPath = @"..\..\..\TestFiles";
        //protected static string rootPath = Path.GetFullPath(@".\TestFiles");

        protected const string fileA = "a.txt";
        protected const string fileB = "b.txt";
        protected const string fileC = "c.txt";
        protected const string fileD = "d.txt";
        protected const string FolderA = "FolderA";
        protected const string FolderParent = "..";
        protected const string fileASize = "13";
        protected const string fileBSize = "14";
        protected const int menuItemFileWidthIn96Dpi = 37; // BZ: Can be OS dependent ???!!!

        protected static WindowsDriver<WindowsElement> session;
        //protected static WindowsElement editBox;

        public static void Setup(TestContext context)
        {
            DefaultSessionImplicitWaitSec = FastAndUnsafe ? TimeSpan.FromSeconds(0.01) : TimeSpan.FromSeconds(0.1);

            // Launch a new instance of Notepad application
            if (session == null)
            {
                //// Get AppId from environment variable (highest prio)
                //string appId = Environment.GetEnvironmentVariable("AppId");
                //// Get AppId from .runsettings
                //if (appId == null)
                //    appId = (string)context.Properties["AppId"];
                //// If not provided as env var nor as in .runsettings, use default
                //if (appId != null)
                //    AppId = appId;
                //AppId = Path.GetFullPath(AppId); // WindowsDriver apparently cannot work with relative paths (sounds reasonable if path is sent to WinAPpDriver server)
                //context.WriteLine("AppId: " + AppId);


                //// Get RootPath from environment variable (highest prio)
                //string rootPath = Environment.GetEnvironmentVariable("RootPath");
                //// Get RootPath from .runsettings
                //if (rootPath == null)
                //    rootPath = (string)context.Properties["RootPath"];
                //// If not provided as env var nor as in .runsettings, use default
                //if (rootPath != null)
                //    RootPath = rootPath;
                //RootPath = Path.GetFullPath(RootPath);
                //context.WriteLine("rootPath: " + RootPath);

                applySetting<string>(context, "AppId", setting => AppId = setting);
                AppId = Path.GetFullPath(AppId); // WindowsDriver apparently cannot work with relative paths (sounds reasonable if path is sent to WinAPpDriver server)
                context.WriteLine("AppId: " + AppId);
                applySetting<string>(context, "RootPath", setting => RootPath = setting);
                RootPath = Path.GetFullPath(RootPath); // Make this a full path, student solutions may not be able to work with relative paths
                context.WriteLine("RootPath: " + RootPath);
                applySetting<bool>(context, "FastAndUnSafe", setting => FastAndUnsafe = setting);
                context.WriteLine("FastAndUnSafe: " + FastAndUnsafe);

                // Create a new session to launch Notepad application
                DesiredCapabilities appCapabilities = new DesiredCapabilities();
                appCapabilities.SetCapability("app", AppId);
                appCapabilities.SetCapability("ms:experimental-webdriver", true);
                session = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appCapabilities);
                Assert.IsNotNull(session);
                Assert.IsNotNull(session.SessionId);

                // Verify that App is started
                // Assert.AreEqual("MiniExplorer", session.Title, "Nem található a MiniExplorer fejlécű ablak");
                Assert.IsTrue(session.Title.StartsWith("MiniExplorer"), "Nem található a MiniExplorer szöveggel kezdődő fejlécű ablak");

                // Set implicit timeout to xxx seconds to make element search to retry every 500 ms for at most three times
                session.Manage().Timeouts().ImplicitWait = DefaultSessionImplicitWaitSec;
            }
        }

        public static void TearDown()
        {
            // Close the application and delete the session
            if (session != null)
            {
                // https://github.com/Microsoft/WinAppDriver/issues/159

                session.Close();

                try
                {
                    // Dismiss message box if displayed
                    session.FindElementByName("Ok").Click();
                }
                catch { }

                try
                {
                    // Dismiss assert box if displayed
                    session.FindElementByName("Quit").Click();
                }
                catch { }

                session.Quit();
                session = null;
            }
        }

        [TestInitialize]
        public void TestInitialize()
        {
        }

        public static void Wait(int millisec)
        {
            Thread.Sleep(FastAndUnsafe ? 0 : millisec);
        }

        protected  WindowsElement OpenDialog()
        {
            var fileMenu = session.AssertFindElementByName("File", "menü");
            fileMenu.Click();
            var openMenu = session.AssertFindElementByName("Open", "menü");
            openMenu.Click();

            Wait(200); // Wait until the  dialog appears

            var dialog = session.AssertFindElementByName("InputDialog", "dialógus ablak");
            return dialog;
        }

        protected void OpenContentForFile(string fileName)
        {
            // Double click on a.txt in ListView

            var listViewItem = session.AssertFindElementByXPath($"//ListItem[@Name=\"{fileName}\"]/Text", "listaelem fájlnévvel");
            listViewItem.DoubleClick();
        }

        protected static void SetCurrentPathToRoot()
        {
            SetCurrentPath(RootPath);
        }

        protected static void SetCurrentPath(string path)
        {
            // Set path to a folder where we have some files
            using (InputDialog dlg = new InputDialog(session))
            {
                dlg.OpenDialog();
                dlg.SetEditText(path);
                dlg.CloseWithOk();
            }
        }

        protected WindowsElement AssertFindListViewItemForFileOrDir(string fileOrDirName, bool isDir = false, string messageOverride = null)
        {
            return session.AssertFindElementByXPath($"//ListItem[@Name=\"{fileOrDirName}\"]",
                messageOverride??$"{(isDir ? "mappanevet" : "fájlnevet")} megjelenítő listaelem");
        }

        // The Text item has to be returned for double clicking
        protected WindowsElement AssertFindListViewItem_Text_ForFileOrDir(string fileOrDirName, bool isDir = false, string messageOverride = null)
        {
            return session.AssertFindElementByXPath($"//ListItem[@Name=\"{fileOrDirName}\"]/Text",
                messageOverride ?? $"{(isDir ? "mappanevet" : "fájlnevet")} megjelenítő listaelem");
        }


        static string getSetting(TestContext context, string settingName)
        {
            // Get setting from environment variable (highest prio)
            string setting = Environment.GetEnvironmentVariable(settingName);
            // Get setting from .runsettings
            if (setting == null)
                setting = (string)context.Properties[settingName];
            return setting;
        }

        static void applySetting<T>(TestContext context, string settingName, Action<T> applySettingAction)
        {
            // Get setting from environment variable (highest prio)
            string setting = Environment.GetEnvironmentVariable(settingName);
            // Get setting from .runsettings
            if (setting == null)
                setting = (string)context.Properties[settingName];

            if (setting != null)
            {
                T val;
                if (typeof(T) == typeof(string))
                    val = (T)(object)setting;
                else if (typeof(T) == typeof(bool))
                    val = (T)(object)Convert.ToBoolean(setting);
                else if (typeof(T) == typeof(int))
                    val = (T)(object)Convert.ToInt32(setting);
                else
                    throw new NotSupportedException("getSetting: The specified type is not supported");

                applySettingAction(val);
            }

            context.WriteLine($"{settingName}: {setting}");

            //if (typeof(T) == typeof(bool))
            //{
            //    return (T)(object)Convert.ToBoolean(setting);
            //}
            //if (typeof(T) == typeof(int))
            //{
            //    return (T)(object)Convert.ToInt32(setting);
            //}
            //else if (typeof(T) == typeof(string))
            //    return (T)(object)setting;
            //else
            //   throw new NotSupportedException("getSetting: The specified type is not supported");
        }

    }
}
