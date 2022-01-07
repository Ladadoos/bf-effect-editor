// BattleForge Special Effect Editor
// Copyright(C) 2021 Skylords Reborn
// Project licensed under GNU General Public License v3.0. See LICENSE for more information.

using System.Windows;
using System.Windows.Input;

namespace BattleForgeEffectEditor.Application
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // This is required, because else the window becomes bigger than the screen
        // when maximizing the window.
        public void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            BorderThickness = WindowState == WindowState.Maximized ? new Thickness(8) : new Thickness(0);
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2) // Double click
                WindowState = WindowState != WindowState.Maximized ? WindowState.Maximized : WindowState.Normal;
            else if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void ButtonMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void ButtonMaximize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState != WindowState.Maximized ? WindowState.Maximized : WindowState.Normal;
        }

        private void ButtonClose_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
