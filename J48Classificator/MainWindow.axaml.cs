using Avalonia.Controls;
using J48;

namespace J48Classificator;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        DataHelper.CsvToDictionatyOfColumns("./temp-data.csv");
    }
}