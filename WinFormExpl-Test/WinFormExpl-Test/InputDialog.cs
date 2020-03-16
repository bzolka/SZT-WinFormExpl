using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WinFormExpl_Test
{
    class InputDialog
    {
        WindowsDriver<WindowsElement> session;

        public InputDialog(WindowsDriver<WindowsElement> session)
        {
            this.session = session;
        }

        public WindowsElement OpenDialog()
        {
            var fileMenu = session.AssertFindElementByName("File", "menü");
            fileMenu.Click();
            var openMenu = session.AssertFindElementByName("Open", "menü");
            openMenu.Click();

            Thread.Sleep(500); // Wait for half second until the  dialog appears

            var dialog = session.AssertFindElementByName("InputDialog", "dialógus ablak");
            return dialog;
        }

        public void SetEditText(string text)
        {
            var edit = session.AssertFindElementByXPath("//Edit", "TextBox az útvonal bekéréshez");
            text = SanitizeBackslashes(text);
            edit.SendKeys(text);
        }

        public string GetEditText()
        {
            var edit = session.AssertFindElementByXPath("//Edit", "TextBox az útvonal bekéréshez");
            return edit.Text;
        }

        public void CloseWithCancel()
        {
            var cancelButton = session.AssertFindElementByName("Cancel", "gomb");
            cancelButton.Click();
            Thread.Sleep(500);
            session.AssertElementNotFound("Cancel", "A Cancel gomb nem zárja be a dialógus ablakot.");
        }

        public void CloseWithOk()
        {
            var OklButton = session.AssertFindElementByAlternativeNames(new string[] { "Ok", "OK", "ok" }, "gomb");    // TODO-bz: elfogadja a máshogy  kis-nagybetűzött OK-ot is?
            OklButton.Click();
            Thread.Sleep(500);
            session.AssertElementNotFound("Ok", "Az OK gomb nem zárja be a dialógus ablakot.");
        }

        static string SanitizeBackslashes(string input) => input.Replace("\\", Keys.Alt + Keys.NumberPad9 + Keys.NumberPad2 + Keys.Alt);

    }
}
