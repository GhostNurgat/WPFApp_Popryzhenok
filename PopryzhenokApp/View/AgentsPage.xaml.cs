using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Entity.Infrastructure;
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
    using Extensions;
    using System.Data;

    public partial class AgentsPage : Page, INotifyPropertyChanged
    {
        private ObservableCollection<ProductSale> agents;
        public ObservableCollection<ProductSale> Agents
        {
            get => agents;
            set { agents = value; if (PropertyChanged != null) PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Agents")); }
        }

        public AgentsPage()
        {
            InitializeComponent();;
            Agents = Connection.Popryzhenok.ProductSale.ToObservableCollection();
            ItemsTypeAgent();
            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

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

                    AgentTypeCb.ItemsSource = ds.Tables["AgentType"].DefaultView;
                    AgentTypeCb.DisplayMemberPath = "Title";
                    AgentTypeCb.SelectedValuePath = "ID";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка подключения", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void GoToAddAgentPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddAgentPage());
        }

        private void SearchByEmail(object sender, TextChangedEventArgs e)
        {
            Agents = Connection.Popryzhenok.ProductSale
                .Where(s => s.Agent.Email.Contains(EmailTb.Text))
                .ToObservableCollection();
        }

        private void SearchByPhone(object sender, TextChangedEventArgs e)
        {
            Agents = Connection.Popryzhenok.ProductSale
                .Where(s => s.Agent.Phone.Contains(PhoneTb.Text))
                .ToObservableCollection();
        }

        private void DeleteAgent(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что вы хотите удалить агента?", "Удаление агента"
                , MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Button button = sender as Button;

                int id = (int)button.Tag;
                ProductSale productSale = Connection.Popryzhenok.ProductSale.Find(id);

                if (productSale.ProductCount > 0)
                {
                    MessageBox.Show("Невозможно удалить агента, т.к. имеется кол-во продуктов", "Удаление агента"
                        , MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                Agent agent = Connection.Popryzhenok.Agent.Find(productSale.AgentID);
                Agents.Remove(productSale);
                Connection.Popryzhenok.ProductSale.Remove(productSale);
                Connection.Popryzhenok.Agent.Remove(agent);

                try
                {
                    Connection.Popryzhenok.SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Удаление агента", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
