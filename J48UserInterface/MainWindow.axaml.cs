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

        Dictionary<string, List<string>> data = DataHelper.GetTrainingData("./temp-data.csv");
        foreach (var key in data.Keys)
        {
            Console.WriteLine(key);
        }

        string classField = "outlook";
        
        SolutionTreeBuilder solutionTreeBuilder = new SolutionTreeBuilder(data, classField);
        SolutionTree solutionTree = solutionTreeBuilder.Build();
        solutionTree.PrintTreeDfs(solutionTree.Root, "");
        
    }
}