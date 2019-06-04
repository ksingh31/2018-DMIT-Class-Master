using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using ChinookSystem.Data.Entities;
using ChinookSystem.Data.DTOs;
using ChinookSystem.Data.POCOs;
using ChinookSystem.DAL;
using System.ComponentModel;
using DMIT2018Common.UserControls;
#endregion

namespace ChinookSystem.BLL
{
    public class PlaylistTracksController
    {
        public List<UserPlaylistTrack> List_TracksForPlaylist(
            string playlistname, string username)
        {
            using (var context = new ChinookSystemContext())
            {

                //code to go here
                var results = from x in context.PlaylistTracks
                                where x.Playlist.Name.Equals(playlistname)
                                && x.Playlist.UserName.Equals(username)
                                orderby x.TrackNumber
                                select new UserPlaylistTrack
                                {
                                    TrackID = x.TrackId,
                                    TrackNumber = x.TrackNumber,
                                    TrackName = x.Track.Name,
                                    Milliseconds = x.Track.Milliseconds,
                                    UnitPrice = x.Track.UnitPrice
                                };
                return results.ToList();
            }
        }//eom
        public void Add_TrackToPLaylist(string playlistname, string username, int trackid)
        {
            using (var context = new ChinookSystemContext())
            {
                //code to go here
                //there are 2 default datatypes from Linq given to
                // the datatype var: IEnumerable or IQueryable
                //in C# explicit datatypes are created at compile time
                Playlist exists = (from x in context.Playlists
                              where x.Name.Equals(playlistname)
                              && x.UserName.Equals(username)
                              select x).FirstOrDefault(); // use this to find something if it exists or not
                PlaylistTrack newTrack = null;
                int tracknumber = 0;

                //this list is required for use by the Business Rule exception of the 
                // MessageUserControl
                List<string> reasons = new List<string>();

                // determine if the Playlist ("parent") instance needs to be created
                if (exists == null)
                {
                    //create a new playlist
                    exists.Name = playlistname;
                    exists.UserName = username;
                    //the .Add(item) ONLY stages your record for adding to the database
                    //the actual Physical Add to the database is done on .SaveChanges();
                    //the returned data instance from the add will happen when the .Savechanges() is
                    // actually executed.
                    //thus there is a logic delay on this code
                    //there is NO Real Identity value in the returned instance
                    //If you return the instance PK value will be 0
                    exists = context.Playlists.Add(exists);
                    //sence this is a new playlist
                    //logically the track number will be 1
                    tracknumber = 1;
                }
                else
                {
                    //calculate the next track number for an existing playlist
                    tracknumber = exists.PlaylistTracks.Count() + 1;

                    //restriction (BusinessRule)
                    // BusinessRule requires all error to be in a single List<string>
                    //A track may only exist once in the playlist
                    newTrack = exists.PlaylistTracks.SingleOrDefault(x => x.TrackId == trackid);
                    if( newTrack != null )
                    {
                        reasons.Add("Track already exists on Playlist");
                    }
                }
                
                // do we add the track to the playlist - reasons list has all the errors so we can check for reasons count
                if( reasons.Count() > 0 )
                {
                    //some business rule within the trasaction has failed
                    //throw the BusinessRuleException
                    throw new BusinessRuleException("Adding track to playlist", reasons);
                }

                else
                {
                    // Part 2
                    // Adding the new track to the playlist tracks table
                }

            }
        }//eom
        public void MoveTrack(string username, string playlistname, int trackid, int tracknumber, string direction)
        {
            using (var context = new ChinookSystemContext())
            {
                //code to go here 

            }
        }//eom


        public void DeleteTracks(string username, string playlistname, List<int> trackstodelete)
        {
            using (var context = new ChinookSystemContext())
            {
               //code to go here


            }
        }//eom
    }
}
