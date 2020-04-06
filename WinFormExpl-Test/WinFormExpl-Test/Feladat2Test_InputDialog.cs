using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Interactions;
using WinFormExpl_Test;

namespace WinFormExpl_Test1
{
    // TODO-BZ - mindenképpen be kell zárni a modális ablakokot, különben bezavar a többi tesztbe!
    [TestClass]
    public class Feladat2Test_InputDialog: AppSession
    {
        [TestMethod]
        public void TestCloseCancel()
        {
            using (InputDialog dlg = new InputDialog(session))
            {
                // Check if the dialog can be open
                dlg.OpenDialog();

                // Check if Cancel button exists and it closes the dialog
                dlg.CloseWithCancel();
            }
        }

        [TestMethod]
        public void TestCloseOk()
        {
            using (InputDialog dlg = new InputDialog(session))
            {
                // Check if the dialog can be open
                dlg.OpenDialog();
                // We had better not test with empty string, students may not think that it should be covered
                dlg.SetEditText(RootPath);
                dlg.CloseWithOk();
            }
        }

        [TestMethod]
        public void TestEdit()
        {
            using (InputDialog dlg = new InputDialog(session))
            {
                // Check if Textbox exists and we can type into it
                dlg.OpenDialog();
                dlg.SetEditText(RootPath);
                string dlgPath = dlg.GetEditText();
                Assert.AreEqual(RootPath, dlgPath);
            }
        }

        [TestMethod]
        public void TestAnchor()
        {
            using (InputDialog dlg = new InputDialog(session))
            {
                // Check if Textbox exists and we can type into it
                var rawDialog = dlg.OpenDialog();
                var edit = dlg.GetEdit();
                var ok = dlg.GetOkButton();
                var cancel = dlg.GetCancelButton();

                var dlgOriginalSize = rawDialog.Size;

                var editOriginalLocation = edit.Location;
                var okOriginalLocation = ok.Location;
                var cancelOriginalLocation = cancel.Location;

                var editOriginalSize = edit.Size;
                var okOriginalSize = ok.Size;
                var cancelOriginalSize = cancel.Size;

                var offset = new Size(20, 30);

                // This did not work, maybe it tries to set size for the main window
                // session.Manage().Window.Size = new Size(dlgSize.Width + offset.Width, dlgSize.Height + offset.Height);
                // session.Manage().Window.Maximize();
                // Maybe could use this somehow
                // var handle = session.CurrentWindowHandle;

                Actions action = new Actions(session);
                action.MoveToElement(rawDialog, dlgOriginalSize.Width, dlgOriginalSize.Height)
                    .ClickAndHold()
                    .MoveByOffset(offset.Width, offset.Height)
                    .Release()
                    .Perform();

                Wait(500); // Not sure if needed

                // This is important, the mouse drag could result a slightly different size change compared to mouse offset
                offset = rawDialog.Size - dlgOriginalSize;

                var editNewLocation = edit.Location;
                var okNewLocation = ok.Location;
                var cancelNewLocation = cancel.Location;

                var editNewSize = edit.Size;
                var okNewSize = ok.Size;
                var cancelNewSize = cancel.Size;

                // Check Edit textbox
                Assert.AreEqual(editOriginalLocation, editNewLocation,
                    "A path szövedgoboz nem megfelelően pozícionálódik az ablak átméretezésekor: csak a szélessége változhat, a helye nem." +
                    "(Esetleg az is problémát okozhat, hogy túl nagy az InputDialog ablak, próbáld kisebbre méretezni).");
                //Assert.AreEqual(editOriginalSize.Width + offset.Width, editNewSize.Width, // This +1 is required based on tests
                //    "A path szövedgoboz nem megfelelően méreteződik az ablak átméretezésekor");
                // Based on experiences, can be a bit different
                Assert.IsTrue(Math.Abs(editOriginalSize.Width + offset.Width - editNewSize.Width) <=2,
                     "A path szövedgoboz nem megfelelően méreteződik az ablak átméretezésekor");
                Assert.AreEqual(editOriginalSize.Height, editNewSize.Height,
                    "A path szövedgoboz nem megfelelően méreteződik az ablak átméretezésekor");

                // Check OK button
                Assert.AreEqual(okOriginalLocation.X, okNewLocation.X,
                   "Az OK gomb pozíciója (X koordináta) nem megfelelő az ablak átméretezésekor");
                Assert.IsTrue(Math.Abs(okOriginalLocation.Y + offset.Height - okNewLocation.Y) <= 2,
                    "Az OK gomb pozíciója (Y koordináta) nem megfelelő az ablak átméretezésekor");
                Assert.AreEqual(okOriginalSize, okNewSize,
                    "Az OK gomb mérete nem megfelelő az ablak átméretezésekor");

                // Check Cancel button
                Assert.IsTrue(Math.Abs(cancelOriginalLocation.X + offset.Width - cancelNewLocation.X) <= 2, // This +1 is required based on tests,
                    "A Cancel gomb pozíciója (X koordináta) nem megfelelő az ablak átméretezésekor");
                Assert.IsTrue(Math.Abs(cancelOriginalLocation.Y + offset.Height - cancelNewLocation.Y) <= 2,
                    "A Cancel gomb pozíciója (Y koordináta) nem megfelelő az ablak átméretezésekor");
                Assert.AreEqual(cancelOriginalSize, cancelNewSize,
                    "A Cancel gomb mérete nem megfelelő az ablak átméretezésekor");
            }
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
