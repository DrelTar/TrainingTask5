using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class ErrorForm : Form
    {
        public ErrorForm(string error)
        {
            InitializeComponent();
            // Wiriting error message.
            label1.Text = error;
            Refresh();
        }
    }
}
