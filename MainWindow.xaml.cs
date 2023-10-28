using Microsoft.Win32;
using System;
using System.IO;
using System.Collections;
using System.Windows;
using System.ComponentModel;
using System.Threading;

namespace RandomNamePicker
{
    public partial class MainWindow : Window
    {
        private static BindHandler handler = new BindHandler();
        private static ArrayList Namelist = new ArrayList();
        private Thread roll = new Thread(NameRoll);
        private static Boolean Status = false;
        private Boolean ifLoad = false;
        private ArrayList HistoryList = new ArrayList();
        private static string Now;
        private Boolean ifstop = true;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = handler;
            this.Closing += programExit;
            roll.Start();
        }

        private void programExit(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Menu_Load(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            if (open.ShowDialog() == true)
            {
                string FilePath = open.FileName;
                try
                {
                    Namelist.Clear();
                    string[] names = File.ReadAllLines(FilePath);
                    foreach (string it in names)
                    {
                        Namelist.Add(it);
                    }
                    ifLoad = true;
                    MessageBox.Show("File Loaded!", "RandomNamePicker");
                    HistoryList.Clear();
                }
                catch (IOException ex)
                {
                    MessageBox.Show("Faild to load name list!", "Something Wrong!");
                }
            }
            else
            {
                MessageBox.Show("Faild to open file!", "Something Wrong!");
            }
        }

        private void Menu_Settings(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Feature Developing...", "Something Wrong!");
        }

        private void Menu_About(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Copyright 2023 @ Kira Mint\nLicense: Apache 2.0", "About");
        }

        private void Menu_Exit(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            if (ifLoad)
            {
                Status = true;
                ifstop = false;
            }
            else
            {
                MessageBox.Show("You have not load the namelist!", "Something Wrong!");
                Menu_Load(sender, e);
            }
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            if (ifstop)
            {
                return;
            }
            if (ifLoad)
            {
                Status = false;
                HistoryList.Add(handler.ShowName);
                string output = "History:\n";
                int i = 1;
                foreach (string it in HistoryList)
                {
                    output += i + ". " + it;
                    output += "\n";
                    i++;
                }
                handler.history = output;
                tb.ScrollToEnd();
                ifstop = true;
            }
            else
            {
                MessageBox.Show("You have not load the namelist!", "Something Wrong!");
                Menu_Load(sender, e);
            }
        }

        private void Pick_Click(object sender, RoutedEventArgs e)
        {
            if (ifLoad)
            {
                Random rand = new Random((int)DateTime.Now.Ticks);
                handler.ShowName = Namelist[rand.Next(0, Namelist.Count)].ToString();
                HistoryList.Add(handler.ShowName);
                string output = "History:\n";
                int i = 1;
                foreach (string it in HistoryList)
                {
                    output += i + ". " + it;
                    output += "\n";
                    i++;
                }
                handler.history = output;
                tb.ScrollToEnd();
            }
            else
            {
                MessageBox.Show("You have not load the namelist!", "Something Wrong!");
                Menu_Load(sender, e);
            }
        }

        private static void NameRoll()
        {
            int i = 0;
            while (true)
            {
                if (Status)
                {
                    if (i < Namelist.Count - 1)
                    {
                        i++;
                    }
                    else
                    {
                        i = 0;
                    }
                    Now = Namelist[i].ToString();
                    handler.ShowName = Now;
                }
                else
                {
                    continue;
                }
            }
        }
    }

    class BindHandler : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string _Showname = "Waiting";
        public string ShowName
        {
            get
            {
                return _Showname;
            }
            set
            {
                if (_Showname != value)
                {
                    _Showname = value;
                    OnPropertyChanged("ShowName");
                }
            }
        }
        public string _history = "History:\n";
        public string history
        {
            get
            {
                return _history;
            }
            set
            {
                if (_history != value)
                {
                    _history = value;
                    OnPropertyChanged("history");
                }
            }
        }
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
