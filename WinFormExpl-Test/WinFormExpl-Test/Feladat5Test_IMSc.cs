using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WinFormExpl_Test
{
    [TestClass]
    public class Feladat5Test_IMSc : AppSession
    {
        const string IMSc_MessagePrefix = "IMSc - ";

        [TestMethod]
        public void TestDirsDisplayed()
        {
            SetCurrentPathToRoot();

            var lvItem = AssertFindListViewItem_Text_ForFileOrDir(FolderA, true);
        }

        [TestMethod]
        public void Test_Inconclusive()
        {
            Assert.Inconclusive("Dummy inconclusive");
        }

        [TestMethod]
        public void TestDirsCanBeOpen()
        {
            SetCurrentPathToRoot();

            var lvItem = AssertFindListViewItem_Text_ForFileOrDir(FolderA, true);
            lvItem.DoubleClick();

            Thread.Sleep(200);

            AssertFindListViewItem_Text_ForFileOrDir(fileC);
            AssertFindListViewItem_Text_ForFileOrDir(fileD);
        }

        [TestMethod]
        public void TestParentFolderShown()
        {
            SetCurrentPathToRoot();

            var lvItem = AssertFindListViewItem_Text_ForFileOrDir(FolderParent, true, "..-ot megjelenítő listaelem");
        }

        [TestMethod]
        public void TestParentFolderDoupleClickNavigatesToParent()
        {
            SetCurrentPath(Path.Combine(rootPath, FolderA));

            var lvItem = AssertFindListViewItem_Text_ForFileOrDir(FolderParent, true, "..-ot megjelenítő listaelem");
            lvItem.DoubleClick();

            AssertFindListViewItem_Text_ForFileOrDir(fileA);
            AssertFindListViewItem_Text_ForFileOrDir(fileB);
        }

        [TestMethod]
        public void TestParentFolderIsNotDisplayedInRoot()
        {
            string root = Path.GetPathRoot(rootPath);
            SetCurrentPath(root);

            try
            {
                var lvItem = AssertFindListViewItem_Text_ForFileOrDir(FolderParent, true, "..-ot megjelenítő listaelem");
                Assert.Fail("A gyökérmappában is megjelenik a .. listaelem.");
            }
            catch (AssertFailedException) { }

        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            Setup(context);
        }

        [ClassCleanup]
        public static void ClassCleanup()
        {
            try
            {
                TearDown();
            }
            catch (Exception) { }            
        }
    }
}
