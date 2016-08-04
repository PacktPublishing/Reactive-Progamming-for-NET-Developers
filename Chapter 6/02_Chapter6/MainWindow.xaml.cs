using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace _02_Chapter6
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            //command definition
            ChangeValueCommand = new RoutedCommand(Guid.NewGuid().ToString(), typeof(MainWindow));
            //command binding registration
            CommandBindings.Add(new CommandBinding(ChangeValueCommand, OnChangeValueCommand));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        //classic WPF implementation
        private void OnChangeValueCommand(object sender, ExecutedRoutedEventArgs e)
        {
            Result += Convert.ToInt32(e.Parameter);
            //notify value update
            Notify("Result");
        }

        public ICommand ChangeValueCommand { get; set; }
        public int Result { get; set; }

        //value update notification
        void Notify([CallerMemberName] string name = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
