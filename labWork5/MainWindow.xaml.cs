using labWork3.Core;
using labWork3.DB;
using labWork3.Models;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace labWork5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ContactRepository _repository;
        ObservableCollection<Contact> _contacts;
        int indexUnderChanging = -1;
        int idUnderChanging = 0;
        public MainWindow()
        {
            InitializeComponent();

            var context = new RepositoryDBContext(null);
            context.Database.EnsureCreated();

            _repository = new DBBackedContactRepository(context);

            _contacts = new();
            ContactsList.ItemsSource = _contacts;
            MessageBox.Show(_repository.GetAllContacts().ToString());

            foreach (var item in  _repository.GetAllContacts())
            {
                _contacts.Add(item);
            }
        }

       

        private  void Contactslist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //MessageBox.Show(ContactsList.SelectedIndex.ToString());
            if (ContactsList.SelectedIndex >= 0)
            {
                indexUnderChanging = ContactsList.SelectedIndex;
                var selectedContact = _contacts[indexUnderChanging];
                idUnderChanging = selectedContact.Id;
                NameField.Text = selectedContact.FirstName;
                LastnameField.Text = selectedContact.LastName;
                PhoneField.Text = selectedContact.PhoneNumber;
                EmailField.Text = selectedContact.Email;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e) {
            indexUnderChanging = -1;
            NameField.Text = "";
            LastnameField.Text = "";
            PhoneField.Text = "";
            EmailField.Text = "";
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var name = NameField.Text;
            var lastName = LastnameField.Text;
            var phone = PhoneField.Text;
            var email = EmailField.Text;

            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Поле \"Имя\" не должно быть пустым");
                return;
            }
            if (string.IsNullOrEmpty(lastName))
            {
                MessageBox.Show("Поле \"Фамилия\" не должно быть пустым");
                return;
            }
            if (string.IsNullOrEmpty(phone))
            {
                MessageBox.Show("Поле \"Номер Телефона\" не должно быть пустым");
                return;
            }

            var contact = new Contact()
            {
                Id = idUnderChanging,
                FirstName = name,
                LastName = lastName,
                PhoneNumber = phone,
                Email = email
            };

            if (indexUnderChanging == -1)
            {
                _contacts.Add(contact);
                _repository.AddContact(contact);
                MessageBox.Show("Контакт добавлен.");

            }
            else
            {
                _contacts[indexUnderChanging] = contact;
                _repository.UpdateContact(contact);
                MessageBox.Show("Контакт изменен.");
            }

        }
    }
}