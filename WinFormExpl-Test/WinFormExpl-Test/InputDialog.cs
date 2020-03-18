﻿using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WinFormExpl_Test
{
    class InputDialog
    {
        WindowsDriver<WindowsElement> session;
        WindowsElement dialog;

        public InputDialog(WindowsDriver<WindowsElement> session)
        {
            this.session = session;
        }

        public WindowsElement OpenDialog()
        {
            var fileMenu = session.AssertFindElementByName("File", "menü");
            fileMenu.Click();
            var openMenu = session.AssertFindElementByName("Open", "menü");
            openMenu.Click();

            Thread.Sleep(500); // Wait for half second until the  dialog appears

            dialog = session.AssertFindElementByName("InputDialog", "dialógus ablak");
            return dialog;
        }
        public WindowsElement GetEdit()
        {
            return session.AssertFindElementByXPath("//Edit", "TextBox az útvonal bekéréshez");
        }

        public WindowsElement GetOkButton()
        {
            return session.AssertFindElementByAlternativeNames(new string[] { "Ok", "OK", "ok" }, "gomb");
        }

        public WindowsElement GetCancelButton()
        {
            return session.AssertFindElementByName("Cancel", "gomb");
        }

        public void SetEditText(string text)
        {
            var edit = GetEdit();
            text = SanitizeBackslashes(text);
            edit.SendKeys(text);
        }

        public string GetEditText()
        {
            var edit = GetEdit();
            return edit.Text;
        }

        public Size GetSize()
        {
            return dialog.Size;
        }

        public void CloseWithCancel()
        {
            var cancelButton = GetCancelButton();
            cancelButton.Click();
            Thread.Sleep(500);
            session.AssertElementNotFound("Cancel", "A Cancel gomb nem zárja be a dialógus ablakot.");
        }

        public void CloseWithOk()
        {
            var OklButton = GetOkButton();
            OklButton.Click();
            Thread.Sleep(500);
            session.AssertElementNotFound("Ok", "Az OK gomb nem zárja be a dialógus ablakot.");
        }

        static string SanitizeBackslashes(string input) => input.Replace("\\", Keys.Alt + Keys.NumberPad9 + Keys.NumberPad2 + Keys.Alt);

    }
}