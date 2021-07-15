using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Example_01.Organizations.Workers;

namespace Example_01.Organizations.Managers
{
    /// <summary>
    /// Абстрактный класс, описывающий управляющего.
    /// </summary>
    public abstract class Manager : Worker
    {
        #region Properties

        /// <summary>
        /// Список всех подчиненных рабочих.
        /// </summary>
        public ObservableCollection<Worker> Workers { get; set; }

        #endregion


        #region Constructors

        /// <summary>
        /// Создаем управляющего.
        /// </summary>
        /// <param name="firstName">Имя.</param>
        /// <param name="lastName">Фамилия.</param>
        /// <param name="salary">Зарплата.</param>
        /// <param name="department">Отдел.</param>
        /// <param name="organization">Организация.</param>
        protected Manager(string firstName, string lastName, uint salary,
            Department department, Organization organization) :
            base(firstName, lastName, salary, department, organization)
        {
            this.Workers = GetAllWorkers(department);
        }

        /// <summary>
        /// Создаем управляющего.
        /// </summary>
        /// <param name="firstName">Имя.</param>
        /// <param name="lastName">Фамилия.</param>
        /// <param name="salary">Зарплата.</param>
        /// <param name="department">Отдел.</param>
        protected Manager(string firstName, string lastName, uint salary,
            Department department, string position) :
            base(firstName, lastName, salary, department, position)
        {
            this.Workers = GetAllWorkers(department);
        }

        /// <summary>
        /// Создаем управляющего.
        /// </summary>
        /// <param name="firstName">Имя.</param>
        /// <param name="lastName">Фамилия.</param>
        /// <param name="salary">Зарплата.</param>
        /// <param name="department">Отдел.</param>
        protected Manager(string firstName, string lastName,
            Department department, string position) :
            base(firstName, lastName, 0, department, position)
        {
            this.Workers = GetAllWorkers(department);
        }

        #endregion


        #region Methods

        /// <summary>
        /// Получить список всех рабочих.
        /// </summary>
        /// <param name="department"></param>
        /// <returns></returns>
        private ObservableCollection<Worker> GetAllWorkers(Department department)
        {
            if (department == null) return new ObservableCollection<Worker>();
            ObservableCollection<Worker> workers = new ObservableCollection<Worker>(department.Workers);

            if (department.Departments.Count == 0) return workers;

            foreach (var departmentSubsidiary in department.Departments)
            {
                ObservableCollection<Worker> temp = GetAllWorkers(departmentSubsidiary);
                foreach (var worker in temp)
                {
                    workers.Add(worker);
                }
            }

            return workers;

        }
        
        #endregion

        
        #region Override Methods

        /// <summary>
        /// Выдать зарплату.
        /// </summary>
        public override void GiveSalary()
        {
            this.Workers = GetAllWorkers(this.Department);
            uint allSalary = (uint) this.Workers.Where(w => w is Employee)
                .Sum(w => w.Salary);
            allSalary += (uint) this.Workers.Where(w => w is Intern)
                .Sum(w => w.Salary * 8 * 30);
            this.Salary = allSalary < 1300 ? 1300 : allSalary;
            base.GiveSalary();
        }

        #endregion

    }
}
