using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppCore.Commons.Exceptions
{
    [Serializable]
    public class NameDuplicateException:Exception
    {
        public NameDuplicateException()
        {
            
        }
        public NameDuplicateException(string message) : base(message:message)
        {
            
        }
    }
}
