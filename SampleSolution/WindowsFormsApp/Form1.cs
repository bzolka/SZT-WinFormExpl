#define IMSc

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HaziWF
{
    public partial class Form1 : Form
    {
        const int counddownInit = 120;
        int countdown = counddownInit;
        const int refreshIntervalSec = 4;
        int counddownDelta;
        FileInfo loadedFile = null;

        public Form1()
        {
            InitializeComponent();
            counddownDelta = counddownInit / (refreshIntervalSec * 1000 / reloadTimer.Interval);
        }

        private void miOpen_Click(object sender, EventArgs e)
        {
            InputDialog dlg = new InputDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string result = dlg.Path;
                // MessageBox.Show(result);

                try
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(result);
                    setCurrentDir(dirInfo);
                }
                catch (Exception)
                { }

            }
        }

        private void lvFiles_DoubleClick(object sender, EventArgs e)
        {
            if (lvFiles.SelectedItems.Count != 1) return;

#if IMSc
            if (lvFiles.SelectedItems[0].Tag is DirectoryInfo di)
            {
                setCurrentDir(di);
                return;
            }
#endif

            loadedFile = (FileInfo)lvFiles.SelectedItems[0].Tag;
            tContent.Text = File.ReadAllText(loadedFile.FullName);
            reloadTimer.Start();
            countdown = counddownInit;
        }

        private void lvFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvFiles.SelectedItems.Count != 1) return;

            FileSystemInfo fi = (FileSystemInfo)lvFiles.SelectedItems[0].Tag;
            lName.Text = fi.Name;
            lCreated.Text = fi.CreationTime.ToString();
        }

        private void reloadTimer_Tick(object sender, EventArgs e)
        {
            countdown -= counddownDelta;
            detailsPanel.Invalidate();

            if (countdown <= 0)
            {
                countdown = counddownInit;
                tContent.Text = File.ReadAllText(loadedFile.FullName);
            }
        }

        private void detailsPanel_Paint(object sender, PaintEventArgs e)
        {
            if (loadedFile != null)
                e.Graphics.FillRectangle(Brushes.Green, 0, 0, countdown, 5);
        }

        private void miRun_Click(object sender, EventArgs e)
        {
            if (lvFiles.SelectedItems.Count != 1) return;
            string fullName = ((FileInfo)lvFiles.SelectedItems[0].Tag).FullName;
            if (fullName != null) Process.Start(fullName);
        }

        private void miExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        void setCurrentDir(DirectoryInfo dirInfo)
        {
            try
            {
                lvFiles.Items.Clear();

#if IMSc
                if (dirInfo.Parent != null)
                {
                    ListViewItem lvi = new ListViewItem(new[] { ".." }, "<DIR>");
                    lvFiles.Items.Add(lvi);
                    lvi.Tag = dirInfo.Parent;
                }

                foreach (var di in dirInfo.GetDirectories())
                {

                    ListViewItem lvi = new ListViewItem(new[] { di.Name }, "<DIR>");
                    lvFiles.Items.Add(lvi);
                    lvi.Tag = di;
                }
#endif
                foreach (var fi in dirInfo.GetFiles())
                {
                    ListViewItem lvi = new ListViewItem(new[] { fi.Name, fi.Length.ToString() });
                    lvFiles.Items.Add(lvi);
                    lvi.Tag = fi;
                }

            }
            catch { }
        }
    }
}
