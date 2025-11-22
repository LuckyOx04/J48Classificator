using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using J48Implementation;


namespace J48UserInterface;

public partial class SolutionTreeWindow : Window
{
    public SolutionTreeWindow(SolutionTree solutionTree)
    {
        InitializeComponent();

        SolutionTreeView.Text = solutionTree.ToString();
        Console.WriteLine(solutionTree);
    }

    private void CloseWindow_OnClick(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}