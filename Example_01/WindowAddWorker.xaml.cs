using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using Example_01.Organizations.Managers;
using Example_01.Organizations.Workers;

namespace Example_01
{
    /// <summary>
    /// Логика взаимодействия для WindowAddWorker.xaml
    /// </summary>
    public partial class WindowAddWorker : Window
    {
        private Department department;
        private bool isManager;

        public WindowAddWorker(Department department)
        {
            InitializeComponent();

            this.department = department;
        }

        private void CbPosition_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isManager = cbPosition.SelectedIndex > 1;
            tbSalary.IsReadOnly = isManager;
            tbSalary.Background = isManager ? Brushes.LightGray: Brushes.White;
        }

        private void btnAddWorker_Click(object sender, RoutedEventArgs e)
        {
            if (IsCheck())
            {
                switch (cbPosition.Text)
                {
                    case "Интерн":
                        AddIntern();
                        break;
                    case "Сотрудник":
                        AddEmployee();
                        break;
                    case "Зам. нач. отдела":
                        AddCoDepartmentHead();
                        break;
                    case "Начальник отдела":
                        AddDepartmentHead();
                        break;
                }

                this.Close();
            }
            else
            {
                MessageBox.Show("Не все поля заполнены!", "Ошибка.");
            }
        }

        private bool IsCheck()
        {
            return !string.IsNullOrEmpty(tbFirstName.Text) &&
                   !string.IsNullOrEmpty(tbLastName.Text) &&
                   (isManager ^ !string.IsNullOrEmpty(tbSalary.Text));

        }

        private void AddDepartmentHead()
        {
            if (department.Workers.Any(w => w is DepartmentHead))
                MessageBox.Show("Заместитель начальника отдела уже есть.", "Ошибка");
            else
                department.Workers.Insert(0,
                    new DepartmentHead(
                        tbFirstName.Text,
                        tbLastName.Text,
                        department
                    ));
        }

        private void AddCoDepartmentHead()
        {
            if (department.Workers.Any(w => w is CoDepartmentHead))
                MessageBox.Show("Заместитель начальника отдела уже есть.", "Ошибка");
            else
            {
                int pos = department.Workers.Any(w => w is DepartmentHead) ? 1 : 0;
                department.Workers.Insert(pos,
                    new CoDepartmentHead(
                        tbFirstName.Text,
                        tbLastName.Text,
                        department
                    ));
            }
        }

        private void AddEmployee()
        {
            department.Workers.Add(
                new Employee(
                    tbFirstName.Text,
                    tbLastName.Text,
                    uint.Parse(tbSalary.Text),
                    department
                ));
        }

        private void AddIntern()
        {
            department.Workers.Add(
                new Intern(
                    tbFirstName.Text,
                    tbLastName.Text,
                    uint.Parse(tbSalary.Text),
                    department
                    ));
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
