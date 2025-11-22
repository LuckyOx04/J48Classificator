using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
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

        Dictionary<string, List<string>> trainingData = DataHelper.GetTrainingDataAsync("./training-data.csv").Result;
        foreach (var key in trainingData.Keys)
        {
            Console.WriteLine(key);
        }

        string classField = "windy";
        
        SolutionTreeBuilder solutionTreeBuilder = new SolutionTreeBuilder(trainingData, classField);
        SolutionTree solutionTree = solutionTreeBuilder.Build();
        Console.WriteLine(solutionTree);
        List<Dictionary<string, string>> testingData =  DataHelper.GetTestingDataAsync("./testing-data.csv").Result;
        InstancesClassifier instancesClassifier = new InstancesClassifier(testingData, classField, solutionTree);
        Console.WriteLine(instancesClassifier.GetConfusionMatrixAsString());
    }

    private async void TrainingFilePath_OnGotFocus(object? sender, GotFocusEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        
        if (topLevel == null)
        {
            Console.Error.WriteLine("Top level could not be found");
            return;
        }

        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open Training Data File",
            AllowMultiple = false,
            FileTypeFilter = [new FilePickerFileType("CSV")
            {
                Patterns = ["*.csv"],
                AppleUniformTypeIdentifiers = ["public.csv"],
                MimeTypes = ["text/csv"]
            }]
        });

        if (files.Count >= 1)
        {
            TrainingFilePath.Text = files[0].Path.ToString();
            
        }
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