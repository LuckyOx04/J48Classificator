using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace J48UserInterface;

public partial class SolutionTreeWindow : Window
{
    public SolutionTreeWindow()
    {
        InitializeComponent();
    }

    private void CloseWindow_OnClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}