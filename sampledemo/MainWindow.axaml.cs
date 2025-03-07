using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using sampledemo.Context;

namespace sampledemo
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<PartnerPresenter> DisplayList { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            LoadPartners();
        }

        private void AddPartner_Click(object sender, RoutedEventArgs e)
        {
            var partnerWindow = new EditWindow(null);
            partnerWindow.Show();
            this.Close();
        }

        private void EditPartner_Click(object sender, RoutedEventArgs e)
        {
            if (PartnerListBox.SelectedItem is PartnerPresenter selectedPartner)
            {
                var partnerWindow = new EditWindow(selectedPartner);
                partnerWindow.Show();
                this.Close();
            }
        }

        private void ShowHistory_Click(object sender, RoutedEventArgs e)
        {
            if (PartnerListBox.SelectedItem is PartnerPresenter selectedPartner)
            {
                var historyWindow = new ProductsWindow(selectedPartner);
                historyWindow.Show();
                this.Close();
            }
        }

        private void PartnerListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool isItemSelected = PartnerListBox.SelectedItem != null;
            EditPartnerButton.IsEnabled = isItemSelected;
            ShowHistoryButton.IsEnabled = isItemSelected;
        }

        private void LoadPartners()
        {
            using var dbContext = new User11Context();
            var partners = dbContext.Partners
                .Include(p => p.TypepartnerNavigation)
                .Include(p => p.Productpartners)
                .Select(partner => new PartnerPresenter
                {
                    Id = partner.Id,
                    Typepartner = partner.Typepartner,
                    Name = partner.Name,
                    Director = partner.Director,
                    Phone = partner.Phone,
                    Rate = partner.Rate,
                    PartnerTypeName = partner.TypepartnerNavigation.Name,
                    CountSum = partner.Productpartners.Sum(r => r.Count)
                }).ToList();

            DisplayList = new ObservableCollection<PartnerPresenter>(partners);
            PartnerListBox.ItemsSource = DisplayList;
        }

        public class PartnerPresenter : Partner
        {
            public string PartnerTypeName { get; set; }

            private int? _countSum;
            public int DiscountPercentage { get; private set; }

            public int? CountSum
            {
                get => _countSum;
                set
                {
                    _countSum = value;
                    CalculateDiscountPercentage();
                }
            }

            private void CalculateDiscountPercentage()
            {
                if (_countSum.HasValue)
                {
                    DiscountPercentage = _countSum switch
                    {
                        < 10000 => 0,
                        < 50000 => 5,
                        < 300000 => 10,
                        _ => 15
                    };
                }
                else
                {
                    DiscountPercentage = 0;
                }
            }
        }
    }
}
