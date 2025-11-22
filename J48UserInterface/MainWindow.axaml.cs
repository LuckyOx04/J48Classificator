using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using J48Implementation;

namespace J48UserInterface;

public partial class MainWindow : Window
{
    private Dictionary<string, List<string>> _trainingData;
    private List<Dictionary<string, string>> _testingData;
    private string _classField;
    private SolutionTree _solutionTree;
    private int _fieldsSelected = 0;
    
    public MainWindow()
    {
        InitializeComponent();

        // Dictionary<string, List<string>> trainingData = DataHelper.GetTrainingDataAsync("./training-data.csv").Result;
        // foreach (var key in trainingData.Keys)
        // {
        //     Console.WriteLine(key);
        // }
        //
        // string classField = "windy";
        //
        // SolutionTreeBuilder solutionTreeBuilder = new SolutionTreeBuilder(trainingData, classField);
        // _solutionTree solutionTree = solutionTreeBuilder.Build();
        // Console.WriteLine(solutionTree);
        // List<Dictionary<string, string>> testingData =  DataHelper.GetTestingDataAsync("./testing-data.csv").Result;
        // InstancesClassifier instancesClassifier = new InstancesClassifier(testingData, classField, solutionTree);
        // Console.WriteLine(instancesClassifier.GetConfusionMatrixAsString());
    }

    private async void TrainingFilePath_OnGotFocus(object? sender, GotFocusEventArgs e)
    {
        _fieldsSelected = _fieldsSelected != 0 ? _fieldsSelected-- : 0;
        
        var topLevel = TopLevel.GetTopLevel(this);
        
        if (topLevel == null)
        {
            await Console.Error.WriteLineAsync("Top level could not be found");
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
            string uri = files[0].TryGetLocalPath();
            TrainingFilePath.Text = uri;
            this._trainingData = await DataHelper.GetTrainingDataAsync(uri);
            SelectClassField.ItemsSource = _trainingData.Keys;
            SelectClassField.IsEnabled = true;
            if (++_fieldsSelected == 3)
            {
                Classify.IsEnabled = true;
            }
        }
    }

    private async void TestingFilePath_OnGotFocus(object? sender, GotFocusEventArgs e)
    {
        _fieldsSelected = _fieldsSelected != 0 ? _fieldsSelected-- : 0;
        
        var topLevel = TopLevel.GetTopLevel(this);
        
        if (topLevel == null)
        {
            await Console.Error.WriteLineAsync("Top level could not be found");
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
            string uri = files[0].TryGetLocalPath();
            TestingFilePath.Text = uri;
            this._testingData = await DataHelper.GetTestingDataAsync(uri);
            if (++_fieldsSelected == 3)
            {
                Classify.IsEnabled = true;
            }
        }
    }

    private void OpenSolutionTree_OnClick(object? sender, RoutedEventArgs e)
    {
        SolutionTreeWindow solutionTreeWindow = new SolutionTreeWindow(_solutionTree);
        solutionTreeWindow.Show();
    }


    private async void Classify_OnClick(object? sender, RoutedEventArgs e)
    {
        this._solutionTree = await Task.Run(() => new SolutionTreeBuilder(this._trainingData, this._classField)
            .Build());
        string classificationResult = await Task.Run(() => new InstancesClassifier(this._testingData,
            this._classField, _solutionTree).GetConfusionMatrixAsString());
        ResultText.Text = classificationResult;
        OpenSolutionTree.IsEnabled = true;
    }

    private void SelectClassField_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        _fieldsSelected = _fieldsSelected != 0 ? _fieldsSelected-- : 0;
        _classField = SelectClassField.SelectedValue?.ToString();
        if (++_fieldsSelected == 3)
        {
            Classify.IsEnabled = true;
        }
    }
}