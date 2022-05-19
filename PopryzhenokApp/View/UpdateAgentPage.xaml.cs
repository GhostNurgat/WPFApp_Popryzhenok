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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PopryzhenokApp.View
{
    using Models;
    using System.Data;
    using System.Data.Entity.Infrastructure;
    using System.Data.SqlClient;
    using System.IO;
    using Microsoft.Win32;
    using System.Reflection;
    using System.Data.Entity.Validation;

    public partial class UpdateAgentPage : Page
    {
        public Agent Agent { get; set; }
        public ProductSale Sale { get; set; }

        public UpdateAgentPage(ProductSale productSale)
        {
            InitializeComponent();
            Agent = Connection.Popryzhenok.Agent.Find(productSale.AgentID);
            Sale = productSale;
            ItemsTypeAgent();
            ItemsProduct();
            DataContext = Sale;
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

                    TypeCb.ItemsSource = ds.Tables["AgentType"].DefaultView;
                    TypeCb.DisplayMemberPath = "Title";
                    TypeCb.SelectedValuePath = "ID";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка подключения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ItemsProduct()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PopryzhenokDB;Integrated Security=True"))
                {
                    conn.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Product", conn);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds, "Product");

                    ProductCb.ItemsSource = ds.Tables["Product"].DefaultView;
                    ProductCb.DisplayMemberPath = "Title";
                    ProductCb.SelectedValuePath = "ID";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка подключения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadLogo(object sender, RoutedEventArgs e)
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
            Agent.Logo = $"\\agents\\{filename}";

            MainImage.BeginInit();
            MainImage.Source = new BitmapImage(new Uri($"pack://siteoforigin:,,,/agents/{filename}"));
            MainImage.EndInit();
        }

        private void SaveAgent(object sender, RoutedEventArgs e)
        {
            TbErrors.Text = string.Empty;

            try
            {
                Connection.Popryzhenok.SaveChanges();
                NavigationService.Refresh();
                MessageBox.Show("Данный успешно сохранён", "Редактирование агента",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityError in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityError.ValidationErrors)
                    {
                        TbErrors.Text += validationError.ErrorMessage + "\n";
                    }
                }
            }
        }

        private void SaveChanges(object sender, RoutedEventArgs e)
        {
            try
            {
                Connection.Popryzhenok.SaveChanges();
                NavigationService.Navigate(new AgentsPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"При сохранении произошла ошибка: {ex.Message}", "Редактирование агента"
                    , MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GoBackAgents(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
