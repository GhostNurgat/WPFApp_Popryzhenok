using System;
using System.Collections.Generic;
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

namespace PopryzhenokApp.View
{
    using Models;
    using System.Data;
    using System.Data.SqlClient;
    using System.Data.Entity.Validation;
    using System.Reflection;
    using System.IO;
    using Microsoft.Win32;

    public partial class NewAgentWindow : Window
    {
        Agent newAgent { get; set; }

        public NewAgentWindow()
        {
            InitializeComponent();
            newAgent = new Agent();
            ItemsTypeAgent();
            DataContext = newAgent;
        }

        private void ItemsTypeAgent()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PopryzhenokDB;Integrated Security=True"))
                {
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM AgentType", conn);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds, "AgentType");

                    TypeAgentCb.ItemsSource = ds.Tables["AgentType"].DefaultView;
                    TypeAgentCb.DisplayMemberPath = "Title";
                    TypeAgentCb.SelectedValuePath = "ID";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка подключения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Load_Logo(object sender, RoutedEventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Лого агента (*.png, *.jpg, *.jpeg)|*.png;*.jpg;*.jpeg";
            string filePath = string.Empty;

            if (fileDialog.ShowDialog() == true)
                filePath = fileDialog.FileName;
            else
                return;

            string filename = Path.GetFileName(filePath);

            string exePath = Assembly.GetExecutingAssembly().Location;
            exePath = Path.GetDirectoryName(exePath);

            if (File.Exists($"{exePath}/agents/{filename}"))
            {
                int num = new Random().Next(1000);
                filename = $"{num}_{filename}";
            }

            File.Copy(filePath, $"{exePath}/agents/{filename}");
            newAgent.Logo = $"\\agents\\{filename}";

            MainImage.BeginInit();
            MainImage.Source = new BitmapImage(new Uri($"pack://siteoforigin:,,,/agents/{filename}"));
            MainImage.EndInit();
        }

        private void AddAgent(object sender, RoutedEventArgs e)
        {
            ErrorTextBox.Text = string.Empty;

            Connection.Popryzhenok.Agent.Add(newAgent);

            try
            {
                Connection.Popryzhenok.SaveChanges();
                DialogResult = true;
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityError in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityError.ValidationErrors)
                        ErrorTextBox.Text += validationError.ErrorMessage + "\n";
                }
            }
        }
    }
}
