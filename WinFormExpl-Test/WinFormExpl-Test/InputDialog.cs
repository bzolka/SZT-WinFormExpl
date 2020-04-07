using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WinFormExpl_Test
{
    class InputDialog: IDisposable
    {
        WindowsDriver<WindowsElement> session;
        WindowsElement dialog;

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

            AppSession.Wait(200); // Wait until the  dialog appears

            dialog = session.AssertFindElementByName("InputDialog", "dialógus ablak");
            return dialog;
        }
        public WindowsElement GetEdit()
        {
            // Both works, originally thought AssertFindElementByTagName is faster, does not seem so
            //return session.AssertFindElementByXPath("//Edit", "TextBox az útvonal bekéréshez");
            return session.AssertFindElementByTagName("Edit", "TextBox az útvonal bekéréshez");
        }

        public WindowsElement GetOkButton()
        {
            return session.AssertFindElementByAlternativeNames(new string[] { "Ok", "OK", "ok" }, "gomb");
        }

        public WindowsElement GetCancelButton()
        {
            return session.AssertFindElementByName("Cancel", "gomb");
        }

        public void SetEditText(string text)
        {
            var edit = GetEdit();
            edit.SendKeys(Keys.Control + "a");
            text = SanitizeSpecialKeys(text);
            edit.SendKeys(text);
        }

        public string GetEditText()
        {
            var edit = GetEdit();
            return edit.Text;
        }

        public Size GetSize()
        {
            return dialog.Size;
        }

        public void CloseWithCancel()
        {
            var cancelButton = GetCancelButton();
            cancelButton.Click();
            AppSession.Wait(500);
            session.AssertElementNotFound("Cancel", "A Cancel gomb nem zárja be a dialógus ablakot + (az is probléma lehet, ha túl széles/magas a dialógusablak).");
        }

        public void CloseWithOk()
        {
            var OklButton = GetOkButton();
            OklButton.Click();
            AppSession.Wait(500);
            session.AssertElementNotFound("Ok", "Az OK gomb nem zárja be a dialógus ablakot.");
        }

        static string SanitizeBackslashes(string input) => input.Replace("\\", Keys.Alt + Keys.NumberPad9 + Keys.NumberPad2 + Keys.Alt);

        static string SanitizeSpecialKeys(string input)
        {
            // On a 101 hun keyboard some keys are swapped, etc.
            // Add extra items if you run into further cases
            return input.Replace("\\", Keys.Alt + Keys.NumberPad9 + Keys.NumberPad2 + Keys.Alt)
                .Replace("-", Keys.Alt + Keys.NumberPad4 + Keys.NumberPad5 + Keys.Alt)
                .Replace("y", Keys.Alt + Keys.NumberPad1 + Keys.NumberPad2 + Keys.NumberPad1 + Keys.Alt)
                .Replace("Y", Keys.Alt + Keys.NumberPad8 + Keys.NumberPad9 + Keys.Alt)
                .Replace("z", Keys.Alt + Keys.NumberPad1 + Keys.NumberPad2 + Keys.NumberPad2 + Keys.Alt)
                .Replace("Z", Keys.Alt + Keys.NumberPad9 + Keys.NumberPad0 + Keys.Alt);
        }

        public void Dispose()
        {
            // Students might have forgoten to commen out the messagebox, let's try to close it
            // The problem: we don't know if this is the OK button of the InputDialog or of a message box
            // Don't close the InputDialog with the OK here, that could result in an empty path set.
            // Solution: use the below order of ops (better would be differentiate between the two windows somehow)

            // This is for the Input dialog
            try
            {
                session.FindElementByName("Cancel").Click();
            }
            catch { }

            try
            {
                // Dismiss message box if displayed
                session.FindElementByName("OK").Click();
            }
            catch { }

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
    }
}
