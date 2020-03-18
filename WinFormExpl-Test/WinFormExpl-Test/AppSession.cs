using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
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
        // TODO-bz
        private const string AppId = @"d:\Tanszek\BZ\SZT-WinForms-Test\Feladatok\WindowsFormsApp\bin\Debug\netcoreapp3.1\ProjectFileForTest.exe";
        protected const string path = @"c:\temp\Watched\";    // TODO-bz, adjust path
        protected const string fileA = "a.txt";
        protected const string fileB = "b.txt";
        protected const string fileASize = "13";
        protected const string fileBSize = "14";
        protected const int menuItemFileWidthIn96Dpi = 37; // Can be OS dependent
        

        protected static WindowsDriver<WindowsElement> session;
        //protected static WindowsElement editBox;

        public static void Setup(TestContext context)
        {
            // Launch a new instance of Notepad application
            if (session == null)
            {
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

                // Keep track of the edit box to be used throughout the session
                //var editBox = session.FindElementByClassName("Edit");
                //Assert.IsNotNull(editBox);
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
            // Select all text and delete to clear the edit box
            //editBox.SendKeys(Keys.Control + "a" + Keys.Control);
            //editBox.SendKeys(Keys.Delete);
            //Assert.AreEqual(string.Empty, editBox.Text);
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

        protected void openContentForFile(string fileName)
        {
            // Double click on a.txt in ListView

            var listViewItem = session.AssertFindElementByXPath($"//ListItem[@Name=\"{fileName}\"]/Text", "listaelem fájlnévvel");
            listViewItem.DoubleClick();
        }

    }
}
