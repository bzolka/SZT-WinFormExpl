﻿using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WinFormExpl_Test;

namespace WinFormExpl_Test1
{
    [TestClass]
    public class Feladat1Test_Menu: AppSession
    {
        [TestMethod]
        public void TestAll()
        {
            // Vezérlők megtalálása FindElementByName-mel:
            // Ha az AccessibleName ki van töltve, akkor az alapján 
            // Ha nincs kitötltve, akkor a Text alapján
            // Vigyázat, ha az AccessibleName egyszer ki volt töltve, akkor nem elég a prop 
            // editorban törölni, a designer.cs-ből is kell!

            var fileMenu = session.AssertFindElementByName("File", "menü");
            fileMenu.Click();
            session.AssertFindElementByName("Open", "menü");

            // TODO-BZ
            // Probléma: ClassCleanup csak későn hívódik a tesztek futtatása után, nem akkor, amikor az egyik osztály minden tesztje lefutott már 
            //var exitMenu = session.AssertFindElementByName("Exit", "menü");
            //exitMenu.Click();
            //Wait(1000); // Not sure if this is needed
            //session.AssertElementNotFound("File", "Az Exit menü nem zárja be az alkalmazást!");

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
