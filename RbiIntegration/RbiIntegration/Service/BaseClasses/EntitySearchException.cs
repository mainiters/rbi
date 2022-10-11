using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RbiIntegration.Service.BaseClasses
{
    /// <summary>
    /// Исключение поиска сущности
    /// </summary>
    public class EntitySearchException : Exception
    {
        public EntitySearchException(string message) 
            : base(message)
        {

        }

    }
}
