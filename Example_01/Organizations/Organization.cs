using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Example_01.Organizations.Managers;
using Example_01.Organizations.Workers;

namespace Example_01.Organizations
{
    /// <summary>
    /// Класс, описывающий организацию.
    /// </summary>
    public class Organization : IEnumerable
    {
        #region Properties

        /// <summary>
        /// Название организации.
        /// </summary>
        public string Name { get; set; }

        public static DateTime CurrentTime { get; set; }

        /// <summary>
        /// Список работников.
        /// </summary>
        public ObservableCollection<Worker> Workers { get; set; }

        /// <summary>
        /// Список отделов.
        /// </summary>
        public ObservableCollection<Department> Departments { get; set; }

        /// <summary>
        /// Директор организации.
        /// </summary>
        public Director Director { get; set; }

        #endregion


        static Organization()
        {
            CurrentTime = DateTime.Now;
        }


        #region Constructors

        /// <summary>
        /// Создаем организацию.
        /// </summary>
        /// <param name="name">Название организации.</param>
        /// <param name="director">Директор организации.</param>
        public Organization(string name, Director director)
        {
            this.Name = name;
            this.Director = director;
            this.Departments = new ObservableCollection<Department>();
        }


        /// <summary>
        /// Создаем организацию.
        /// </summary>
        /// <param name="name">Название организации.</param>
        public Organization(string name)
        {
            this.Name = name;
            this.Departments = new ObservableCollection<Department>();
        }

        #endregion


        #region Methods

        public void GiveSalary(ObservableCollection<Department> departments)
        {
            foreach (var department in departments)
            {
                var workers = department.Workers;
                foreach (var worker in workers)
                {
                    worker.GiveSalary();
                }

                if (department.Departments.Count > 0) GiveSalary(department.Departments);
            }
        }

        #endregion


        #region Override Methods

        public override string ToString()
        {
            return this.Name;
        }

        public IEnumerator GetEnumerator()
        {
            return this.Departments.GetEnumerator();
        }

        #endregion
    }
}
