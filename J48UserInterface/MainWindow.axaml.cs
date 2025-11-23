using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using J48Implementation;

namespace J48UserInterface;

public partial class MainWindow : Window
{
    private Dictionary<string, List<string>>? _trainingData;
    private List<Dictionary<string, string>>? _testingData;
    private string? _classField;
    private SolutionTree? _solutionTree;
    private int _fieldsSelected;
    
    public MainWindow()
    {
        InitializeComponent();
    }

    private async void TrainingFilePath_OnGotFocus(object? sender, GotFocusEventArgs e)
    {
        _fieldsSelected = _fieldsSelected != 0 ? _fieldsSelected-- : 0;
        
        var topLevel = GetTopLevel(this);
        
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
            string? uri = files[0].TryGetLocalPath();
            if (uri != null)
            {
                TrainingFilePath.Text = uri;
                _trainingData = await DataHelper.GetTrainingDataAsync(uri);
                SelectClassField.ItemsSource = _trainingData.Keys;
                SelectClassField.IsEnabled = true;
                if (++_fieldsSelected == 3)
                {
                    Classify.IsEnabled = true;
                }
            }
            else
            {
                TrainingFilePath.Text = "Training file uri not found!";
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
            string? uri = files[0].TryGetLocalPath();
            if (uri != null)
            {
                TestingFilePath.Text = uri;
                this._testingData = await DataHelper.GetTestingDataAsync(uri);
                if (++_fieldsSelected == 3)
                {
                    Classify.IsEnabled = true;
                }
            }
            else
            {
                TrainingFilePath.Text = "Training file uri not found!";
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
        if (_classField != null && _trainingData != null && _testingData != null)
        {
            _solutionTree = await Task.Run(() => new SolutionTreeBuilder(_trainingData, _classField)
                .Build());
            string classificationResult = await Task.Run(() => new InstancesClassifier(_testingData,
                _classField, _solutionTree).GetConfusionMatrixAsString());
            ResultText.Text = classificationResult;
            OpenSolutionTree.IsEnabled = true;
        }
        else
        {
            ResultText.Text = "Training data, testing data or class field is not set!";
        }
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