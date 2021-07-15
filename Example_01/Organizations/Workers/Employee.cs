using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example_01.Organizations.Workers
{
    /// <summary>
    /// Класс, описывающий сотрудника.
    /// </summary>
    public class Employee : Worker
    {
        #region Constructors

        /// <summary>
        /// Создаем сотрудника. ВНИМАНИЕ! Зарплата сотрудника задается за месяц.
        /// </summary>
        /// <param name="firstName">Имя сотрудника.</param>
        /// <param name="lastName">Фамилия сотрудника.</param>
        /// <param name="salary">Зарплата сотрудника за месяц.</param>
        /// <param name="department">Отдел.</param>
        /// <param name="organization">Организация.</param>
        public Employee(string firstName, string lastName, uint salary, 
            Department department) 
            : base(firstName, lastName, salary, department, "Сотрудник")
        {
        }

        #endregion
    }
}
