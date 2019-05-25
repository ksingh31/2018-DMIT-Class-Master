
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using ChinookSystem.Data.POCOs;
#endregion

namespace ChinookSystem.Data.DTOs
{
    public class SupportEmployee
    {
        public string Name { get; set; }
        public string EmpTitle { get; set; }
        public int ClientCount { get; set; }
        public List<Client> ClientList { get; set; }
    }
}
