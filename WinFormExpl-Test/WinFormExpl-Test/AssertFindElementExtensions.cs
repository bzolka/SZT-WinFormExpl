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
        const string ErrorTextTemplate = "Nem található a következő felületelem: {0}. (Az is problémát okozhat, ha a vezérlő AccessibleName tulajdonságát is állítottad: " +
                    "ha így történt, nyisd meg a megfelelő designer.cs forrásfájlt, és töröld ki azokat a sorokat, melyek az AccessibleName tulajdonságot állítják. " +
            "Szintén probléma lehet, ha a MessageBox feldobását nem kommentezted ki.)";
        public static WindowsElement AssertFindElementByName(this WindowsDriver<WindowsElement> session, string name, string elementType)
        {
            try
            {
                return session.FindElementByName(name);
            }
            catch (InvalidOperationException)
            {
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
    }
}
