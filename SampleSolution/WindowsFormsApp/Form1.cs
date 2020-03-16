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
        int countdown = 150;
        FileInfo loadedFile = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void miOpen_Click(object sender, EventArgs e)
        {
            InputDialog dlg = new InputDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                string result = dlg.Path;
                MessageBox.Show(result);
                try
                {
                    lvFiles.Items.Clear();
                    DirectoryInfo di = new DirectoryInfo(result);
                    foreach (var fi in di.GetFiles())
                    {

                        ListViewItem lvi = new ListViewItem(new[] { fi.Name, fi.Length.ToString() });
                        lvFiles.Items.Add(lvi);
                        lvi.Tag = fi;
                    }
                }
                catch { }
            }
        }

        private void lvFiles_DoubleClick(object sender, EventArgs e)
        {
            if (lvFiles.SelectedItems.Count != 1) return;
            loadedFile = (FileInfo)lvFiles.SelectedItems[0].Tag;
            tContent.Text = File.ReadAllText(loadedFile.FullName);
            reloadTimer.Start();
            countdown = 100;
        }

        private void lvFiles_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvFiles.SelectedItems.Count != 1) return;
            FileInfo fi = (FileInfo)lvFiles.SelectedItems[0].Tag;
            lName.Text = fi.Name;
            lCreated.Text = fi.CreationTime.ToString();

        }

        private void reloadTimer_Tick(object sender, EventArgs e)
        {
            countdown--;
            detailsPanel.Invalidate();

            if (countdown <= 0)
            {
                countdown = 100;
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
    }
}
