using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;
using System.Xml.Linq;

namespace WpfApp3
{

    public partial class MainWindow : Window
    {
        public ObservableCollection<Period> period = null;

        public MainWindow()
        {
            InitializeComponent();

            try
            {
                period = new ObservableCollection<Period>();

                XDocument doc = XDocument.Load(@"C:\Penzin\CSharp\WpfApp3\WpfApp3\bin\Debug\net8.0-windows\Period.xml");
                var periods = doc.Element("periods").Elements("period");

                foreach (var p in periods)
                {
                    Period newPeriod = new Period
                    {
                        DateStart = DateTime.Parse(p.Element("dateStart").Value),
                        DateFinish = DateTime.Parse(p.Element("dateFinish").Value),
                        Type = int.Parse(p.Element("type").Value)
                    };

                    period.Add(newPeriod);
                }

                WPF.ItemsSource = period;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void AddLine(object sender, RoutedEventArgs e)
        {
            period.Add(new Period { DateStart = DateTime.Now, DateFinish = DateTime.Now, Type = 0 });
        }

        private void DeleteLine(object sender, RoutedEventArgs e)
        {
            if (WPF.SelectedItem != null && WPF.SelectedItem is Period selectedPeriod)
            {
                period.Remove(selectedPeriod);
            }
        }


        private void SaveXML(object sender, RoutedEventArgs e)
        {
            XDocument newDoc = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),
                new XElement("periods",
                    from p in period
                    select new XElement("period",
                        new XElement("dateStart", p.DateStart),
                        new XElement("dateFinish", p.DateFinish),
                        new XElement("type", p.Type)
                    )
                )
            );

            try
            {
                newDoc.Save(@"C:\Penzin\CSharp\WpfApp3\WpfApp3\bin\Debug\net8.0-windows\NewPeriod.xml");
                MessageBox.Show("Файл сохранен");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
    
}