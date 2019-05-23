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
#endregion

namespace ChinookSystem.BLL
{
    [DataObject]
    public class AlbumController
    {
        #region Queries
        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<Album> Album_List()
        {
            using (var context = new ChinookSystemContext())
            {
                return context.Albums.ToList();
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public Album Album_Get(int albumid)
        {
            using (var context = new ChinookSystemContext())
            {
                return context.Albums.Find(albumid);
            }
        }

        [DataObjectMethod(DataObjectMethodType.Select, false)]
        public List<Album> Album_GetByArtist(int artistid)
        {
            using (var context = new ChinookSystemContext())
            {
                var results = from x in context.Albums
                              where x.ArtistId == artistid
                              select x;
                return results.ToList();
            }
        }
        
        [DataObjectMethod(DataObjectMethodType.Select,false)]
        public List<AlbumArtists> Album_ListAlbumArtists()
        {
            //when bringing your query from LinqPad
            //   you must remember LinqPad is Linq to SQL
            //in this application we use entities
            //therefore we will use Linq to Entities
            //setup your usual transaction to your context class
            //reference your appropriate context DbSet<>
            using (var context = new ChinookSystemContext())
            {
                var results = from a in context.Albums
                              orderby a.Title
                              select new AlbumArtists
                              {
                                  Title = a.Title,
                                  Year = a.ReleaseYear,
                                  ArtistName = a.Artist.Name
                              };
                return results.ToList();
            }
        }

        #endregion

        #region Add, Update, Delete

        [DataObjectMethod(DataObjectMethodType.Insert,false)]
        public int Album_Add(Album item)
        {
            using (var context = new ChinookSystemContext())
            {
                context.Albums.Add(item);   //stage add action
                context.SaveChanges();      //entity validation is executed
                return item.AlbumId;
            }
        }

        [DataObjectMethod(DataObjectMethodType.Update,false)]
        public int Album_Update(Album item)
        {
            using (var context = new ChinookSystemContext())
            {
                context.Entry(item).State = System.Data.Entity.EntityState.Modified;
                return context.SaveChanges();   //returned is number of rows affected
            }
        }

        [DataObjectMethod(DataObjectMethodType.Delete,false)]
        public int Album_Delete(Album item)
        {
            return Album_Delete(item.AlbumId);
        }

        public int Album_Delete(int albumid)
        {
            using (var context = new ChinookSystemContext())
            {
                var existing = context.Albums.Find(albumid);
                context.Albums.Remove(existing);
                return context.SaveChanges();
            }
        }
        #endregion
    }
}
