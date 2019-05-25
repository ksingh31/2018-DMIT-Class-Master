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
    public class InfoAlbum
    {
        public string ATitle { get; set; }
        public string AName { get; set; }
        public List<InfoSong> Songs { get; set; }
    }
}
