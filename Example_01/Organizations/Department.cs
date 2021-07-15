using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Example_01.Annotations;
using Example_01.Organizations.Managers;
using Example_01.Organizations.Workers;

namespace Example_01.Organizations
{
    /// <summary>
    /// Класс, описывающий отдел.
    /// </summary>
    public class Department : IEnumerable, INotifyPropertyChanged
    {
        #region Properties

        private string name;

        /// <summary>
        /// Название отдела.
        /// </summary>
        public string Name
        {
            get => this.name;
            set
            {
                this.name = value;
                this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
            }
        }

        /// <summary>
        /// Вышестоящий отдел.
        /// </summary>
        public Department UpDepartment { get; set; }

        /// <summary>
        /// Дочерние отделы.
        /// </summary>
        public ObservableCollection<Department> Departments { get; set; }

        /// <summary>
        /// Список работников.
        /// </summary>
        public ObservableCollection<Worker> Workers { get; set; }

        /// <summary>
        /// Управляющий отделом.
        /// </summary>
        public DepartmentHead DepartmentHead { get; set; }

        /// <summary>
        /// Управляющий отделом.
        /// </summary>
        public CoDepartmentHead CoDepartmentHead { get; set; }

        /// <summary>
        /// Организация.
        /// </summary>
        public Organization Organization { get; set; }

        #endregion


        #region Constructors

        /// <summary>
        /// Создаем отдел.
        /// </summary>
        /// <param name="name">Название отдела.</param>
        /// <param name="upDepartment">Вышестоящий отдел.</param>
        /// <param name="departments">Дочерние отделы.</param>
        /// <param name="workers">Список работников.</param>
        /// <param name="departmentHead">Управляющий отделом.</param>
        /// <param name="coDepartmentHead">Заместитель управляющего отделом.</param>
        public Department(string name, Department upDepartment, ObservableCollection<Department> departments,
            ObservableCollection<Worker> workers, DepartmentHead departmentHead, CoDepartmentHead coDepartmentHead, Organization organization)
        {
            this.Name = name;
            this.UpDepartment = upDepartment;
            this.Departments = departments;
            this.Workers = workers;
            this.DepartmentHead = departmentHead;
            this.CoDepartmentHead = coDepartmentHead;
            this.Organization = organization;
        }

        /// <summary>
        /// Создаем отдел.
        /// </summary>
        /// <param name="name">Название отдела.</param>
        /// <param name="organization">Организация.</param>
        public Department(string name, Organization organization) :
            this(name, null, new ObservableCollection<Department>(),
                new ObservableCollection<Worker>(), null, null, organization)
        {
        }

        #endregion

        public IEnumerator GetEnumerator()
        {
            return Departments.GetEnumerator();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
