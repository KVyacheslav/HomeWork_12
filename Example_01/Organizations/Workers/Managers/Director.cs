using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Example_01.Organizations.Managers
{
    /// <summary>
    /// Класс, описывающий директора организации.
    /// </summary>
    public class Director : Manager
    {

        /// <summary>
        /// Создаем директора.
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="organization"></param>
        public Director(string firstName, string lastName, Organization organization) :
            base(firstName, lastName, null, "Директор")
        {
            this.Organization = organization;
        }
    }
}
