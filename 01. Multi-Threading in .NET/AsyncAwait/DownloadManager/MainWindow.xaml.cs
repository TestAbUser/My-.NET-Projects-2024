﻿using System.Windows;
using DownloadManager.ViewModels;

namespace DownloadManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindowViewModel ViewModel { get; set; } = new MainWindowViewModel();
        public MainWindow()
        {
            InitializeComponent();
        }
    }
}