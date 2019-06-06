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
                    exists = new Playlist();
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

                    // create a new instanrace of the PlaylistTrack
                    newTrack = new PlaylistTrack();
                    //load with known data
                    newTrack.TrackId = trackid;
                    newTrack.TrackNumber = tracknumber;

                    // currently the Playlist ID is unknown if the Playlist is brand new
                    // NOTE: using navigational properties, one can let the HashSet of 
                    // playlist handle the PlaylistId PKey Value.
                    // Adding via the navigational property will have the system enter
                    // the parent PKey for the corresponding FKey value
                    // In PlaylistTrack, PlaylistId is BOTH, the PKey and FKey
                    // During Savechanges() the PlaylistId will be filled
                    exists.PlaylistTracks.Add(newTrack);

                    //committing of your work
                    // only one comit for the transaction
                    // During .Savechanges() the data is added physically to
                    // your database at which time PKey (identity) is generated
                    //the order of actions has been done by your logic
                    // the playlist PKey will be generated
                    //this value will be placed in the FKey of the child record
                    // the child  record will be placed in its table
                    context.SaveChanges();
                }

            }
        }//eom
        public void MoveTrack(string username, string playlistname, int trackid, int tracknumber, string direction)
        {
            using (var context = new ChinookSystemContext())
            {
                //code to go here 
                // since data can be accessed by multiple individuals at the same time 
                // your BLL method should do validation to ensure the data coming is appropriate

                var exists = context.Playlists.Where(x => x.UserName.Equals(username) &&
                                x.Name.Equals(playlistname)).Select(x => x).FirstOrDefault();
                // playlist no longer exists
                if (exists == null)
                {
                    throw new Exception("Playlist has been removed from files.");
                }
                else
                {
                    var movetrack = exists.PlaylistTracks.Where(x => x.TrackId == trackid).Select(x => x).FirstOrDefault();
                    //playlist track no longer exists
                    if(movetrack == null)
                    {
                        throw new Exception("Playlist track has been removed from files - Movetrack.");
                    }
                    else
                    {
                        PlaylistTrack othertrack = null;
                        // determine direction
                        if (direction.Equals("up"))
                        {
                            if (movetrack.TrackNumber == 1)
                            {
                                throw new Exception("Playlist track already at top.");
                            }
                            else
                            {
                                //setup for track movement
                                othertrack = (from x in exists.PlaylistTracks
                                              where x.TrackNumber == movetrack.TrackNumber - 1
                                              select x).FirstOrDefault();
                                if (othertrack == null)
                                {
                                    throw new Exception("Playlist tracks have been altered. Unable to Complete move");
                                }

                                else
                                {
                                    movetrack.TrackNumber -= 1;
                                    othertrack.TrackNumber += 1;
                                }
                            }
                        }
                        else
                        {
                            if (movetrack.TrackNumber == exists.PlaylistTracks.Count)
                            {
                                throw new Exception("Playlist track already at last.");
                            }
                            else
                            {
                                //setup for track movement
                                othertrack = (from x in exists.PlaylistTracks
                                              where x.TrackNumber == movetrack.TrackNumber + 1
                                              select x).FirstOrDefault();
                                if (othertrack == null)
                                {
                                    throw new Exception("Playlist tracks have been altered. Unable to Complete move");
                                }

                                else
                                {
                                    movetrack.TrackNumber += 1;
                                    othertrack.TrackNumber -= 1;
                                }
                            }
                        }
                        //staging
                        //update
                        context.Entry(movetrack).Property(y => y.TrackNumber).IsModified = true;
                        context.Entry(othertrack).Property(y => y.TrackNumber).IsModified = true;

                        //commit
                        context.SaveChanges();
                    }
                }
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
