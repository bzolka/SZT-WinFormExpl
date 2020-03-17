using OpenQA.Selenium.Appium.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinFormExpl_Test
{
    class AssertElements
    {
        WindowsDriver<WindowsElement> session;

        public AssertElements(WindowsDriver<WindowsElement> session)
        {
            this.session = session;
        }

        public WindowsElement FileListView() => 
            session.AssertFindElementByXPath("//List", "ListView megjelenítő");

        public WindowsElement FileContentEdit() => 
            session.AssertFindElementByXPath("//Edit[@AutomationId=\"tContent\"]", "tContent nevű többsoros szövegdoboz");
    }
}
