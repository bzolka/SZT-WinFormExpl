using System;
using System.Collections.ObjectModel;
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
        public void TestAll()
        {
            InputDialog dlg = new InputDialog(session);

            // Check if the dialog can be open
            dlg.OpenDialog();

            // Check if Cancel button exists and it closes the dialog
            dlg.CloseWithCancel();

            // Check if Textbox exists and we can type into it
            dlg.OpenDialog();
            const string path = @"c:\temp\Watched\";    // TODO-bz, adjust path
            dlg.SetEditText(path);
            string dlgPath = dlg.GetEditText();
            Assert.AreEqual(path, dlgPath);

            // TODOTODOTODOTODOTODOTODOTODO - findElements, mindet megtalálni és ezekete végignézni!
            // var elements = session.FindElementsByXPath("//*");
            // Check if OK button exists and it closes the dialog
            //string sss = edit.GetAttribute("Path");
            //Assert.AreEqual(path, sss);
            dlg.CloseWithOk();
        }



        // Never throws
        // Should be made more robust, so that it can also find it if dialog ha different name.
        void closeDialog()
        {
            try
            {
                var dialog = session.AssertFindElementByName("InputDialog", "dialógus ablak");
                // This works
                dialog.SendKeys(Keys.Alt + Keys.F4);
                // This works as well
                //Actions actions = new Actions(session);
                //actions.KeyDown(Keys.Alt);
                //actions.SendKeys(Keys.F4);
                //actions.KeyUp(Keys.Alt);
                //actions.Perform();
            }
            catch (Exception) { }

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
