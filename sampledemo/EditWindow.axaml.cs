using Avalonia.Controls;
using Avalonia.Interactivity;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using sampledemo.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sampledemo
{
    public partial class EditWindow : Window
    {
        private readonly Partner _partner;
        private List<Partnertype> _partnerTypes;

        public EditWindow(Partner partner)
        {
            InitializeComponent();
            _partner = partner;

            LoadPartnerTypes();
            InitializeFields();
        }
        public EditWindow()
        {
            
        }

        private void LoadPartnerTypes()
        {
            using var dbContext = new User11Context();
            _partnerTypes = dbContext.Partnertypes.ToList();
            PartnerTypeComboBox.ItemsSource = _partnerTypes;
        }

        private void InitializeFields()
        {
            if (_partner != null)
            {
                PartnerTypeComboBox.SelectedItem = _partnerTypes.FirstOrDefault(pt => pt.Id == _partner.Typepartner);
                NameTextBox.Text = _partner.Name;
                DirectorTextBox.Text = _partner.Director;
                PhoneTextBox.Text = _partner.Phone;
                RateTextBox.Text = _partner.Rate?.ToString() ?? string.Empty;
            }
            else
            {
                PartnerTypeComboBox.SelectedIndex = 0;
                NameTextBox.Text = DirectorTextBox.Text = PhoneTextBox.Text = RateTextBox.Text = string.Empty;
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateFields())
            {
                if (_partner != null)
                {
                    UpdatePartner();
                }
                else
                {
                    CreatePartner();
                }
                ShowMainWindow();
            }
        }

        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                ShowError("Поле 'Наименование' обязательно для заполнения.").Wait();
                return false;
            }

            if (string.IsNullOrWhiteSpace(DirectorTextBox.Text))
            {
                ShowError("Поле 'Директор' обязательно для заполнения.").Wait();
                return false;
            }

            if (string.IsNullOrWhiteSpace(PhoneTextBox.Text))
            {
                ShowError("Поле 'Телефон' обязательно для заполнения.").Wait();
                return false;
            }

            if (!int.TryParse(RateTextBox.Text, out _))
            {
                ShowError("Поле 'Рейтинг' должно быть числом.").Wait();
                return false;
            }

            return true;
        }

        private async Task ShowError(string message)
        {
            var box = MessageBoxManager.GetMessageBoxStandard("Ошибка", message, ButtonEnum.Ok);
            await box.ShowAsync();
        }

        private void CreatePartner()
        {
            var selectedPartnerType = PartnerTypeComboBox.SelectedItem as Partnertype;
            var newPartner = new Partner
            {
                Typepartner = selectedPartnerType?.Id,
                Name = NameTextBox.Text,
                Director = DirectorTextBox.Text,
                Phone = PhoneTextBox.Text,
                Rate = int.TryParse(RateTextBox.Text, out int rate) ? rate : (int?)null
            };

            using var dbContext = new User11Context();
            dbContext.Partners.Add(newPartner);
            dbContext.SaveChanges();
        }

        private void UpdatePartner()
        {
            var selectedPartnerType = PartnerTypeComboBox.SelectedItem as Partnertype;
            _partner.Typepartner = selectedPartnerType?.Id;
            _partner.Name = NameTextBox.Text;
            _partner.Director = DirectorTextBox.Text;
            _partner.Phone = PhoneTextBox.Text;
            _partner.Rate = int.TryParse(RateTextBox.Text, out int rate) ? rate : (int?)null;

            using var dbContext = new User11Context();
            if (dbContext.Entry(_partner).State == EntityState.Detached)
            {
                dbContext.Partners.Attach(_partner);
            }
            dbContext.Entry(_partner).State = EntityState.Modified;
            dbContext.SaveChanges();
        }

        private void ShowMainWindow()
        {
            var mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            ShowMainWindow();
        }
    }
}
