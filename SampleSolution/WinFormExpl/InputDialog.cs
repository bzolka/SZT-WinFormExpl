using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HaziWF
{
    public partial class InputDialog : Form
    {

        public string Path
        {
            get { return tPath.Text; }
            set { tPath.Text = value; }
        }
        public InputDialog()
        {
            InitializeComponent();
        }

    }
}
