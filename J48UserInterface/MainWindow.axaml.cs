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

        Dictionary<string, List<string>> data = DataHelper.CsvToDictionatyOfColumns("./temp-data.csv");
        foreach (var key in data.Keys)
        {
            Console.WriteLine(key);
        }

        FieldEntropyAlgorithm.ClassField = "Play";
        FieldEntropyAlgorithm.Data = data;
        Console.WriteLine(FieldEntropyAlgorithm.GetInformationGainForField("Windy"));
    }
}