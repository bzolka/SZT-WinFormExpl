using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WinFormExpl_Test
{
    [TestClass]
    public class Feladat1Test: AppSession
    {
        [TestMethod]
        public void TestMethod1()
        {
            // Vezérlők megtalálása FindElementByName-mel:
            // Ha az AccessibleName ki van töltve, akkor az alapján 
            // Ha nincs kitötltve, akkor a Text alapján
            // Vigyázat, ha az AccessibleName egyszer ki volt töltve, akkor nem elég a prop 
            // editorban törölni, a designer.cs-ből is kell!

            var fileMenu = FindElementByName("File", "menü");
            fileMenu.Click();
            var openMenu = FindElementByName("Open", "menü");
            var exitMenu = FindElementByName("Exit", "menü");
            exitMenu.Click();

            Thread.Sleep(1000); // Not sure if this is needed
            AssertElementNotFound("File", "Az Exit menü nem zárja be az alkalmazást!");

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
