using System;
using System.Collections.ObjectModel;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Interactions;

namespace WinFormExpl_Test
{
    class SearchContext : ISearchContext
    {
        public IWebElement FindElement(By by)
        {
            throw new NotImplementedException();
        }

        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            throw new NotImplementedException();
        }
    }

    [TestClass]
    public class Feladat2Test: AppSession
    {
        [TestMethod]
        public void TestMethod1()
        {
            // Check if the dialog can be open
            openDialog();

            // Check if Cancel button exists and it closes the dialog
            var cancelButton = FindElementByName("Cancel", "gomb");
            cancelButton.Click();
            Thread.Sleep(500);
            AssertElementNotFound("Cancel", "A Cancel gomb nem zárja be a dialógus ablakot.");

            // Check if Textbox exists and we can type into it
            var dlg = openDialog();
            //var edit = FindElementByName("tPath", "szövegdoboz"); // TODO-bz: ősbe asserttel + nem accessiblename-mel !!!!
            var edit = FindElementByXPath("//Edit", "TextBox az útvonal bekéréshez"); 
            const string path = @"c:\temp\Watched\";
            string text = SanitizeBackslashes(path);
            edit.SendKeys(text);
            var s = edit.Text;
            Assert.AreEqual(path, s);

            // TODOTODOTODOTODOTODOTODOTODO - findElements, mindet megtalálni és ezekete végignézni!
            var elements = session.FindElementsByXPath("//*");
            // Check if OK button exists and it closes the dialog
            //string sss = edit.GetAttribute("Path");
            //Assert.AreEqual(path, sss);
            var OklButton = FindElementByAlternativeNames(new string[] { "Ok", "OK", "ok" }, "gomb");    // TODO-bz: elfogadja a máshogy  kis-nagybetűzött OK-ot is?
            OklButton.Click();
            Thread.Sleep(500);
            AssertElementNotFound("Ok", "Az OK gomb nem zárja be a dialógus ablakot.");
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

        // Never throws
        // Should be made more robust, so that it can also find it if dialog ha different name.
        void closeDialog()
        {
            try
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
