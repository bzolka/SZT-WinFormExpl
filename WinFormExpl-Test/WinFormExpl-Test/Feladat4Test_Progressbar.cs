using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace WinFormExpl_Test
{
    [TestClass]
    public class Feladat4Test_Progressbar : AppSession
    {
        AssertElements assertElements = new AssertElements(session);

        // Make sure to update the Current item when introducing a new year
        enum Year  { Y2020, Y2019, Y2018, Y2017, Y2016, Current = Y2020 };

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
    
                //Thread.Sleep(200);

                var parameters = parametersForYears[Year.Current];
                int expectedProgress_Width_InPixels = (int)(parameters.Progress_Width_InPixels * dpiScale);

                // Instantly get progress length
                int? lenAtStart = getProgressBarLength(session.GetScreenshot().AsByteArray, delatisPanelOffset);
                int sleepIntervalMS = 2000;
                Thread.Sleep(sleepIntervalMS);
                int? lenAfter2Sec = getProgressBarLength(session.GetScreenshot().AsByteArray, delatisPanelOffset);

                if (!lenAtStart.HasValue || !lenAfter2Sec.HasValue)
                    Assert.Fail("Nem található a következő frissítésig hátralevő időt jelző kitöltött téglalap.");

                // TODO-BZ: Csak oktatói gépen (bár ezt plugizálás nélkül is elronthatja!)
                Assert.IsTrue(Math.Abs(expectedProgress_Width_InPixels - lenAtStart.Value) < 15,
                    "A következő frissítésig hátralevő időt jelző kitöltött téglalap kezdeti hossza nem megfelelő.");

                int dLen = lenAtStart.Value - lenAfter2Sec.Value;
                if (dLen == 0)
                    Assert.Fail("A következő frissítésig hátralevő időt jelző kitöltött téglalap hossza nem változik.");

                // TODO-BZ: Csak oktatói gépen (bár ezt plugizálás nélkül is elronthatja!)
                // Check if progress delta is about the expected value
                var dLenExpected = expectedProgress_Width_InPixels * sleepIntervalMS / (parameters.Progress_IntervalSec * 1000);
                Assert.IsTrue( Math.Abs(dLenExpected - dLen) < 10,
                    "A következő frissítésig hátralevő időt jelző kitöltött téglalap hossza nem az előírt paramétereknek megfelelően változik.");
                
                // TODO-bz: progressbar magasság ellenőrzése

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
            string filePath = Path.Combine(rootPath, fileName);
            OpenContentForFile(fileName);
            Thread.Sleep(300);
            var editContent = assertElements.FileContentEdit();
            string fileContentTextOriginal = File.ReadAllText(filePath);
            Assert.AreEqual(fileContentTextOriginal, editContent.Text, "A többsoros szövegdoboz nem jeleníti meg a fájl tartalmát");

            try
            {
                string fileContentTextUpdated = fileContentTextOriginal + " appended text";
                File.WriteAllText(filePath, fileContentTextUpdated);

                Thread.Sleep(parametersForYears[Year.Current].Progress_IntervalSec * 1000 + 2000);
                Assert.AreEqual(fileContentTextUpdated, editContent.Text, "A többsoros szövegdoboz nem frissíti a fájl tartalmát az frissítési intervallum lejárta után");
            }
            finally
            {
                File.WriteAllText(filePath, fileContentTextOriginal);
            }
        }


        // TODO-BZ: csak oktatói gépen, jelezze, ha nem jó a szín


        // Returns null if no progressbar found
        int? getProgressBarLength(byte[] screenShotBytes, Point delatisPanelOffset)
        {
            // https://github.com/microsoft/WinAppDriver/blob/master/Tests/WebDriverAPI/Screenshot.cs
            using (MemoryStream msScreenshot = new MemoryStream(screenShotBytes))
            {
                // Verify that the element screenshot has a valid size
                Bitmap image = (Bitmap)Image.FromStream(msScreenshot);

                Color backgroundColor = image.GetPixel(delatisPanelOffset.X, delatisPanelOffset.Y + 20);
                int x = delatisPanelOffset.X + 1;
                int y = delatisPanelOffset.Y + 2;
                int maxLen = image.Width;

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

                while (image.GetPixel(x, y) != backgroundColor)
                {
                    if (x == maxLen)
                        return null;
                    x++;
                }
                    
                return x - delatisPanelOffset.X;
            }
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
            InputDialog dlg = new InputDialog(session);
            dlg.OpenDialog();
            dlg.SetEditText(rootPath);
            dlg.CloseWithOk();
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
