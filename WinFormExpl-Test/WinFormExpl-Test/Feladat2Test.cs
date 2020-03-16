using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Interactions;

namespace WinFormExpl_Test
{
    [TestClass]
    public class Feladat2Test: AppSession
    {
        [TestMethod]
        public void TestMethod1()
        {
            openDialog();

            closeDialog();
            //var cancelButton = FindElementByName("Cancel", "gomb");
            //cancelButton.Click();
            //Thread.Sleep(500);

            //AssertElementNotFound("Cancel", "A Cancel gomb nem zárja be a dialógus ablakot.");

            //openDialog();
            //var edit = session.FindElementByName("tPath"); // TODO-bz: ősbe asserttel + nem accessiblename-mel
            //const string path = @"c:\temp\Watched\";
            //string text = SanitizeBackslashes(path);
            //edit.SendKeys(text);
            //var s = edit.Text;
            //Assert.AreEqual(path, s);
        }

        WindowsElement openDialog()
        {
            var fileMenu = FindElementByName("File", "menü");
            fileMenu.Click();
            var openMenu = FindElementByName("Open", "menü");
            openMenu.Click();

            Thread.Sleep(1000); // Wait for 1 second until the  dialog appears

            var dialog = FindElementByName("InputDialog", "dialógus ablak");
            return dialog;
        }

        void closeDialog()
        {
            var dialog = FindElementByName("InputDialog", "dialógus ablak");
            // This works
            dialog.SendKeys(Keys.Alt + Keys.F4);
            // This works as well
            //Actions actions = new Actions(session);
            //actions.KeyDown(Keys.Alt);
            //actions.SendKeys(Keys.F4);
            //actions.KeyUp(Keys.Alt);
            //actions.Perform();
        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Setup(context);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TearDown();
        }

    }
}
