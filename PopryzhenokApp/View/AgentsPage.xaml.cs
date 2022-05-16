using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    public partial class AgentsPage : Page, INotifyPropertyChanged
    {
        private ObservableCollection<ProductSale> agents;
        public ObservableCollection<ProductSale> Agents
        {
            get => agents;
            set { agents = value; if (PropertyChanged != null) PropertyChanged.Invoke(this, new PropertyChangedEventArgs("Agents")); }
        }

        int CurrentPageIndex = 1;
        int ItemPerPage = 10;
        int totalPage = 0;

        public AgentsPage()
        {
            InitializeComponent();
            Agents = Connection.Popryzhenok.ProductSale.ToObservableCollection();
            DataContext = this;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void GoToAddAgentPage(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AddAgentPage());
        }
    }
}
