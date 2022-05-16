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

    public partial class AddAgentPage : Page
    {
        public ProductSale newProductSale { get; set; }

        public AddAgentPage()
        {
            InitializeComponent();
            newProductSale = new ProductSale
            {
                SaleDate = DateTime.Now
            };
            ItemsAgent();
            ItemsProduct();
            DataContext = newProductSale;
        }

        private void ItemsAgent()
        {
            try
            {
                using (SqlConnection con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PopryzhenokDB;Integrated Security=True"))
                {
                    con.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Agent", con);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds, "Agent");

                    AgentsComboBox.ItemsSource = ds.Tables["Agent"].DefaultView;
                    AgentsComboBox.DisplayMemberPath = "Title";
                    AgentsComboBox.SelectedValuePath = "ID";
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
                using (SqlConnection con = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=PopryzhenokDB;Integrated Security=True"))
                {
                    con.Open();
                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Product", con);
                    DataSet ds = new DataSet();
                    adapter.Fill(ds, "Product");

                    ProductsComboBox.ItemsSource = ds.Tables["Product"].DefaultView;
                    ProductsComboBox.DisplayMemberPath = "Title";
                    ProductsComboBox.SelectedValuePath = "ID";
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка подключения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void GoToNewAgentWindow(object sender, RoutedEventArgs e)
        {
            NewAgentWindow agentWindow = new NewAgentWindow();
            agentWindow.ShowDialog();

            if (agentWindow.DialogResult == true)
            {
                MessageBox.Show("Данный успешно добавлено!", "Добавление нового агента",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.Refresh();
            }
        }

        private void AddSale(object sender, RoutedEventArgs e)
        {
            Connection.Popryzhenok.ProductSale.Add(newProductSale);

            try
            {
                Connection.Popryzhenok.SaveChanges();
                NavigationService.Navigate(new AgentsPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"При добавлении произошла ошибка: {ex.Message}", "Добавление агента",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
