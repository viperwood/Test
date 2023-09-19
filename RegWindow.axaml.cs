using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace Attempt4;

public partial class RegWindow : Window
{
    public RegWindow()
    {
        InitializeComponent();
        SigninButton.Click += Signin_Click;
    }

    private void Signin_Click(object? sender, RoutedEventArgs e)
    {
        MainWindow mainWindow = new MainWindow(KeyTextBox.Text == "0000");
        mainWindow.Show();
        Close();
    }

}