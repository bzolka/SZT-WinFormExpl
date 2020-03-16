using System;
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
            var listView = session.AssertFindElementByXPath("//List", "ListView megjelenítő");
            // Name header exists
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
            var editContent = session.AssertFindElementByXPath("//Edit[@AutomationId=\"tContent\"]", "tContent nevű többsoros szövegdoboz");
            string fileContentText = File.ReadAllText(Path.Combine(path, fileName));
            Assert.AreEqual(fileContentText, editContent.Text, "A többsoros szövegdoboz nem jeleníti mega  fájl tartalmát");
        }

        [TestMethod]
        public void TestRun()
        {


        }

        public void TestDock()
        {

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
