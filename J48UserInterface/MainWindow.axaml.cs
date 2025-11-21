using System;
using System.Collections.Generic;
using Avalonia.Controls;
using J48Implementation;

namespace J48UserInterface;

public partial class MainWindow : Window
{
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
}