using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using J48Implementation;

namespace J48UserInterface;

public partial class MainWindow : Window
{
    private Dictionary<string, Dictionary<string, string>> _trainingData;
    private List<Dictionary<string, string>> _testingData;
    private string _classField;
    private List<string> _dataHeaders;
    private SolutionTreeBuilder _solutionTreeBuilder;
    public MainWindow()
    {
        InitializeComponent();

        Dictionary<string, List<string>> trainingData = DataHelper.GetTrainingData("./training-data.csv");
        foreach (var key in trainingData.Keys)
        {
            Console.WriteLine(key);
        }

        string classField = "windy";
        
        SolutionTreeBuilder solutionTreeBuilder = new SolutionTreeBuilder(trainingData, classField);
        SolutionTree solutionTree = solutionTreeBuilder.Build();
        Console.WriteLine(solutionTree);
        List<Dictionary<string, string>> testingData =  DataHelper.GetTestingData("./testing-data.csv");
        InstancesClassifier instancesClassifier = new InstancesClassifier(testingData, classField, solutionTree);
        Console.WriteLine(instancesClassifier.GetConfusionMatrixAsString());
    }

    private void TrainingFilePath_OnGotFocus(object? sender, GotFocusEventArgs e)
    {
        Console.WriteLine($"{sender?.GetType().Name} was pressed");
    }

    private void TestingFilePath_OnGotFocus(object? sender, GotFocusEventArgs e)
    {
        Console.WriteLine($"{sender?.GetType().Name} was pressed");
    }

    private void SelectClassField_OnClick(object? sender, RoutedEventArgs e)
    {
        Console.WriteLine($"{sender?.GetType().Name} was pressed");
    }

    private void OpenSolutionTree_OnClick(object? sender, RoutedEventArgs e)
    {
        SolutionTreeWindow solutionTreeWindow = new SolutionTreeWindow();
        solutionTreeWindow.Show();
    }


    private void Classify_OnClick(object? sender, RoutedEventArgs e)
    {
        
    }
}