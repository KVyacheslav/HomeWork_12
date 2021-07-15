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
using Example_01.Organizations;

namespace Example_01
{
    /// <summary>
    /// Логика взаимодействия для WindowAddDepartment.xaml
    /// </summary>
    public partial class WindowAddDepartment : Window
    {
        private Department currentDepartment;
        private MainWindow window;

        public WindowAddDepartment(MainWindow window, Department currentDepartment)
        {
            InitializeComponent();
            
            this.currentDepartment = currentDepartment;
            this.window = window;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var name = tbNameDepartment.Text;

            if (string.IsNullOrEmpty(name))
                MessageBox.Show("Название отдела не может быть пустым", "Ошибка добавления.",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            else
            {
                Organization organization = window.cbOrganizations.SelectedItem as Organization;
                Department newDepartment = new Department(name, organization);

                if (currentDepartment == null)
                {
                    organization?.Departments.Add(newDepartment);
                    this.Close();
                    return;
                }

                this.currentDepartment.Departments.Add(newDepartment);
                newDepartment.UpDepartment = currentDepartment;

                this.Close();
            }
        }
    }
}
