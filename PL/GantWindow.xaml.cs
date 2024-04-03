using BO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL;

/// <summary>
/// Interaction logic for GantWindow.xaml
/// </summary>
public partial class GantWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    public IEnumerable<GantTask>? ListGantTasks { get; set; }
    public DateTime StartDateColumn { get; set; }
    public DateTime CompleteDateColumn { get; set; }

    public GantWindow()
    {
        ListGantTasks = s_bl.CreateGantList();
        if (ListGantTasks is not null && ListGantTasks!.Count() != 0)
        {
            StartDateColumn = ListGantTasks!.First().StartDate;
            CompleteDateColumn = ListGantTasks!.Max(task => task.CompleteDate);
        }
        else
        {
            StartDateColumn = s_bl.Clock;
            CompleteDateColumn = s_bl.Clock;
        }
        InitializeComponent();
    }


    private string ChangeDependenciesToString(IEnumerable<int>? dependencies)
    {
        if (dependencies?.Count() == 0)
            return "";
        string str = "";
        foreach (var item in dependencies!)
            str += $" {item},";
        return str.Remove(str.Length - 1);
    }

    private void GantGrid_Initialized(object sender, EventArgs e)
    {
        DataGrid? dg = sender as DataGrid;

        DataTable dt = new DataTable();

        if (dg != null)
        {
            dg.Columns.Add(new DataGridTextColumn() { Header = "Id", Binding = new Binding("[0]") });
            dt.Columns.Add("Id", typeof(int));
            dg.Columns.Add(new DataGridTextColumn() { Header = "Alias", Binding = new Binding("[1]") });
            dt.Columns.Add("Alias", typeof(string));
            dg.Columns.Add(new DataGridTextColumn() { Header = "Worker Id", Binding = new Binding("[2]") });
            dt.Columns.Add("Worker Id", typeof(string));
            dg.Columns.Add(new DataGridTextColumn() { Header = "Worker Name", Binding = new Binding("[3]") });
            dt.Columns.Add("Worker Name", typeof(string));
            dg.Columns.Add(new DataGridTextColumn() { Header = "Task dependencies", Binding = new Binding("[4]") });
            dt.Columns.Add("Task dependencies", typeof(string));

            int column = 5;
            for (DateTime date = StartDateColumn.Date; date <= CompleteDateColumn; date = date.AddDays(1))
            {
                string strDate = $"{date.Day}/{date.Month}/{date.Year}";
                dg.Columns.Add(new DataGridTextColumn() { Header = strDate, Binding = new Binding($"[{column}]") });
                dt.Columns.Add(strDate, typeof(BO.PLStatus));
                column++;
            }

            if (ListGantTasks is not null)
            {
                foreach (BO.GantTask gant in ListGantTasks)
                {
                    DataRow row = dt.NewRow();
                    row[0] = gant.Id;
                    row[1] = gant.Alias;
                    row[2] = gant.WorkerId;
                    row[3] = gant.WorkerName;
                    row[4] = ChangeDependenciesToString(gant.DependentTasks);

                    int rows = 5;

                    for (DateTime date = StartDateColumn.Date; date <= CompleteDateColumn; date = date.AddDays(1))
                    {
                        if (date.Date < gant.StartDate.Date || date.Date > gant.CompleteDate.Date)
                            row[rows] = BO.PLStatus.All;
                        else
                        {
                            row[rows] = gant.Status;
                            if (s_bl.Task.InJeopardyCheck(gant.Id))
                                row[rows] = BO.PLStatus.InJeopardy;
                        }
                        rows++;
                    }
                    dt.Rows.Add(row);
                }
                if (dt is not null)
                {
                    dg.ItemsSource = dt.DefaultView;
                }
            }

        }
    }
}
