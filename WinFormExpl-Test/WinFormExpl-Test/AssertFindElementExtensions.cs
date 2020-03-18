﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        public static WindowsElement AssertFindElementByName(this WindowsDriver<WindowsElement> session, string name, string elementType)
        {
            try
            {
                return session.FindElementByName(name);
            }
            catch (InvalidOperationException)
            {
                Assert.Fail($"Nem található a következő felületelem: {name} {elementType}");
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
                Assert.Fail($"Nem található a következő felületelem: {elementDescription}");
                throw; // Needed to reassure the compiler
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
            Assert.Fail($"Nem található a következő felületelem: {names[0]} {elemetType}");
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