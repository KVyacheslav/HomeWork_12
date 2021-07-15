using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Example_01.Organizations;
using Example_01.Organizations.Managers;
using Example_01.Organizations.Workers;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;

namespace Example_01
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Organization> Organizations { get; set; }
        private DateTime currentDate;
        private int countDays;
        private Random rnd;

        public MainWindow()
        {
            InitializeComponent();
            this.Organizations = new ObservableCollection<Organization>();
            //this.Organizations.Add(new Organization("ООО ОМСК"));
            //this.Organizations.Add(new Organization("ООО НОВОСИБИРСК"));
            //this.Organizations.Add(new Organization("ООО МОСКВА"));
            this.currentDate = DateTime.Now;
            this.countDays = 0;

            var fileName = "data.json";

            //Organizations[0].Director = new Director("Aleksey", "Pavlov", Organizations[0]);
            //Organizations[0].Director.Department = new Department($"{Organizations[0].Name}", Organizations[0]);
            ////InitializeDepartments(Organizations[0]);
            //int countDeps = rnd.Next(8, 12);
            //InitializeDeps(Organizations[0], Organizations[0].Departments, countDeps);

            //Organizations[1].Director = new Director("Petr", "Yaryigin", Organizations[1]);
            //Organizations[1].Director.Department = new Department($"{Organizations[1].Name}", Organizations[1]);
            ////InitializeDepartments(Organizations[1]);
            //countDeps = rnd.Next(5, 8);
            //InitializeDeps(Organizations[1], Organizations[1].Departments, countDeps);

            //Organizations[2].Director = new Director("Vasya", "Melin", Organizations[2]);
            //Organizations[2].Director.Department = new Department($"{Organizations[2].Name}", Organizations[2]);
            ////InitializeDepartments(Organizations[2]);
            //countDeps = rnd.Next(5, 11);
            //InitializeDeps(Organizations[2], Organizations[2].Departments, countDeps);

            //cbOrganizations.ItemsSource = Organizations;
            //cbOrganizations.SelectedItem = cbOrganizations.Items[0];

            if (File.Exists(fileName))
            {
                PreLoad(fileName);
            }

        }

        private void InitializeDeps(Organization organization,
            ObservableCollection<Department> departments, int countDeps, Department upDep = null)
        {
            for (int i = 0; i < countDeps; i++)
            {
                string id = Guid.NewGuid().ToString();
                id = id.Substring(0, 5);
                var dep = new Department($"Department_{id}", organization);

                if (rnd.Next(4) == 1)
                {
                    countDeps = rnd.Next(2, 5);
                    InitializeDeps(organization, dep.Departments, countDeps, dep);
                }
                else
                {
                    int countWorkers = rnd.Next(3, 11);
                    InitializeWorkers(dep, countWorkers);
                }

                if (upDep != null)
                    dep.UpDepartment = upDep;
                departments.Add(dep);
            }

            organization.Director.Department.Departments = organization.Departments;

            organization.GiveSalary(organization.Departments);
            organization.Director.GiveSalary();
        }

        //private void InitializeDepartments(Organization organization)
        //{
        //    for (int i = 0; i < 10; i++)
        //    {
        //        string id_1 = Guid.NewGuid().ToString().Substring(0, 5);
        //        var dep = new Department($"Department_{id_1}", organization);

        //        if (rnd.Next(4) == 3)
        //        {
        //            int count = rnd.Next(2, 5);

        //            for (int j = 0; j < count; j++)
        //            {
        //                string id_2 = Guid.NewGuid().ToString().Substring(0, 5);
        //                var dep_2 = new Department($"Department_{id_1}_{id_2}", organization);

        //                if (rnd.Next(4) == 3)
        //                {
        //                    count = rnd.Next(2, 4);

        //                    for (int k = 0; k < count; k++)
        //                    {
        //                        string id_3 = Guid.NewGuid().ToString().Substring(0, 5);
        //                        var dep_3 = new Department($"Department_{id_1}_{id_2}_{id_3}", organization);
        //                        InitializeWorkers(dep_3, rnd.Next(3, 10));
        //                        dep_2.Departments.Add(dep_3);
        //                    }
        //                }
        //                else
        //                {
        //                    InitializeWorkers(dep_2, rnd.Next(3, 10));
        //                }

        //                dep.Departments.Add(dep_2);
        //            }
        //        }
        //        else
        //        {
        //            InitializeWorkers(dep, rnd.Next(3, 10));
        //        }

        //        organization.Departments.Add(dep);
        //    }

        //    organization.Director.Department.Departments = organization.Departments;

        //    organization.GiveSalary(organization.Departments);
        //    organization.Director.GiveSalary();
        //}

        private void InitializeWorkers(Department dep, int countWorkers)
        {
            string id = dep.Name.Substring(dep.Name.IndexOf("_") + 1);
            DepartmentHead head = new DepartmentHead($"Head_{id}", $"Head_{id}", dep);
            dep.Workers.Add(head);
            dep.DepartmentHead = head;
            if (rnd.Next(3) == 1)
            {
                CoDepartmentHead coHead = new CoDepartmentHead($"CoHead_{id}", $"CoHead_{id}", dep);
                dep.Workers.Add(coHead);
                dep.CoDepartmentHead = coHead;
            }

            int countEmps = countWorkers - rnd.Next(countWorkers);
            int countInterns = countWorkers - countEmps;

            for (int i = 0; i < countEmps; i++)
            {
                Employee emp = new Employee($"Emp_{id}#{i + 1}", $"Emp_{id}#{i + 1}",
                    (uint) rnd.Next(400, 901), dep);
                dep.Workers.Add(emp);
            }

            for (int i = 0; i < countInterns; i++)
            {
                Intern intern = new Intern($"Intern_{id}#{i + 1}", $"Intern_{id}#{i + 1}",
                    (uint) rnd.Next(5, 10), dep);
                dep.Workers.Add(intern);
            }
        }

        /// <summary>
        /// Показывает список работников при выборе отдела.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeView_OnSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            listView.ItemsSource = (treeView.SelectedItem as Department)?.Workers;
        }

        /// <summary>
        /// Добавить отдел.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddDepartment(object sender, RoutedEventArgs e)
        {
            Department department = treeView.SelectedItem as Department;
            new WindowAddDepartment(this, department).Show();
        }

        /// <summary>
        /// Удалить отдел.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveDepartment(object sender, RoutedEventArgs e)
        {
            var currentDepartment = treeView.SelectedItem as Department;

            if (currentDepartment == null)
            {
                MessageBox.Show("Для удаления, нужно выбрать отдел.", "Ошибка удаления.");
            }
            else
            {
                var upDepartment = currentDepartment.UpDepartment;
                Organization organization = cbOrganizations.SelectedItem as Organization;

                if (upDepartment == null)
                {
                    organization?.Departments.Remove(currentDepartment);
                }
                else
                {
                    upDepartment.Departments.Remove(currentDepartment);
                }

                currentDepartment.Workers.Clear();
                MessageBox.Show("Сотрудники уволены!.", "Отдел удален.");

                organization?.GiveSalary(organization.Departments);
                organization?.Director.GiveSalary();
                ObservableCollection<Worker> dir = new ObservableCollection<Worker>();
                dir.Add(organization?.Director);
                listView.ItemsSource = dir;
            }
        }

        /// <summary>
        /// Следующий день.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextDay(object sender, RoutedEventArgs e)
        {
            Organization.CurrentTime = Organization.CurrentTime.AddDays(1);
            countDays++;
            tbDays.Text = countDays.ToString();
            foreach (var organization in Organizations)
            {
                organization.GiveSalary(organization.Departments);
                organization.Director.GiveSalary();
            }
        }

        /// <summary>
        /// Добавить сотрудника.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddWorker(object sender, RoutedEventArgs e)
        {
            if (treeView.SelectedItem is Department department)
            {
                if (department.Departments.Count != 0)
                {
                    MessageBox.Show("Нельзя добавлять в этом отделе сотрудников, " +
                                    "так как он имеет дочерние отделы!", "Ошибка добавления сотрудника.");
                    return;
                }

                new WindowAddWorker(department).Show();

                Organization organization = cbOrganizations.SelectedItem as Organization;
                organization?.GiveSalary(organization.Departments);
                organization?.Director.GiveSalary();
            }
        }

        /// <summary>
        /// Удалить сотрудника.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RemoveWorker(object sender, RoutedEventArgs e)
        {
            Worker worker = listView.SelectedItem as Worker;

            if (worker == null)
            {
                MessageBox.Show("Должен быть выбран сотрудник!", "Ошибка.");
                return;
            }

            Department department = treeView.SelectedItem as Department;
            department.Workers.Remove(worker);

            Organization organization = cbOrganizations.SelectedItem as Organization;
            organization?.GiveSalary(organization.Departments);
            organization?.Director.GiveSalary();
        }

        /// <summary>
        /// Выбор организации.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbOrganizations_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            treeView.ItemsSource = null;
            ObservableCollection<Worker> dir = new ObservableCollection<Worker>();
            Organization organization = cbOrganizations.SelectedItem as Organization;
            dir.Add(organization?.Director);
            listView.ItemsSource = dir;
            treeView.ItemsSource = organization?.Departments;
            this.Title = organization?.Name ?? "Не выбрана организация";
        }

        /// <summary>
        /// Сохранить данные.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new SaveFileDialog { Filter = "JSON Files|*.json" };

            var flag = saveFileDialog.ShowDialog();

            if (flag == true)
            {
                var organizations = new JArray();

                foreach (var organization in Organizations)
                {
                    var org = new JObject();
                    org["name"] = organization.Name;

                    var director = new JObject();
                    director["first_name"] = organization.Director.FirstName;
                    director["last_name"] = organization.Director.LastName;
                    director["sum"] = organization.Director.Sum;

                    org["director"] = director;

                    var departments = organization.Departments;

                    var departArray = new JArray();

                    foreach (var department in departments)
                    {
                        var dep = new JObject();
                        dep["name"] = department.Name;

                        if (department.DepartmentHead != null)
                        {
                            JObject departmentHead = new JObject();
                            departmentHead["dep_head_first_name"] = department.DepartmentHead.FirstName;
                            departmentHead["dep_head_last_name"] = department.DepartmentHead.LastName;
                            departmentHead["dep_head_sum"] = department.DepartmentHead.Sum;

                            dep["department_head"] = departmentHead;
                        }

                        if (department.CoDepartmentHead != null)
                        {
                            JObject coDepartmentHead = new JObject();
                            coDepartmentHead["co_dep_head_first_name"] = department.CoDepartmentHead.FirstName;
                            coDepartmentHead["co_dep_head_last_name"] = department.CoDepartmentHead.LastName;
                            coDepartmentHead["co_dep_head_sum"] = department.CoDepartmentHead.Sum;

                            dep["co_department_head"] = coDepartmentHead;
                        }

                        FillWorkers(dep, department);

                        FillDepartments(dep, department);

                        departArray.Add(dep);
                    }

                    org["departments"] = departArray;

                    organizations.Add(org);
                }

                File.WriteAllText(saveFileDialog.FileName, organizations.ToString());
            }
        }

        /// <summary>
        /// Заполнить рабочих в Json объект из отдела.
        /// </summary>
        /// <param name="dep">Json объект.</param>
        /// <param name="department">Отдел.</param>
        private void FillWorkers(JObject dep, Department department)
        {
            if (department.Workers.Count == 0)
                return;

            var workers = new JArray();

            foreach (var worker in department.Workers)
            {
                if (worker is Manager)
                    continue;

                var w = new JObject();
                w["first_name"] = worker.FirstName;
                w["last_name"] = worker.LastName;
                w["salary"] = worker.Salary;
                w["sum"] = worker.Sum;
                workers.Add(w);
            }

            dep["workers"] = workers;
        }

        /// <summary>
        /// Заполнить отдел списком дочерних отделов.
        /// </summary>
        /// <param name="dep">Json объект отдела.</param>
        /// <param name="department">Отдел.</param>
        private void FillDepartments(JObject dep, Department department)
        {
            if (department.Departments.Count == 0)
                return;

            var departArray = new JArray();

            foreach (var oDep in department.Departments)
            {
                var tempDep = new JObject();
                tempDep["name"] = oDep.Name;

                if (oDep.DepartmentHead != null)
                {
                    var departmentHead = new JObject();
                    departmentHead["dep_head_first_name"] = oDep.DepartmentHead.FirstName;
                    departmentHead["dep_head_last_name"] = oDep.DepartmentHead.LastName;
                    departmentHead["dep_head_sum"] = oDep.DepartmentHead.Sum;

                    tempDep["department_head"] = departmentHead;
                }

                if (oDep.CoDepartmentHead != null)
                {
                    JObject coDepartmentHead = new JObject();
                    coDepartmentHead["co_dep_head_first_name"] = oDep.CoDepartmentHead.FirstName;
                    coDepartmentHead["co_dep_head_last_name"] = oDep.CoDepartmentHead.LastName;
                    coDepartmentHead["co_dep_head_sum"] = oDep.CoDepartmentHead.Sum;

                    tempDep["co_department_head"] = coDepartmentHead;
                }

                FillWorkers(tempDep, oDep);
                FillDepartments(tempDep, oDep);

                departArray.Add(tempDep);
            }

            dep["departments"] = departArray;
        }

        /// <summary>
        /// Загрузить данные.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Load(object sender, RoutedEventArgs e)
        {
            var saveFileDialog = new OpenFileDialog { Filter = "JSON Files|*.json" };

            var flag = saveFileDialog.ShowDialog();
            if (flag == true)
            {
                var data = File.ReadAllText(saveFileDialog.FileName);
                var jOrganizations = JArray.Parse(data);
                this.Organizations = new ObservableCollection<Organization>();

                foreach (var organization in jOrganizations)
                {
                    var name = organization["name"]?.ToString();
                    var org = new Organization(name);

                    var director = organization["director"];
                    if (director != null)
                    {
                        var firstNameDir = director["first_name"]?.ToString();
                        var lastNameDir = director["last_name"]?.ToString();
                        var sum = Convert.ToUInt32(director["sum"]?.ToString());

                        org.Director = new Director(firstNameDir, lastNameDir, org) { Sum = sum };

                        org.GiveSalary(org.Departments);
                        org.Director.GiveSalary();
                    }

                    var departments = organization["departments"]?.ToArray();

                    if (departments != null)
                        foreach (var department in departments)
                        {
                            var depName = department["name"]?.ToString();
                            var dep = new Department(depName, org);

                            var head = department["department_head"];
                            if (head != null)
                            {
                                var firstName = head["dep_head_first_name"]?.ToString();
                                var lastName =  head["dep_head_last_name"]?.ToString();
                                var sum = Convert.ToUInt32(head["dep_head_sum"]?.ToString());
                                var departmentHead =
                                    new DepartmentHead(firstName, lastName, dep) {Sum = sum};
                                dep.DepartmentHead = departmentHead;
                                dep.Workers.Add(departmentHead);
                            }

                            var coHead = department["co_department_head"];
                            if (coHead != null)
                            {
                                var firstName = coHead["co_dep_head_first_name"]?.ToString();
                                var lastName = coHead["co_dep_head_last_name"]?.ToString();
                                var sum = Convert.ToUInt32(coHead["dep_head_sum"]?.ToString());
                                var coDepartmentHead =
                                    new CoDepartmentHead(firstName, lastName, dep) { Sum = sum };
                                dep.CoDepartmentHead = coDepartmentHead;
                                dep.Workers.Add(coDepartmentHead);
                            }


                            org.Departments.Add(dep);

                            LoadWorkers(department, dep);
                            LoadDepartments(department, dep);
                        }

                    org.Director.Department = new Department(org.Name, org) {Departments = org.Departments};
                    org.GiveSalary(org.Departments);
                    org.Director.GiveSalary();
                    Organizations.Add(org);
                }

                cbOrganizations.ItemsSource = Organizations;
                cbOrganizations.SelectedItem = cbOrganizations.Items[0];
            }
        }

        /// <summary>
        /// Предзагрузка данных.
        /// </summary>
        /// <param name="fileName"></param>
        private void PreLoad(string fileName)
        {
            var data = File.ReadAllText(fileName);
            var jOrganizations = JArray.Parse(data);
            this.Organizations = new ObservableCollection<Organization>();

            foreach (var organization in jOrganizations)
            {
                var name = organization["name"]?.ToString();
                var org = new Organization(name);

                var director = organization["director"];
                if (director != null)
                {
                    var firstNameDir = director["first_name"]?.ToString();
                    var lastNameDir = director["last_name"]?.ToString();
                    var sum = Convert.ToUInt32(director["sum"]?.ToString());

                    org.Director = new Director(firstNameDir, lastNameDir, org) { Sum = sum };

                    org.GiveSalary(org.Departments);
                    org.Director.GiveSalary();
                }

                var departments = organization["departments"]?.ToArray();

                if (departments != null)
                    foreach (var department in departments)
                    {
                        var depName = department["name"]?.ToString();
                        var dep = new Department(depName, org);

                        var head = department["department_head"];
                        if (head != null)
                        {
                            var firstName = head["dep_head_first_name"]?.ToString();
                            var lastName = head["dep_head_last_name"]?.ToString();
                            var sum = Convert.ToUInt32(head["dep_head_sum"]?.ToString());
                            var departmentHead =
                                new DepartmentHead(firstName, lastName, dep) { Sum = sum };
                            dep.DepartmentHead = departmentHead;
                            dep.Workers.Add(departmentHead);
                        }

                        var coHead = department["co_department_head"];
                        if (coHead != null)
                        {
                            var firstName = coHead["co_dep_head_first_name"]?.ToString();
                            var lastName = coHead["co_dep_head_last_name"]?.ToString();
                            var sum = Convert.ToUInt32(coHead["dep_head_sum"]?.ToString());
                            var coDepartmentHead =
                                new CoDepartmentHead(firstName, lastName, dep) { Sum = sum };
                            dep.CoDepartmentHead = coDepartmentHead;
                            dep.Workers.Add(coDepartmentHead);
                        }


                        org.Departments.Add(dep);

                        LoadWorkers(department, dep);
                        LoadDepartments(department, dep);
                    }

                org.Director.Department = new Department(org.Name, org) { Departments = org.Departments };
                org.GiveSalary(org.Departments);
                org.Director.GiveSalary();
                Organizations.Add(org);
            }

            cbOrganizations.ItemsSource = Organizations;
            cbOrganizations.SelectedItem = cbOrganizations.Items[0];
        }

        /// <summary>
        /// Загрузить сотрудников.
        /// </summary>
        /// <param name="department"></param>
        /// <param name="dep"></param>
        private void LoadWorkers(JToken department, Department dep)
        {
            if (department["workers"] == null)
                return;

            var workers = department["workers"].ToArray();

            foreach (var worker in workers)
            {
                var firstName = worker["first_name"]?.ToString();
                var lastName = worker["last_name"]?.ToString();
                var salary = Convert.ToUInt32(worker["salary"]?.ToString());
                var sum = Convert.ToUInt32(worker["sum"]?.ToString());

                Worker w;

                if (firstName.StartsWith("Emp"))
                    w = new Employee(firstName, lastName, salary, dep) { Sum = sum };
                else
                    w = new Intern(firstName, lastName, salary, dep) { Sum = sum };

                dep.Workers.Add(w);
            }
        }

        /// <summary>
        /// Загрузить отделы.
        /// </summary>
        /// <param name="department"></param>
        /// <param name="dep"></param>
        private void LoadDepartments(JToken department, Department dep)
        {
            if (department["departments"] == null)
                return;

            var departments = department["departments"].ToArray();

            foreach (var oDep in departments)
            {
                var name = oDep["name"]?.ToString();
                var tmpDep = new Department(name, dep.Organization);

                var head = oDep["department_head"];
                if (head != null)
                {
                    var firstName = head["dep_head_first_name"]?.ToString();
                    var lastName = head["dep_head_last_name"]?.ToString();
                    var sum = Convert.ToUInt32(head["dep_head_sum"]?.ToString());
                    var departmentHead =
                        new DepartmentHead(firstName, lastName, tmpDep) {Sum = sum};
                    tmpDep.DepartmentHead = departmentHead;
                    tmpDep.Workers.Add(departmentHead);
                }

                var coHead = oDep["co_department_head"];
                if (coHead != null)
                {
                    var firstName = coHead["co_dep_head_first_name"]?.ToString();
                    var lastName = coHead["co_dep_head_last_name"]?.ToString();
                    var sum = Convert.ToUInt32(coHead["dep_head_sum"]?.ToString());
                    var coDepartmentHead =
                        new CoDepartmentHead(firstName, lastName, tmpDep) {Sum = sum};
                    tmpDep.CoDepartmentHead = coDepartmentHead;
                    tmpDep.Workers.Add(coDepartmentHead);
                }

                LoadWorkers(oDep, tmpDep);
                LoadDepartments(oDep, tmpDep);

                dep.Departments.Add(tmpDep);
                tmpDep.UpDepartment = dep;
            }
        }

        /// <summary>
        /// Переименовать отдел.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChangeNameDepartment(object sender, RoutedEventArgs e)
        {
            Department department = treeView.SelectedItem as Department;
            if (department == null)
            {
                MessageBox.Show("Должен быть выбран отдел!", "Ошибка.");
                return;
            }

            new WindowRenameDepartment(department).Show();
        }

        /// <summary>
        /// Соритровка по имени.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SortByFirstName(object sender, RoutedEventArgs e)
        {
            Department department = treeView.SelectedItem as Department;
            if (department == null)
            {
                MessageBox.Show("Должен быть выбран отдел!", "Ошибка.");
                return;
            }

            List<Worker> tmp = department.Workers.ToList();
            tmp.Sort(Worker.SorteBy(CriterionSort.FirstName));
            department.Workers.Clear();

            foreach (var worker in tmp)
            {
                department.Workers.Add(worker);
            }
        }

        private void SortByLastName(object sender, RoutedEventArgs e)
        {
            Department department = treeView.SelectedItem as Department;
            if (department == null)
            {
                MessageBox.Show("Должен быть выбран отдел!", "Ошибка.");
                return;
            }

            List<Worker> tmp = department.Workers.ToList();
            tmp.Sort(Worker.SorteBy(CriterionSort.LastName));
            department.Workers.Clear();

            foreach (var worker in tmp)
            {
                department.Workers.Add(worker);
            }
        }
        private void SortBySalary(object sender, RoutedEventArgs e)
        {
            Department department = treeView.SelectedItem as Department;
            if (department == null)
            {
                MessageBox.Show("Должен быть выбран отдел!", "Ошибка.");
                return;
            }

            List<Worker> tmp = department.Workers.ToList();
            tmp.Sort(Worker.SorteBy(CriterionSort.Salary));
            department.Workers.Clear();

            foreach (var worker in tmp)
            {
                department.Workers.Add(worker);
            }
        }

        private void SortBySum(object sender, RoutedEventArgs e)
        {
            Department department = treeView.SelectedItem as Department;
            if (department == null)
            {
                MessageBox.Show("Должен быть выбран отдел!", "Ошибка.");
                return;
            }

            List<Worker> tmp = department.Workers.ToList();
            tmp.Sort(Worker.SorteBy(CriterionSort.Sum));
            department.Workers.Clear();

            foreach (var worker in tmp)
            {
                department.Workers.Add(worker);
            }
        }

        private void SortByPosition(object sender, RoutedEventArgs e)
        {
            Department department = treeView.SelectedItem as Department;
            if (department == null)
            {
                MessageBox.Show("Должен быть выбран отдел!", "Ошибка.");
                return;
            }

            List<Worker> tmp = department.Workers.ToList();
            tmp.Sort(Worker.SorteBy(CriterionSort.Position));
            department.Workers.Clear();

            foreach (var worker in tmp)
            {
                department.Workers.Add(worker);
            }
        }

        private void ChangeWorker(object sender, RoutedEventArgs e)
        {
            Worker worker = listView.SelectedItem as Worker;

            if (worker == null)
            {
                MessageBox.Show("Должен быть выбран сотрудник!", "Ошибка.");
                return;
            }

            Department department = treeView.SelectedItem as Department;
            new WindowChangeWorker(this).Show();
        }
    }
}
