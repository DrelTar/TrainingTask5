using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApp2
{

    /// <summary>
    /// Class in which all fractals are drawn.
    /// </summary>
    public partial class FractalForm : Form
    {
        // Surely some of that can be removed.
        // Length decreasing coefficient. Must be less than 1 and more than 0.
        private double _lengthCoefficient;
        // Angle of left branch of tree. Counted in radians.
        private double _leftAngle;
        // Angle of right branch of tree. Counted in radians.
        private double _rightAngle;
        // Type of fractal. *wind* *carpet* *triangle* are available.
        private string _type;
        // Recursion level. Must be more than 0.
        private int _recursion;

        /// <summary>
        /// Recursion property. Maybe can be removed.
        /// </summary>
        public int Recursion
        {
            get
            {
                return _recursion;
            }
            set
            {
                _recursion = value;
            }
        }

        /// <summary>
        /// Initializing form.
        /// </summary>
        /// <param name="type"> Look _type comment. </param>
        /// <param name="recursion"> Look _recursion comment. </param>
        /// <param name="lengthCoefficient"> Look _lengthCoefficient comment. </param>
        /// <param name="leftAngle"> Look _leftAngle comment. </param>
        /// <param name="rightAngle"> Look _rightAngle comment.</param>
        public FractalForm(string type, int recursion, double lengthCoefficient, double leftAngle, double rightAngle)
        {
            InitializeComponent();
            _lengthCoefficient = lengthCoefficient;
            _leftAngle = leftAngle;
            _rightAngle = rightAngle;
            _recursion = recursion;
            _type = type;
        }

        /// <summary>
        /// Calculating type and drawing form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FractalForm_Paint(object sender, PaintEventArgs e)
        {
            switch (_type) 
            {
                case "wind":
                    WindFractal windFractal = new WindFractal(_recursion, this, e, _lengthCoefficient, _leftAngle, _rightAngle);
                    // Point and angle can be with any values. This is just one variant.
                    windFractal.DrawFractal(1, new Point(Size.Width / 2, Size.Height - 10), Math.PI / 2);
                    return;
                case "carpet":
                    CarpetFractal carpetFractal = new CarpetFractal(_recursion, e);
                    // Rectangle can be with any values. This is just one variant.
                    carpetFractal.DrawFractal(1, new Rectangle(Size.Width / 3, Size.Height / 3, Size.Width / 3, Size.Width / 3));
                    return;
                case "triangle":
                    TriangleFractal triangleFractal = new TriangleFractal(_recursion, this, e);
                    // Point can be with any values. This is just one variant.
                    triangleFractal.DrawFractal(1, new Point(Size.Height / 3, 2 * Size.Height / 3));
                    return;
            }   
        }

        /// <summary>
        /// Redraw form of resizing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FractalForm_Resize(object sender, EventArgs e)
        {
            this.Refresh();
        }

        /// <summary>
        /// Fractal abstract. Maybe can removed to class library. But I don't know how. 
        /// </summary>
        public abstract class Fractal
        {
            // Maximum recursion level. Must be higher than 0.
            private int _maximumRecursion = 20;
            // User recursion level. Must be under maximum and higher than 0.
            private int _recursion;
            // Just pen. Can be any color and width.
            private Pen _pen = new Pen(Color.Black, 2);
            // Paint event args for drawing.
            private PaintEventArgs _graphics;

            public int MaximumRecursion
            {
                get
                {
                    return _maximumRecursion;
                }
                set
                {
                    _maximumRecursion = value;
                }
            }

            public int Recursion
            {
                get
                {
                    return _recursion;
                }
                set
                {
                    // Checking if recursion level is okay.
                    if (MaximumRecursion >= value && value > 0)
                    {
                        _recursion = value;
                    }
                    else
                    {
                        ErrorMessage($"Wrong recursion level. {MaximumRecursion} > recursion level > 0");
                    }

                }
            }

            public Pen Pen
            {
                get
                {
                    return _pen;
                }
            }

            public PaintEventArgs Graphics
            {
                get
                {
                    return _graphics;
                }
                set
                {
                    _graphics = value;
                }
            }

            /// <summary>
            /// Abstract method for drawing wind tree fractal. Part of draw fracral method.
            /// </summary>
            /// <param name="currentRecursionLevel"> Start with 1. End with User recursion level. </param>
            /// <param name="currentPoint"> Current Point for drawing. </param>
            /// <param name="angle"> Angle of current line. </param>
            public virtual void DrawFractal(int currentRecursionLevel, Point currentPoint, double angle) { }

            /// <summary>
            /// Abstract method for drawing carpet fractal. Part of DrawFractal method.
            /// </summary>
            /// <param name="currentRecursionLevel"> Start with 1. End with User recursion level. </param>
            /// <param name="rectangle"> Current rectangle. Only squares are allowed. </param>
            public virtual void DrawFractal(int currentRecursionLevel, Rectangle rectangle) { }

            /// <summary>
            /// Abstract method for drawing triangle fractal. Part of DrawFractal method.
            /// </summary>
            /// <param name="currentRecursionLevel"> Start with 1. End with User recursion level. </param>
            /// <param name="currentPoint"> Current Point for drawing. </param>
            public virtual void DrawFractal(int currentRecursionLevel, Point currentPoint) { }

            /// <summary>
            /// Draw angled line.
            /// </summary>
            /// <param name="angle"> Radian from 0 to Pi. </param>
            /// <param name="length"> Length of line. </param>
            /// <param name="startPoint"> Starting point of line. </param>
            public void DrawAngledLine(double angle, int length, Point startPoint)
            {
                // I had some overflow exceptions during debuging, so maybe they can come again.
                try
                {
                    // Creating end point for line.
                    Point endPoint = new Point((int)(startPoint.X + Math.Cos(angle) * length),
                                               (int)(startPoint.Y - Math.Sin(angle) * length));
                    Graphics.Graphics.DrawLine(Pen, startPoint, endPoint);
                }
                catch (OverflowException exception)
                {
                    ErrorMessage(exception.Message);
                }
            }
        }

        /// <summary>
        /// WindFractal class for drawing wind tree fractal.
        /// </summary>
        public class WindFractal : Fractal
        {
            private double _lengthCoefficient;
            private double _leftAngle;
            private double _rightAngle;
            private int _length;

            /// <summary>
            /// Initializing fractal.
            /// </summary>
            /// <param name="recursion"> Recursion level. </param>
            /// <param name="form"> Used for size property. </param>
            /// <param name="graphics"> PaintEventArgs for drawing. </param>
            /// <param name="lengthCoefficient"> Length decreasing coefficient from 0 to 1. </param>
            /// <param name="leftAngle"> Left tree branch's angle. </param>
            /// <param name="rightAngle"> Right tree branch's angle. </param>
            public WindFractal(int recursion, Form form, PaintEventArgs graphics, double lengthCoefficient,
                               double leftAngle, double rightAngle)
            {
                Recursion = recursion;
                // Can be any, just example.
                _length = form.Size.Height / 4;
                Graphics = graphics;
                _lengthCoefficient = lengthCoefficient;
                _leftAngle = leftAngle;
                _rightAngle = rightAngle;
            }

            /// <summary>
            /// Recursive method drawing wind tree fractal.
            /// </summary>
            /// <param name="currentRecursion"> Current recursion level. </param>
            /// <param name="currentPoint"> Current point. </param>
            /// <param name="angle"> Current line angle. </param> 
            public override void DrawFractal(int currentRecursion, Point currentPoint, double angle)
            {
                // Checking if correct level.
                if (currentRecursion > Recursion)
                {
                    return;
                }
                // Drawing first line.
                if (currentRecursion == 1)
                {
                    Graphics.Graphics.DrawLine(Pen, currentPoint, new Point(currentPoint.X, currentPoint.Y - _length));
                    DrawFractal(currentRecursion + 1, new Point(currentPoint.X, currentPoint.Y - _length), Math.PI / 2);
                }
                else
                {
                    try
                    {
                        // Temporary length variable for calculating current length.
                        int tmpLength = (int)(_length * Math.Pow(_lengthCoefficient, currentRecursion - 1));
                        // Drawing two lines and step to next recursion level.
                        DrawAngledLine(angle - _rightAngle, tmpLength, currentPoint);
                        DrawAngledLine(angle + _leftAngle, tmpLength, currentPoint);
                        DrawFractal(currentRecursion + 1,
                                    new Point((int)(currentPoint.X + Math.Cos(angle - _rightAngle) * tmpLength),
                                              (int)(currentPoint.Y - Math.Sin(angle - _rightAngle) * tmpLength)),
                                              angle - _rightAngle);
                        DrawFractal(currentRecursion + 1,
                                    new Point((int)(currentPoint.X + Math.Cos(angle + _leftAngle) * tmpLength),
                                              (int)(currentPoint.Y - Math.Sin(angle + _leftAngle) * tmpLength)),
                                              angle + _leftAngle);
                    }
                    catch (ArithmeticException exception)
                    {
                        ErrorMessage(exception.Message);
                    }
                }
            }
        }

        /// <summary>
        /// CarpetFractal class for drawing Serpinsky carpet fractal.
        /// </summary>
        public class CarpetFractal : Fractal
        {
            /// <summary>
            /// Initializing class.
            /// </summary>
            /// <param name="recursion"> User recursion level. </param>
            /// <param name="graphics"> PaintEventArgs for drawing. </param>
            public CarpetFractal(int recursion, PaintEventArgs graphics)
            {
                // Maybe can be higher.
                MaximumRecursion = 10;
                Recursion = recursion;
                Graphics = graphics;
            }

            /// <summary>
            /// Draw carpet fractal.
            /// </summary>
            /// <param name="currentRecursion"> Current Recursion level. </param>
            /// <param name="rectangle"> Current square. </param>
            public override void DrawFractal(int currentRecursion, Rectangle rectangle)
            {
                // Checking if recursion level is ok.
                if (currentRecursion > Recursion)
                {
                    return;
                }
                // Drawing big blue square.
                if (currentRecursion == 1)
                {
                    Graphics.Graphics.FillRectangle(new SolidBrush(Color.Blue), rectangle);
                }
                try
                {
                    // Drawing small white square and try to draw 8 smaller white squares around. Looks bad, feels exactly the same.
                    Graphics.Graphics.FillRectangle(new SolidBrush(Color.White), rectangle.X + rectangle.Width / 3,
                                                    rectangle.Y + rectangle.Height / 3, rectangle.Width / 3, rectangle.Height / 3);
                    DrawFractal(currentRecursion + 1, new Rectangle(rectangle.X, rectangle.Y,
                                                                    rectangle.Width / 3, rectangle.Height / 3));
                    DrawFractal(currentRecursion + 1, new Rectangle(rectangle.X + rectangle.Width / 3, rectangle.Y,
                                                                    rectangle.Width / 3, rectangle.Height / 3));
                    DrawFractal(currentRecursion + 1, new Rectangle(rectangle.X + 2 * rectangle.Width / 3, rectangle.Y,
                                                                    rectangle.Width / 3, rectangle.Height / 3));
                    DrawFractal(currentRecursion + 1, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height / 3,
                                                                    rectangle.Width / 3, rectangle.Height / 3));
                    DrawFractal(currentRecursion + 1, new Rectangle(rectangle.X + 2 * rectangle.Width / 3,
                                                                    rectangle.Y + rectangle.Height / 3,
                                                                    rectangle.Width / 3, rectangle.Height / 3));
                    DrawFractal(currentRecursion + 1, new Rectangle(rectangle.X, rectangle.Y + 2 * rectangle.Height / 3,
                                                                    rectangle.Width / 3, rectangle.Height / 3));
                    DrawFractal(currentRecursion + 1, new Rectangle(rectangle.X + rectangle.Width / 3,
                                                                    rectangle.Y + 2 * rectangle.Height / 3,
                                                                    rectangle.Width / 3, rectangle.Height / 3));
                    DrawFractal(currentRecursion + 1, new Rectangle(rectangle.X + 2 * rectangle.Width / 3,
                                                                    rectangle.Y + 2 * rectangle.Height / 3,
                                                                    rectangle.Width / 3, rectangle.Height / 3));
                }
                catch(ArithmeticException exception)
                {
                    ErrorMessage(exception.Message);
                }
            }
        }

        /// <summary>
        /// TriangleFractal class for drawing Serpinsky triangle fractal. 
        /// </summary>
        public class TriangleFractal : Fractal
        {
            private int _length;

            /// <summary>
            /// Initializing class.
            /// </summary>
            /// <param name="recursion"> User recursion level. </param>
            /// <param name="form"> For size only. Surely I can use only size but that was not my first idea. </param>
            /// <param name="graphics"> PaintEventArgs for drawing. </param>
            public TriangleFractal(int recursion, Form form, PaintEventArgs graphics)
            {
                // Higher recursion level, higher gap between triangles.
                MaximumRecursion = 10;
                // Can be any.
                _length = form.Size.Height / 3;
                Recursion = recursion;
                Graphics = graphics;
            }

            /// <summary>
            /// Drawing triangle fractal.
            /// </summary>
            /// <param name="currentRecursion"> Current Recursion level. </param>
            /// <param name="currentPoint"> Current the most left point of current triangle. </param>
            public override void DrawFractal(int currentRecursion, Point currentPoint)
            {
                // Checking if reursion level is ok.
                if (currentRecursion > Recursion)
                {
                    return;
                }
                if (currentRecursion == 1)
                {
                    // Drawing triangle look at the top. Surely can be done with DrawAngledLine method. 
                    // But I have only 20 mins until dealine.
                    Graphics.Graphics.DrawLine(Pen, currentPoint, new Point(currentPoint.X + _length, currentPoint.Y));
                    Graphics.Graphics.DrawLine(Pen, new Point(currentPoint.X + _length, currentPoint.Y),
                                                    new Point(currentPoint.X + _length / 2,
                                                         currentPoint.Y - (int)(Math.Sin(Math.PI / 3) * _length)));
                    Graphics.Graphics.DrawLine(Pen, new Point(currentPoint.X + _length / 2,
                                                              currentPoint.Y - (int)(Math.Sin(Math.PI / 3) * _length)), currentPoint);
                    DrawFractal(currentRecursion + 1, new Point(currentPoint.X + (int)(Math.Cos(Math.PI / 3) * _length / 2),
                                                                currentPoint.Y - (int)(Math.Sin(Math.PI / 3) * _length / 2)));
                }
                else
                {
                    try {
                        // Temporary length variable for calculating decreasing of triangle side.
                        int tmpLength = (int)(_length * Math.Pow(0.5, currentRecursion - 1));
                        // Drawing trianlge which looks at the bottom.
                        Graphics.Graphics.DrawLine(Pen, currentPoint, new Point(currentPoint.X + tmpLength, currentPoint.Y));
                        Graphics.Graphics.DrawLine(Pen, new Point(currentPoint.X + tmpLength, currentPoint.Y),
                                                   new Point(currentPoint.X + tmpLength / 2,
                                                             currentPoint.Y + (int)(Math.Sin(Math.PI / 3) * tmpLength)));
                        Graphics.Graphics.DrawLine(Pen, new Point(currentPoint.X + tmpLength / 2,
                                                                  currentPoint.Y + (int)(Math.Sin(Math.PI / 3) * tmpLength)),
                                                   currentPoint);
                        // I have no idea why, but with +1 it looks much pretier than without.
                        DrawFractal(currentRecursion + 1, new Point(currentPoint.X + (int)(Math.Cos(Math.PI / 3) * tmpLength / 2 + 1),
                                                                    currentPoint.Y - (int)(Math.Sin(Math.PI / 3) * tmpLength / 2)));
                        DrawFractal(currentRecursion + 1, new Point(currentPoint.X - (int)(Math.Cos(Math.PI / 3) * tmpLength / 2),
                                                                    currentPoint.Y + (int)(Math.Sin(Math.PI / 3) * tmpLength / 2)));
                        DrawFractal(currentRecursion + 1, new Point(currentPoint.X - (int)(Math.Cos(Math.PI / 3) * tmpLength / 2)
                                                                                   + tmpLength,
                                                                    currentPoint.Y + (int)(Math.Sin(Math.PI / 3) * tmpLength / 2)));
                    }
                    catch (ArithmeticException exception)
                    {
                        ErrorMessage(exception.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Creating erro form.
        /// </summary>
        /// <param name="error"> String error message. </param>
        public static void ErrorMessage(string error)
        {
            ErrorForm form = new ErrorForm(error);
            form.Show();
        }
    }
}
