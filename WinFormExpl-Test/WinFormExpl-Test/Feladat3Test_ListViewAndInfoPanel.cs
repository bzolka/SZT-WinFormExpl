﻿using System;
using System.Drawing;
using System.IO;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Interactions;
using WinFormExpl_Test;

namespace WinFormExpl_Test1
{
    [TestClass]
    public class Feladat3Test_ListViewAndInfoPanel: AppSession
    {
        AssertElements assertElements = new AssertElements(session);

        [TestMethod]
        public void TestContentView()
        {
            testSelectionDoesNotChangeContent();
            testContentForFile(fileA);
            testContentForFile(fileB);
        }      

        [TestMethod]
        public void TestFileView()
        {
            // ListView exists
            assertElements.FileListView();
            var headerName = session.AssertFindElementByXPath("//HeaderItem[@Name=\"Name\"]", "Name fejlécű oszlop");
            // Size header exists
            var headerSize = session.AssertFindElementByXPath("//HeaderItem[@Name=\"Size\"]", "Size fejlécű oszlop");

            // fileA
            AssertFindListViewItemForFileOrDir(fileA);
            // fileB
            AssertFindListViewItemForFileOrDir(fileB);

            const string fileSizeProblemText = "listaelem, méret. A listában a Size oszlopban listaelemek nincsenek" +
                " kitöltve, vagy nem a méretet jelenítik meg, vagy nem jó formátumban jelenítik meg " +
                "(a méret oszlop a fájl méretét jelenítse meg byte-ban, csak a számot, mindenféle mértékegység hozzáfűzése nélkül)";
            // fileA size
            session.AssertFindElementByXPath($"//Text[contains(@Name,\"{fileASize}\")]", fileSizeProblemText);
            // fileB size
            session.AssertFindElementByXPath($"//Text[contains(@Name,\"{fileBSize}\")]", fileSizeProblemText);

            //var elements = session.FindElementsByXPath("//*");
            // session.AssertFindElementByXPath("/ListItem[@T=\"a.txt\"]", "alma");


            // Jó
            // ok: session.AssertFindElementByXPath("//List[@AutomationId=\"lvFiles\"]", "alma");
            // 
            // ?? session.AssertFindElementByXPath("/List[@AutomationId=\"lvFiles\"]/ListItem[@Name=\"a.txt\"][starts-with(@AutomationId,\"ListViewItem-\")]", "alma");
            // ?? session.AssertFindElementByXPath("/ListItem[@Name=\"a.txt\"][starts-with(@AutomationId,\"ListViewItem-\")]", "alma");
            // ok: session.AssertFindElementByXPath("//ListItem[@Name=\"a.txt\"]", "alma");
            // nok: session.AssertFindElementByXPath("//ListItem[starts-with(@AutomationId,\"ListViewItem-\")]", "alma");       
            // nok: session.AssertFindElementByXPath("/List/ListItem[@Name=\"a.txt\"]" +
            //    "[starts-with(@AutomationId,\"ListViewItem-\")]/Text[@Name=\"a.txt\"][starts-with(@AutomationId,\"ListViewSubItem-\")]", "listaelem, fájlnév");


        }
        
        [TestMethod]
        public void TestInfoPanel()
        {
            // select b.txt in ListView
            var listViewItem = session.AssertFindElementByXPath("//ListItem[@Name=\"b.txt\"]/Text", "listaelem fájlnévvel");
            listViewItem.Click();

            var lName = session.AssertFindElementByXPath("//Text[contains(@Name,\"b.txt\")][@AutomationId=\"lName\"]", "Olyan címke, mely a listában kiválasztott fájl nevét mutatja. Vagy ha létezik a címke, " +
                "akkor az nem az aktuálisan kiválasztott fájl nevét jeleníti meg. Az is probléma lehet, hogy csak duplakattintás, és nem egyszerű kiválasztás után jeleníti meg a fájl nevét! " +
                "Szintén probléma lehet, ha a fájl nevét nem egy lName nevű Label jelenítni meg. A címke neve lName kell legyen (melyben a prefix kicsi 'l', a labelre utalva, és nem nagy 'I', mint Ibolya)");

            //var lName = session.AssertFindElementByXPath("//Text[@Name=\"b.txt\"][@AutomationId=\"lName\"]", "Olyan címke, mely a listában kiválasztott fájl nevét mutatja. Vagy ha létezik a címke, " +
            //    "akkor az nem az aktuálisan kiválasztott fájl nevét jeleníti meg. Az is probléma lehet, hogy csak duplakattintás, és nem egyszerű kiválasztás után jeleníti meg a fájl nevét!");

            string sCreated = new FileInfo(Path.Combine(RootPath, "b.txt")).CreationTime.ToString();
            session.AssertFindElementByXPath($"//Text[contains(@Name,\"{sCreated}\")][@AutomationId=\"lCreated\"]", "Olyan címke, mely a listában kiválasztott fájl létrehozási dátumát mutatja. Pár lehetséges probléma:"
                + "\r\n * A cimke nem az aktuálisan kiválasztott fájl létrehozási dátumát jeleníti meg"
                + "\r\n * Duplakattintás, és nem egyszerű kiválasztás után jeleníti meg a fájl létrehozási dátumát"
                + "\r\n * A fájl létrehozás dátumát nem egy lCreated nevű Label jeleníti meg"
                + "\r\n * A dátum formázása nem alapértelmezett (paraméter nélküli ToString() hívást használj)");
        }

        [TestMethod]
        public void TestSplitter()
        {
            // Volt olyan hallgatói megoldás, ahol ez nagy DPI-n nálam elesett, de 
            // sima full hd-n nem! De aztán visszatéve uhd/nagy dpi-be, WinAppDriver újraindítva már megtalálta!
            // TODO-bz, ez csak akkor működik, ha nem nevezte át? 
            session.AssertFindElementByXPath("//Pane[starts-with(@AutomationId,\"splitContainer\")]", "Splitter. Az is lehet, hogy a " +
                "SplitContainer neve nem megfelelő (az alapértelmezett splitContainer1 kell legyen).");
        }

        void testSelectionDoesNotChangeContent()
        {
            var editContent = assertElements.FileContentEdit();

            var listViewItemA = session.AssertFindElementByXPath($"//ListItem[@Name=\"{fileA}\"]/Text", "listaelem fájlnévvel");
            listViewItemA.Click(); // Valahol, mintha írták volna, hogy erre szükség lehet!
            listViewItemA.DoubleClick();
            string editContentTextBeforeClick = editContent.Text;

            var listViewItemB = session.AssertFindElementByXPath($"//ListItem[@Name=\"{fileB}\"]/Text", "listaelem fájlnévvel");
            listViewItemB.Click();
            string editContentTextAfterClick = editContent.Text;

            Assert.AreEqual(editContentTextBeforeClick, editContentTextAfterClick, "A többsoros szövegdoboz (tContent) tartalma akkor is megváltozik, ha " +
                " a listviewban új fájl kerül kiválasztásra. Csak akkor szabad megváltoznia, ha a felhasználó duplán kattint az egérrel egy fájlon.");
        }

        void testContentForFile(string fileName)
        {
            // Double click on fileA in ListView
            OpenContentForFile(fileName);
            var editContent = assertElements.FileContentEdit();
            Wait(300);
            string fileContentText = File.ReadAllText(Path.Combine(RootPath, fileName));
            Assert.AreEqual(fileContentText, editContent.Text, "A többsoros szövegdoboz nem jeleníti meg a fájl tartalmát");
        }

        //[TestMethod]
        //public void TestRun()
        //{
        //    // Ezt idén kihagyjuk, .NET Core-ban más, a feladatsorból is kikerült

        //}

        [TestMethod]
        public void TestDock()
        {
            var listView = assertElements.FileListView();
            var listViewOriginalSize = listView.Size;

            var editContent = assertElements.FileContentEdit();
            var editContentOriginalSize = editContent.Size;

            var detailsPanel = session.AssertFindElementByXPath("//Pane[@AutomationId=\"detailsPanel\"]", "detailsPanel nevű panel");
            int detailsPanelOriginalHeight = detailsPanel.Size.Height;

            var originalWindowSize = session.Manage().Window.Size;

            // First, test by increasing the window. This may fail if original window size is very larga, as new size may exceed the available desktop space
            var offset = new Size(50, 60);
            try
            {
                testResize(listView, listViewOriginalSize, editContent, editContentOriginalSize, detailsPanel, detailsPanelOriginalHeight, originalWindowSize, offset);
            } 
            catch (Exception)
            {
                offset = new Size(-50, -60);
                //  If failed, try by decresing the window size (his may fail if original window size is very small, or controls on it (textbox) are very small)
                testResize(listView, listViewOriginalSize, editContent, editContentOriginalSize, detailsPanel, detailsPanelOriginalHeight, originalWindowSize, offset);
            }

        }

        private static void testResize(WindowsElement listView, Size listViewOriginalSize, WindowsElement editContent, Size editContentOriginalSize, WindowsElement detailsPanel, int detailsPanelOriginalHeight, Size originalWindowSize, Size offset)
        {
            // Resize main window
            var newWindowSize = new Size(originalWindowSize.Width + offset.Width, originalWindowSize.Height + offset.Height);
            session.Manage().Window.Size = newWindowSize;
            Wait(500);

            // Check if main window could actually be resized
            var actualWindowSize = session.Manage().Window.Size;
            const int resizeErrorMargin = 5;
            if (actualWindowSize.Width < newWindowSize.Width - resizeErrorMargin || actualWindowSize.Width > newWindowSize.Width + resizeErrorMargin
                || actualWindowSize.Height < newWindowSize.Height - resizeErrorMargin || actualWindowSize.Height > newWindowSize.Height + resizeErrorMargin)
            {
                Assert.Fail("Valamilyen nem várt okból az ablak mérete nem változott meg átméretezéskor, " +
                  "lehet GitHub környezeti probléma miatt, vagy mert túl nagy/kicsi az ablak alapmérete" +
                  $" (Benedek Zoltán felé jelezd kérlek a problémát). Várt méret: {newWindowSize}, aktuális méret: {actualWindowSize}");
            }


            Assert.AreEqual(detailsPanelOriginalHeight, detailsPanel.Size.Height, "Ablak átméretezés után a details panel magassága megváltozott.");

            var listViewNewSize = listView.Size;

            double panelRatio = (double)listViewOriginalSize.Width / (editContentOriginalSize.Width + listViewOriginalSize.Width);
            Assert.IsTrue(
                listViewNewSize.Width >= listViewOriginalSize.Width + (int)(offset.Width * panelRatio) - 15 && listViewNewSize.Width <= listViewOriginalSize.Width + (int)(offset.Width * panelRatio) + 15 &&
                listViewNewSize.Height >= listViewOriginalSize.Height + offset.Height - 5 && listViewNewSize.Height <= listViewOriginalSize.Height + offset.Height + 5,
                "A fájlmegjelenítő lista nem méreteződik az ablakkal");

            var editContentNewSize = editContent.Size;
            Assert.IsTrue(
                editContentNewSize.Width >= editContentOriginalSize.Width + (int)(offset.Width * (1 - panelRatio)) - 15 && editContentNewSize.Width <= editContentOriginalSize.Width + (int)(offset.Width * (1 - panelRatio)) + 15 &&
                editContentNewSize.Height >= editContentOriginalSize.Height + offset.Height - 5 && editContentNewSize.Height <= editContentOriginalSize.Height + offset.Height + 5,
                "A fájltartalom megjelenítő szövegdoboz nem jól méreteződik az ablakkal. A probléma okai lehetnek többek között: " +
                "\r\n* A szövegdoboz dock tulajdonsága nincs jól beállítva" +
                "\r\n* A details panel nem fix magasságú (nincs Top módon dokkolva)" +
                "\r\n* A szövegdobozt nem a SplitContainer Panel2-n van, hanem a details panelen (a Document Outline ablakban ellenőrizhető és drag&drop-pal javítható)");
        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Setup(context);
            SetCurrentPathToRoot();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TearDown();
        }
    }
}
