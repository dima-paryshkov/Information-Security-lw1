using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace lw1_IS_2
{
    public partial class ChartForm : Form
    {
        public ChartForm(int[] input)
        {
            //создаем элемент Chart
            Chart myChart = new Chart();
            //кладем его на форму и растягиваем на все окно.
            myChart.Parent = this;
            myChart.Dock = DockStyle.Fill;
            //добавляем в Chart область для рисования графиков, их может быть
            //много, поэтому даем ей имя.
            myChart.ChartAreas.Add(new ChartArea("Math functions"));
            //Создаем и настраиваем набор точек для рисования графика, в том
            //не забыв указать имя области на которой хотим отобразить этот
            //набор точек.
            Series mySeriesOfPoint = new Series("Sinus");
            mySeriesOfPoint.ChartType = SeriesChartType.Line;
            mySeriesOfPoint.ChartArea = "Math functions";
            myChart.ChartAreas[0].AxisX.Title = "Number of round";
            myChart.ChartAreas[0].AxisY.Title = "Number of changed bit";
            for (int x = 0; x < 64; x++)
            {
                mySeriesOfPoint.Points.AddXY(x, input[x]);
            }
            //Добавляем созданный набор точек в Chart
            myChart.Series.Add(mySeriesOfPoint);
        }
    }
}
