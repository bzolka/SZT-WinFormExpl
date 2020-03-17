﻿using System;
using System.Drawing;
using System.IO;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Interactions;

namespace WinFormExpl_Test
{
    [TestClass]
    public class Feladat3Test: AppSession
    {
        const string path = @"c:\temp\Watched\";    // TODO-bz, adjust path
        static InputDialog dlg;

        AssertElements assertElements = new AssertElements(session);


        [TestMethod]
        public void TestSplitter()
        {
            // TODO-bz, ez csak akkor működik, ha nem nevezte át? 
            session.AssertFindElementByXPath("//Pane[starts-with(@AutomationId,\"splitContainer\")]", "Splitter");
        }

        [TestMethod]
        public void TestFileView()
        {
            // ListView exists
            assertElements.FileListView();
            var headerName = session.AssertFindElementByXPath("//HeaderItem[@Name=\"Name\"]", "Name fejlécű oszlop");
            // Size header exists
            var headerSize = session.AssertFindElementByXPath("//HeaderItem[@Name=\"Size\"]", "Size fejlécű oszlop");

            // a.txt
            session.AssertFindElementByXPath("//ListItem[@Name=\"a.txt\"]", "listaelem, fájlnév");
            // b.txt
            session.AssertFindElementByXPath("//ListItem[@Name=\"b.txt\"]", "listaelem, fájlnév");
            // a.txt size
            session.AssertFindElementByXPath("//Text[@Name=\"13\"]", "listaelem, méret");
            // b.txt size
            session.AssertFindElementByXPath("//Text[@Name=\"14\"]", "listaelem, méret");

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
        public void TesInfoPanel()
        {
            // select b.txt in ListView
            var listViewItem = session.AssertFindElementByXPath("//ListItem[@Name=\"b.txt\"]/Text", "listaelem fájlnévvel");
            listViewItem.Click();

            var lName = session.AssertFindElementByXPath("//Text[@Name=\"b.txt\"][@AutomationId=\"lName\"]", "címke, mely a fájl nevét mutatja");

            string sCreated = new FileInfo(Path.Combine(path, "b.txt")).CreationTime.ToString();
            var lCreated = session.AssertFindElementByXPath($"//Text[@Name=\"{sCreated}\"][@AutomationId=\"lCreated\"]", "címke, mely a fájl létrehozási idejét mutatja");

            
        }

        [TestMethod]
        public void TestContentView()
        {
            testContentForFile("a.txt");
            testContentForFile("b.txt");
        }

        void testContentForFile(string fileName)
        {
            // Double click on a.txt in ListView

            var listViewItem = session.AssertFindElementByXPath($"//ListItem[@Name=\"{fileName}\"]/Text", "listaelem fájlnévvel");
            listViewItem.DoubleClick();
            var editContent = assertElements.FileContentEdit();
            string fileContentText = File.ReadAllText(Path.Combine(path, fileName));
            Assert.AreEqual(fileContentText, editContent.Text, "A többsoros szövegdoboz nem jeleníti mega  fájl tartalmát");
        }

        [TestMethod]
        public void TestRun()
        {
            // TODO-bz

        }

        [TestMethod]
        public void TestDock()
        {
            // TODO-bz
            var listView = assertElements.FileListView();
            var listViewOriginalSize = listView.Size;

            var editContent = assertElements.FileContentEdit();
            var editContentOriginalSize = editContent.Size;

            var windowSize = session.Manage().Window.Size;

            var offset = new Size(100, 120);

            // Resize main window
            session.Manage().Window.Size = new Size(windowSize.Width + offset.Width, windowSize.Height + offset.Height);
            Thread.Sleep(500);

            var listViewNewSize = listView.Size;
            Assert.IsTrue(
                listViewNewSize.Width >= listViewOriginalSize.Width + offset.Width/2 - 15  && listViewNewSize.Width <= listViewOriginalSize.Width + offset.Width / 2 + 15 &&
                listViewNewSize.Height >= listViewOriginalSize.Height + offset.Height -5 && listViewNewSize.Height <= listViewOriginalSize.Height + offset.Height + 5,
                "A fájlmegjelenítő lista nem méreteződik az ablakkal");

            var editContentNewSize = editContent.Size;
            Assert.IsTrue(
                editContentNewSize.Width >= editContentOriginalSize.Width + offset.Width / 2 - 15 && editContentNewSize.Width <= editContentOriginalSize.Width + offset.Width / 2 + 15 &&
                editContentNewSize.Height >= editContentOriginalSize.Height + offset.Height - 5 && editContentNewSize.Height <= editContentOriginalSize.Height + offset.Height + 5,
                "A fájltartalom megjelenítő szövegdoboz nem méreteződik az ablakkal");
        }


        static void InitPath()
        {
            // Set path to a folder where we have some files
            InputDialog dlg = new InputDialog(session);
            dlg.OpenDialog();
            dlg.SetEditText(path);
            dlg.CloseWithOk();

            // Check if main window displays content

            // Check functionality of the Run menu
        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Setup(context);
            InitPath();
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            TearDown();
        }
    }
}
