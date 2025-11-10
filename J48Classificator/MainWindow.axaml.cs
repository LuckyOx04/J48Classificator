using System;
using System.Collections.Generic;
using Avalonia.Controls;
using J48;

namespace J48Classificator;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        Dictionary<string, List<string>> data = DataHelper.CsvToDictionatyOfColumns("./temp-data.csv");
        foreach (var key in data.Keys)
        {
            Console.WriteLine(key);
        }

        TrainingAlgorithm.ClassField = "Play";
        TrainingAlgorithm.Data = data;
        Console.WriteLine(TrainingAlgorithm.GetInformationGainForField("Outlook"));
    }
}