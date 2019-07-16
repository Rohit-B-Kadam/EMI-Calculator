using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace EMI_Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel vm;
        public MainWindow()
        {
            InitializeComponent();
            vm = new MainViewModel();
            this.Loaded += (s, e) => DataContext = vm;
            
        }

        private void Submit(object sender, RoutedEventArgs e)
        {
            if(vm.isNumber(TxtLoanAmount.Text) && vm.isNumber(TxtDuration.Text) )
                vm.CalculateEMI();
                
        }

    }

    public class MainViewModel : INotifyPropertyChanged
    {
        //Important
        private void Notify(string propName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        //Loan Amount
        private double _loanAmount;
        public double LoanAmount
        {
            get { return _loanAmount; }
            set
            {
                _loanAmount = value;
                Notify("LoanAmount");
            }
        }


        // Duration
        private int _duration;
        public int Duration
        {
            get { return _duration; }
            set
            {
                _duration = value;
                Notify("Duration");
            }
        }

        //ROI
        private double _ROI;
        public double ROI
        {
            get { return _ROI; }
            set
            {
                _ROI = value;
                Notify("ROI");
            }
        }

        //EMI

        private double _EMI;
        public double EMI
        {
            get { return _EMI; }
            set
            {
                _EMI = value;
                Notify("EMI");
            }
        }

        public Dispatcher _current;
        public Thread thread;

        public ClientSocket socket;

        public MainViewModel()
        {
            ROI = 1.0;
            EMI = 0.0;
            _current = Dispatcher.CurrentDispatcher;
            socket = new ClientSocket();
            ConnetToServer();
            GetROI();
        }


        public void CalculateEMI()
        {
            EMI = (LoanAmount * Duration * ROI) / 100;
        }

        public bool isNumber(string numString)
        {
            long number = 0;
            bool isNum = long.TryParse(numString, out number);

            if (isNum)
                return true;
            else
                return false;
        }
        
        public void ConnetToServer()
        {
            if (socket.EstablishedConnection())
            {
                // all ok
            }
            else
            {
                //Exception
                MessageBox.Show("Server is not started");
                Environment.Exit(0);
            }
        }

        public void GetROI()
        {
            socket.SendMessage("GiveROI");

            ROI = Convert.ToDouble(socket.ReceiveMessage(256));
        }
    }
}
