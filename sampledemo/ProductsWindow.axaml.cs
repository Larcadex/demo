using System;
using System.Collections.ObjectModel;
using System.Linq;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.EntityFrameworkCore;
using sampledemo.Context;

namespace sampledemo
{
    public partial class ProductsWindow : Window
    {
        public ObservableCollection<ProductPartnerPresenter> ProductHistory { get; set; }

        public ProductsWindow(Partner partner)
        {
            InitializeComponent();
            DataContext = this;
            LoadProductHistory(partner);
        }
        public ProductsWindow()
        {
            
        }

        private void LoadProductHistory(Partner partner)
        {
            using var context = new User11Context();
            var history = context.Productpartners
                .Where(pp => pp.Partnerid == partner.Id)
                .Include(pp => pp.Product)
                .Select(pp => new ProductPartnerPresenter
                {
                    ProductName = pp.Product.Name,
                    Count = pp.Count,
                    Date = pp.Date
                })
                .ToList();

            ProductHistory = new ObservableCollection<ProductPartnerPresenter>(history);
            History.ItemsSource = ProductHistory;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            ShowMainWindow();
        }

        private void ShowMainWindow()
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        public class ProductPartnerPresenter
        {
            public string ProductName { get; set; }
            public int? Count { get; set; }
            public DateOnly? Date { get; set; }
        }
    }
}
