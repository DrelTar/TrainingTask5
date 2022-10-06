using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// I have done only 3 of 5 fractals. And no extra functions. That's sad. But at the end it's not the last peergrade.

namespace WindowsFormsApp2
{
    /// <summary>
    /// Main form which get and calculate all user input.
    /// </summary>
    public partial class MainForm : Form
    {
        // Current active form with fractal.
        private FractalForm _form;

        /// <summary>
        /// Initialize form.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            Size size = this.ClientSize;
        }

        /// <summary>
        /// Redraw form on resizing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Resize(object sender, EventArgs e)
        {
            this.Refresh();
        }

        /// <summary>
        /// Create Windtree Fractal form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindSubmit_Click(object sender, EventArgs e)
        {
            // Temporary variables for TryParse stuff.
            double tmpCoefficient;
            int tmpRecursion;
            // Close existing form.
            if (_form != null)
            {
                _form.Close();
            }
            // Checking if coefficient okay.
            if (double.TryParse(textBox1.Text, out tmpCoefficient) && tmpCoefficient < 1 && tmpCoefficient > 0)
            {
                // Checking if recursion level is more than 0. Checking if it is less than Maximum recursion level will be later.
                if (int.TryParse(textBox4.Text, out tmpRecursion) && tmpRecursion > 0)
                {
                    try
                    {
                        // Formula transform degrees to radians and make sure it is lesser than PI.
                        _form = new FractalForm("wind", tmpRecursion, tmpCoefficient,
                                                (double.Parse(textBox2.Text) % 180) * Math.PI / 180,
                                                (double.Parse(textBox3.Text) % 180) * Math.PI / 180);
                        _form.Show();
                    }
                    catch (FormatException exception)
                    {
                        ErrorMessage(exception.Message);
                    }
                }
                else
                {
                    ErrorMessage("Wrong recursion level. recursion level > 0");
                }
            }
            else
            {
                ErrorMessage("Wrong coefficient. 1 > coefficient > 0 ");
            }
        }

        /// <summary>
        /// Monitore recursion level change and apply changings.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Recursion_TextChanged(object sender, EventArgs e)
        {
            // Temporary variable for TryParse stuff.
            int tmpRecursion;
            // Check if reucrsion level is more than 0.
            if (int.TryParse(textBox4.Text, out tmpRecursion) && tmpRecursion > 0)
            {
                // Checking if form exist.
                if (_form != null)
                {
                    _form.Recursion = tmpRecursion;
                    _form.Refresh();
                }
                else
                {
                    ErrorMessage("Form is not exist");
                }
            }
            else
            {
                ErrorMessage("Wrong recursion level. recursion level > 0");
            }
        }

        /// <summary>
        /// Create Serpinsky Carpet fractal form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CarpetSubmit_Click(object sender, EventArgs e)
        {
            // Checking if form exists
            if (_form != null)
            {
                _form.Close();
            }
            int tmpRecursion;
            // Checking if recursion level is more than 0.
            if (int.TryParse(textBox4.Text, out tmpRecursion) && tmpRecursion > 0)
            {
                // Three 0, cause I've no idea, how to do it other way.
                _form = new FractalForm("carpet", tmpRecursion, 0, 0, 0);
                _form.Show();
            }
            else
            {
                ErrorMessage("Wrong recursion level. recursion level > 0");
            }
        }

        /// <summary>
        /// Create Serpinsky Triangle Fractal form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TriangleSubmit_Click(object sender, EventArgs e)
        {
            // Checking if form exists.
            if (_form != null)
            {
                _form.Close();
            }
            int tmpRecursion;
            // Checking if recursion level is more than 0.
            if (int.TryParse(textBox4.Text, out tmpRecursion) && tmpRecursion > 0)
            {
                // Same as in CarpetSubmit. I'm sure there is variant to do it but I haven't found it.
                _form = new FractalForm("triangle", tmpRecursion, 0, 0, 0);
                _form.Show();
            }
            else
            {
                ErrorMessage("Wrong recursion level. recursion level > 0");
            }
        }

        /// <summary>
        /// Create error form.
        /// </summary>
        /// <param name="error"> String with error explanation. </param>
        public void ErrorMessage(string error)
        {
            ErrorForm form = new ErrorForm(error);
            form.Show();
        }
    }
}
