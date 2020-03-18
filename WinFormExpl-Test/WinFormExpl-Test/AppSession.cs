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

namespace WinFormExpl_Test
{
    public class AppSession
    {
        protected const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";
        protected static readonly TimeSpan DefaultSessionImplicitWaitSec = TimeSpan.FromSeconds(0.1);
        // This setting can be overridden by the runsettings file
        //private static string AppId = @"d:\Tanszek\BZ\SZT-WinForms-Test\Feladatok\WindowsFormsApp\bin\Debug\netcoreapp3.1\ProjectFileForTest.exe";
        private static string AppId = @"..\..\..\..\SampleSolution\WinFormExpl\bin\Debug\netcoreapp3.1\ProjectFileForTest.exe";
        // This setting can be overridden by the runsettings file
        protected static string rootPath = Path.GetFullPath(@"..\..\..\TestFiles");
        protected const string subFolderName = "a.txt";

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
            // Launch a new instance of Notepad application
            if (session == null)
            {
                bool e = File.Exists(AppId);
                // Get AppId from .runsettings
                var tempAppId = context.Properties["AppId"];
                if (tempAppId != null)
                    AppId = (string)tempAppId;
                AppId = Path.GetFullPath(AppId); // WindowsDriver apparently cannot work with relative paths (sounds reasonable if path is sent to WinAPpDriver server)
                context.WriteLine("AppId: " + AppId);

                // Get RootPath from .runsettings
                var tempRootPath = context.Properties["RootPath"];
                if (tempRootPath != null)
                    rootPath = (string)tempRootPath;
                context.WriteLine("rootPath: " + rootPath);

                // Create a new session to launch Notepad application
                DesiredCapabilities appCapabilities = new DesiredCapabilities();
                appCapabilities.SetCapability("app", AppId);
                appCapabilities.SetCapability("ms:experimental-webdriver", true);
                session = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appCapabilities);
                Assert.IsNotNull(session);
                Assert.IsNotNull(session.SessionId);

                // Verify that App is started
                Assert.AreEqual("MiniExplorer", session.Title, "Nem található a MiniExplorer fejlécű ablak");

                // Set implicit timeout to 1.5 seconds to make element search to retry every 500 ms for at most three times
                session.Manage().Timeouts().ImplicitWait = DefaultSessionImplicitWaitSec;
            }
        }

        public static void TearDown()
        {
            // Close the application and delete the session
            if (session != null)
            {
                session.Close();

                //try
                //{
                //    // Dismiss Save dialog if it is blocking the exit
                //    session.FindElementByName("Don't Save").Click();
                //}
                //catch { }

                session.Quit();
                session = null;
            }
        }

        [TestInitialize]
        public void TestInitialize()
        {
        }

        protected  WindowsElement OpenDialog()
        {
            var fileMenu = session.AssertFindElementByName("File", "menü");
            fileMenu.Click();
            var openMenu = session.AssertFindElementByName("Open", "menü");
            openMenu.Click();

            Thread.Sleep(200); // Wait for half second until the  dialog appears

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
            SetCurrentPath(rootPath);
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

    }
}
