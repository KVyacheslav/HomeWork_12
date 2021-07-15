using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example_01.Organizations.Managers
{
    /// <summary>
    /// Класс, описывающий начальника отдела.
    /// </summary>
    public class DepartmentHead : Manager
    {
        public DepartmentHead(string firstName, string lastName, Department department) : 
            base(firstName, lastName, department, "Начальник отдела")
        {
        }
    }
}
