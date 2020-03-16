using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormExpl_Test
{
    static class WindowsElementExtensions
    {
        public static void DoubleClick(this WindowsElement element)
        {
            Actions action = new Actions(element.WrappedDriver);
            action.MoveToElement(element).Click().DoubleClick().Build().Perform();
        }
    }
}
