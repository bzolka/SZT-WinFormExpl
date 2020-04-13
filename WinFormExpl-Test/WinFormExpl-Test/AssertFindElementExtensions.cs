using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormExpl_Test
{
    static class AssertFindElementExtensions
    {
        const string ErrorTextTemplate = "Nem található a következő felületelem: {0}" 
            + "\r\nPár lehetséges speciálisabb ok:" 
            + "\r\n* A vezérlő AccessibleName tulajdonságát is állítottad: ha így történt, nyisd meg a megfelelő designer.cs forrásfájlt, és töröld ki azokat a sorokat, melyek az AccessibleName tulajdonságot állítják. "
            + "\r\n* A MessageBox feldobását nem kommentezted ki. "
            + "\r\n* Van egy extra space is az adott elem szövegének az elején/végén."
            + "\r\n* Valamilyen hiba keletkezett az előző tesztek egyikének következtében, ami egy hibaablak megjelenését vonja maga után";
        public static WindowsElement AssertFindElementByName(this WindowsDriver<WindowsElement> session, string name, string elementType)
        {
            try
            {
                return session.FindElementByName(name);
            }
            catch (InvalidOperationException)
            {
                assertInCaseErrorMessageIsDisplayed_AndCloseDialog(session);
                Assert.Fail(string.Format(ErrorTextTemplate, name + " " + elementType));
                throw; // Needed to reassure the compiler
            }
        }

        public static WindowsElement AssertFindElementByXPath(this WindowsDriver<WindowsElement> session, string xpath, string elementDescription)
        {
            try
            {
                return session.FindElementByXPath(xpath);
            }
            catch (InvalidOperationException)
            {
                assertInCaseErrorMessageIsDisplayed_AndCloseDialog(session);
                Assert.Fail(string.Format(ErrorTextTemplate, elementDescription));
                throw;
            }
        }

        /// <summary>
        /// Finds element by TagName, which is of "type", e.g. for TextBox it's Edit. You can find the TagName out by finding the element by other means
        /// and then checking the TagName property of the WindowsElement object and removing the "ControlType." prefix.
        /// </summary>
        public static WindowsElement AssertFindElementByTagName(this WindowsDriver<WindowsElement> session, string xpath, string tagName)
        {
            try
            {
                return session.FindElementByTagName(xpath);
            }
            catch (InvalidOperationException)
            {
                assertInCaseErrorMessageIsDisplayed_AndCloseDialog(session);
                Assert.Fail(string.Format(ErrorTextTemplate, tagName));
                throw;
            }
        }

        public static WindowsElement AssertFindElementByAlternativeNames(this WindowsDriver<WindowsElement> session, string[] names, string elemetType)
        {
            foreach (var name in names)
            {
                try
                {
                    var e = session.FindElementByName(name);
                    return e;
                }
                catch (InvalidOperationException)
                {
                }
            }
            assertInCaseErrorMessageIsDisplayed_AndCloseDialog(session);
            Assert.Fail(string.Format(ErrorTextTemplate, names[0] + " " + elemetType));
            throw new Exception("never get here"); // Needed to reassure the compiler
        }

        public static void AssertElementNotFound(this WindowsDriver<WindowsElement> session, string name, string errorText)
        {
            try
            {
                session.FindElementByName(name);
                Assert.Fail(errorText);
            }
            catch (InvalidOperationException)
            {
            }
        }

        static void assertInCaseErrorMessageIsDisplayed_AndCloseDialog(WindowsDriver<WindowsElement> session)
        {
            string msg = getErrorMessage_InCaseErrorMessageIsDisplayed_AndCloseDialog(session);
            if (msg != null)
                Assert.Fail("Nem található egy felületelem. Ennek oka, hogy a teszt, vagy egy előző teszt következtében az alkalmazás egy hibaablakot" +
                    " jelenít meg az alábbi szöveggel: \r\n" + msg);
        }

        static string getErrorMessage_InCaseErrorMessageIsDisplayed_AndCloseDialog(WindowsDriver<WindowsElement> session)
        {
            WindowsElement bQuit = null;
            WindowsElement bDetails = null;
            try
            {
                bQuit = session.FindElementByName("Quit");
            } catch { }

            try
            {
                bDetails = session.FindElementByName("Details");
            }
            catch { }

            // Probably we don't have the standard error window
            if (bQuit == null || bDetails == null)
                return null;

            // Open Details
            bDetails.Click();

            // Try to find details textbox

            string errorText = null;
            try
            {
                errorText = session.FindElementByTagName("Edit").Text;
            }
            catch { }

            // !!!!!
            // Valami nagyon mellément, zárjuk be az alkalmazást. De valóban ez a jó irány? Nemdeterminisztikus, mi az időzítése,
            // lehet már nem is lesz alkalma az alkalmazásnak kinaplózni a hibaszöveget? Jobb lenne a "Continue" használata?
            try
            {
                bQuit.Click();
            }
            catch { }

            return errorText;
        }
    }
}
