using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WinFormExpl_Test;

namespace WinFormExpl_Test1
{
    [TestClass]
    public class Feladat4Test_Progressbar : AppSession
    {
        AssertElements assertElements = new AssertElements(session);

        // Make sure to update the Current item when introducing a new year
        enum Year  { Y2021, Y2020, Y2019, Y2018, Y2017, Y2016, Current = Y2021 };

        class ExpectedParams
        {
            public readonly int Progress_Width_InPixels;
            public readonly int Progress_Height_InPixels;
            public readonly int Progress_IntervalSec;
            public readonly Color Progress_Color;

            public ExpectedParams(int progress_Width_InPixels, int progress_Height_InPixels, int progress_IntervalSec, Color progress_Color)
            {
                Progress_Width_InPixels = progress_Width_InPixels;
                Progress_Height_InPixels = progress_Height_InPixels;
                Progress_IntervalSec = progress_IntervalSec;
                Progress_Color = progress_Color;
            }
        }

        readonly Dictionary<Year, ExpectedParams> parametersForYears = new Dictionary<Year, ExpectedParams>()
        {
            // Ne vedd túl kicsire, 10 sec alattira a frissítési időt, az automata tesztben túl nagy elcsúszások
            // lesznek, pl. a kezdeti hosszmérése a progressnek már nagyon elmegy
            // { Year.Y2020, new ExpectedParams(120, 5, 12, Color.Green) },

            // A KEZDŐHOSSZ NE VÁLTOZZON, AZT MINDEN ÉVBEN A HALLGATÓK SZÁMÁRA MEG IS JELENÍTJÜK, HA
            // ENNEK KAPCSÁN PROBLÉMA VAN!
            // SAJNOS ELRONTOTTAM, maradt az útmutatóban a 120 a 100 helyett, így itt is visszaírtam, 
            // ez a hajó elment.
            { Year.Y2021, new ExpectedParams(120, 8, 3, Color.Blue) },
            { Year.Y2020, new ExpectedParams(120, 5, 4, Color.Green) },
            { Year.Y2019, new ExpectedParams(100, 2, 10, Color.Red) },
            { Year.Y2018, new ExpectedParams(100, 2, 10, Color.Red) },
            { Year.Y2017, new ExpectedParams(100, 5, 10, Color.Green) },
            { Year.Y2016, new ExpectedParams(100, 10, 10, Color.Blue) }
        };


        [TestMethod]
        public void TestProgressbar()
        {
            //var dpi = getDPI();
            //double dpiScale1 = (double)dpi.dpiX / 96;

            double dpiScale = getDpiScaleFactor();

            // Open file with minimum delay till first sampling of length
            try
            {

                // Get info panel
                var detailsPanel = session.AssertFindElementByXPath("//Pane[@AutomationId=\"detailsPanel\"]", "detailsPanel nevű panel");
                var delatisPanelOffset = detailsPanel.Location;

                session.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(50);
                OpenContentForFile(fileA);

                //Wait(200);

                var parameters = parametersForYears[Year.Current];
                int expectedProgress_Width_InPixels = (int)(parameters.Progress_Width_InPixels * dpiScale);

                // Instantly get progress length
                int? lenAtStart = getProgressBarLength(session.GetScreenshot().AsByteArray, delatisPanelOffset);
                int sleepIntervalMS = 2000;
                Thread.Sleep(sleepIntervalMS);
                int? lenAfter2Sec = getProgressBarLength(session.GetScreenshot().AsByteArray, delatisPanelOffset);

                if (!lenAtStart.HasValue || !lenAfter2Sec.HasValue)
                    Assert.Fail("Nem található a következő frissítésig hátralevő időt jelző kitöltött téglalap. A problémát az is okozhatja, hogy a detailsPanel poziciomnálása nem " +
                        "megfelelő, pl. nincs a Dock tulajdonsága Top-ra állítva.");

                // Ez minden évben ugyanaz a tervek szerint, mehet a hallgatók felé is a visszajelzés
                Assert.IsTrue(Math.Abs(expectedProgress_Width_InPixels - lenAtStart.Value) < 15,
                    "A következő frissítésig hátralevő időt jelző kitöltött téglalap kezdeti hossza nem megfelelő. A problémát az is okozhatja, ha a tartalom megjelenítő nem " +
                    "egér duplakattintás eseményre, hanem új listaelem kiválasztás eseményre frissül, vagy ha a megjelenítés nem a (0,0) koordinátában kezdődik.");

                int dLen = lenAtStart.Value - lenAfter2Sec.Value;
                if (dLen == 0)
                    Assert.Fail("A következő frissítésig hátralevő időt jelző kitöltött téglalap hossza nem változik.");

                // TODO-BZ: Téglalap hossz változás (csak oktatói környezetben, plugizálás ellenőrzésre), bár ezt plugizálás nélkül is elronthatja!
                // Check if progress delta is about the expected value
                //var dLenExpected = expectedProgress_Width_InPixels * sleepIntervalMS / (parameters.Progress_IntervalSec * 1000);
                //Assert.IsTrue( Math.Abs(dLenExpected - dLen) < 10,
                //    "A következő frissítésig hátralevő időt jelző kitöltött téglalap hossza nem az előírt paramétereknek megfelelően változik.");
                
                // TODO-bz: progressbar magasság ellenőrzése (csak oktatói környezetben, plugizálás ellenőrzésre)

                // screenShot.SaveAsFile("screenshot1.png");
            }
            finally
            {
                session.Manage().Timeouts().ImplicitWait = DefaultSessionImplicitWaitSec;
            }
        }


        [TestMethod]
        public void TestContentIsUpdatedAfterTimeout()
        {
            string fileName = fileA;
            string filePath = Path.Combine(RootPath, fileName);
            OpenContentForFile(fileName);
            Wait(300);
            var editContent = assertElements.FileContentEdit();
            string fileContentTextOriginal = File.ReadAllText(filePath);
            Assert.AreEqual(fileContentTextOriginal, editContent.Text, "A többsoros szövegdoboz nem jeleníti meg a fájl tartalmát");

            try
            {
                string fileContentTextUpdated = fileContentTextOriginal + " appended text";
                File.WriteAllText(filePath, fileContentTextUpdated);

                Thread.Sleep(parametersForYears[Year.Current].Progress_IntervalSec * 1000 + 2000);
                const string errorMessage = "A többsoros szövegdoboz nem frissíti a fájl tartalmát az frissítési intervallum lejárta után";
                if (fileContentTextUpdated == string.Empty)
                    Assert.Fail(errorMessage);
                Assert.AreEqual(fileContentTextUpdated, editContent.Text, errorMessage);
            }
            finally
            {
                File.WriteAllText(filePath, fileContentTextOriginal);
            }
        }


        // TODO-BZ: jelezze, ha nem jó a szín (csak oktatói környezetben, plugizálás ellenőrzésre)


        // Returns null if no progressbar found
        int? getProgressBarLength(byte[] screenShotBytes, Point delatisPanelOffset)
        {
            // https://github.com/microsoft/WinAppDriver/blob/master/Tests/WebDriverAPI/Screenshot.cs
            using (MemoryStream msScreenshot = new MemoryStream(screenShotBytes))
            {
                // Verify that the element screenshot has a valid size
                Bitmap image = (Bitmap)Image.FromStream(msScreenshot);

       
                
                int x = delatisPanelOffset.X + 1;
                int y = delatisPanelOffset.Y + 2;
                int maxX = image.Width - 1;

                //image.Save("screenshot2.png", ImageFormat.Png);

                //Color c = image.GetPixel(x, y);
                //Color sc = Color.FromArgb(255, 0, 128, 0);
                //for (int i = 0; i < image.Width; i++)
                //    for (int j = 0; j < image.Height; j++)
                //    {
                //        Color c1 = image.GetPixel(i, j);
                //        if (c1 == sc)
                //            break;

                //    }

                int? xFound = null;
                Color backgroundColor;

                //// 1. try it with sampling a pixel that is probably of background color
                backgroundColor = image.GetPixel(delatisPanelOffset.X, delatisPanelOffset.Y + 20);
                int argb0 = backgroundColor.ToArgb();
                xFound = tryFindForBackgroundColor(image, backgroundColor, x, maxX, y);

                //backgroundColor = SystemColors.Control;
                //int argb1 = backgroundColor.ToArgb();
                //xFound = tryFindForBackgroundColor(image, backgroundColor, x, maxX, y);

                // 2. If not found, try to find with a typical fixed background color
                if (!xFound.HasValue)
                {
                    backgroundColor = Color.FromArgb(240, 240, 240);
                    xFound = tryFindForBackgroundColor(image, backgroundColor, x, maxX, y);

                }
                // 3. If not found, try to find with SystemColors.Control
                if (!xFound.HasValue)
                {
                    backgroundColor = SystemColors.Control;
                    xFound = tryFindForBackgroundColor(image, backgroundColor, x, maxX, y);

                }

                if (xFound.HasValue)
                    return xFound - delatisPanelOffset.X;
                else
                    return null;
            }
        }

        int? tryFindForBackgroundColor(Bitmap image, Color backgroundColor, int startX, int maxX, int y)
        {
            int x = startX;
            while (image.GetPixel(x, y).ToArgb() != backgroundColor.ToArgb()) // ToArgb is important to compare only the color (ARGB) members
            {
                if (x == maxX)
                    return null;
                x++;
            }
            return x;
        }

        double getDpiScaleFactor()
        {
            // Get with of File menu item
            var fileMenu = session.AssertFindElementByName("File", "menü");

            return (double)fileMenu.Size.Width / menuItemFileWidthIn96Dpi;

        }


        // Does not work
        //double getDpiScaleFactor()
        //{
        //    using (MemoryStream screenShot = new MemoryStream(session.GetScreenshot().AsByteArray))
        //    {
        //        // Verify that the element screenshot has a valid size
        //        var image = Image.FromStream(screenShot);
        //        return (double)image.Width / session.Manage().Window.Size.Width;
        //    }
        //}

        // Does not work, maybe need to set DPI awareness
        //(float dpiX, float dpiY) getDPI()
        //{

        //    var windowHandle = new IntPtr(Convert.ToInt32(session.CurrentWindowHandle, 16));
        //    // using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
        //    using (Graphics graphics = Graphics.FromHwnd(windowHandle))
        //    {
        //        float dpiX = graphics.DpiX;
        //        float dpiY = graphics.DpiY;
        //        return (dpiX, dpiY);
        //    }
        //}


        static void InitPath()
        {
            // Set path to a folder where we have some files
            using (InputDialog dlg = new InputDialog(session))
            {
                dlg.OpenDialog();
                dlg.SetEditText(RootPath);
                dlg.CloseWithOk();
            }
      
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
