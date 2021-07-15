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
    /// Логика взаимодействия для WindowRenameDepartment.xaml
    /// </summary>
    public partial class WindowRenameDepartment : Window
    {
        private Department department;

        public WindowRenameDepartment(Department department)
        {
            InitializeComponent();

            this.department = department;
        }

        private void ChangeNameDepartment(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(tbNameDepartment.Text))
            {
                MessageBox.Show("Имя не должно быть пустым!", "Ошибка.");
                return;
            }

            this.department.Name = tbNameDepartment.Text;
            this.Close();
        }
    }
}
