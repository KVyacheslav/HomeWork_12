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
    /// Логика взаимодействия для WindowChangeWorker.xaml
    /// </summary>
    public partial class WindowChangeWorker : Window
    {
        private Department department;
        private MainWindow window;
        private Worker worker;
        private bool isManager;

        public WindowChangeWorker(MainWindow window)
        {
            InitializeComponent();

            this.window = window;

            InitializeData();

            this.tbFirstName.Text = worker.FirstName;
            this.tbLastName.Text = worker.LastName;
            foreach (var cbPositionItem in cbPosition.Items)
            {
                if (cbPositionItem.ToString().Equals(worker.Position))
                {
                    cbPosition.SelectedItem = cbPositionItem;
                    break;
                }
            }
            isManager = cbPosition.SelectedIndex > 1;
            if (!isManager)
            {
                tbSalary.Text = worker.Salary.ToString();
            }
            tbSalary.IsReadOnly = isManager;
            tbSalary.Background = isManager ? Brushes.LightGray : Brushes.White;
        }

        private void InitializeData()
        {
            this.department = window.treeView.SelectedItem as Department;
            this.worker = this.window.listView.SelectedItem as Worker;
            switch (worker.Position)
            {
                case "Директор":
                    this.worker = worker as Director;
                    break;
                case "Начальник отдела":
                    this.worker = worker as DepartmentHead;
                    break;
                case "Зам начальника отдела":
                    this.worker = worker as CoDepartmentHead;
                    break;
                case "Интерн":
                    this.worker = worker as Intern;
                    break;
                default:
                    this.worker = worker as Employee;
                    break;
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void CbPosition_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            isManager = cbPosition.SelectedIndex > 1;
            tbSalary.IsReadOnly = isManager;
            tbSalary.Background = isManager ? Brushes.LightGray : Brushes.White;
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
            {
                department.Workers.Remove(worker);
                worker = new DepartmentHead(
                    tbFirstName.Text,
                    tbLastName.Text,
                    department
                );
                department.Workers.Insert(0, worker);
            }
        }

        private void AddCoDepartmentHead()
        {
            if (department.Workers.Any(w => w is CoDepartmentHead))
                MessageBox.Show("Заместитель начальника отдела уже есть.", "Ошибка");
            else
            {
                department.Workers.Remove(worker);
                int pos = department.Workers.Any(w => w is DepartmentHead) ? 1 : 0;
                worker = new CoDepartmentHead(
                    tbFirstName.Text,
                    tbLastName.Text,
                    department
                );
                department.Workers.Insert(pos, worker);
            }
        }

        private void AddEmployee()
        {
            department.Workers.Remove(worker);
            worker = new Employee(
                    tbFirstName.Text,
                    tbLastName.Text,
                    uint.Parse(tbSalary.Text),
                    department
                );
            department.Workers.Add(worker);
        }

        private void AddIntern()
        {
            department.Workers.Remove(worker);
            worker = new Intern(
                tbFirstName.Text,
                tbLastName.Text,
                uint.Parse(tbSalary.Text),
                department
            );
            department.Workers.Add(worker);
        }

        private void ChangeWorker(object sender, RoutedEventArgs e)
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
    }
}
