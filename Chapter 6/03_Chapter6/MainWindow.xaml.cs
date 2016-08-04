using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
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

namespace _03_Chapter6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            //command definition
            var command = new EventCommand();
            ChangeValueCommand = command;

            //sequence initialization

            //register to the event from the command
            Observable.FromEventPattern(command, "ExecuteRaised")
            //subscribe to messages from the sequence
            .Subscribe(eventDetail =>
            {
        //EventArgs contains the Parameter of the command
        Result += Convert.ToInt32(eventDetail.EventArgs);
        //notify the value update
        Notify("Result");
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand ChangeValueCommand { get; set; }
        public int Result { get; set; }

        //value update notification
        void Notify([CallerMemberName] string name = null)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }

//a simplified CommandToEvent command
public class EventCommand : ICommand
{
    public event EventHandler<object> ExecuteRaised;
    public event EventHandler CanExecuteChanged;

    public bool CanExecute(object parameter)
    {
        return true;
    }

    public void Execute(object parameter)
    {
        if (ExecuteRaised != null)
            ExecuteRaised(this, parameter);
    }
}
}
