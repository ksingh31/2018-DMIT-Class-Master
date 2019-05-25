using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using ChinookSystem.DAL;
using ChinookSystem.Data.Entities;
using System.ComponentModel; //ods
using ChinookSystem.Data.POCOs;
using ChinookSystem.Data.DTOs;
#endregion

namespace ChinookSystem.BLL
{
    [DataObject]
    public class InfoAlbumController
    {
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<InfoAlbum> Info_AlbumSongList()
        {
            using (var context = new ChinookSystemContext())
            {
                var albumInfo = from x in context.Albums
                                where x.Tracks.Count() > 25
                                select new InfoAlbum
                                {
                                    ATitle = x.Title,
                                    AName = x.Artist.Name,
                                    Songs = (from y in x.Tracks
                                             select new InfoSong
                                             {
                                                 SongTitle = y.Name,
                                                 SongLength = y.Milliseconds / 60000 + ":" + (y.Milliseconds % 60000) / 1000
                                             }).ToList()
                                };
                return albumInfo.ToList();
            }
        }
    }
}
