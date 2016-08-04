using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _04_Chapter6
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            //sequence initialization

            //register to buttons
            var button1Sequence = Observable.FromEventPattern(button1, "Click")
            //create the message to specify right numeric value
            .Select(x => -10);

            var button2Sequence = Observable.FromEventPattern(button2, "Click")
            //create the message to specify right numeric value
            .Select(x => -1);

            var button3Sequence = Observable.FromEventPattern(button3, "Click")
            //create the message to specify right numeric value
            .Select(x => +1);

            var button4Sequence = Observable.FromEventPattern(button4, "Click")
            //create the message to specify right numeric value
            .Select(x => +10);

            //create a single merged sequence
            button1Sequence.Merge(button2Sequence).Merge(button3Sequence).Merge(button4Sequence)
            //flatten values into a single
            .Scan((previous, actual) => previous + actual)
            //subscribe to handle value change
            .Subscribe(x =>
            {
                //notify the value update
                textBox1.Text = x.ToString();
            });
        }
    }
}
