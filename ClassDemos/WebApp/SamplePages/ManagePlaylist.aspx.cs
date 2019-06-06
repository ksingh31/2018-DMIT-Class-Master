using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#region Additonal Namespaces
using ChinookSystem.BLL;
using ChinookSystem.Data.POCOs;
#endregion

namespace Jan2018DemoWebsite.SamplePages
{
    public partial class ManagePlaylist : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            TracksSelectionList.DataSource = null;
        }

        protected void CheckForException(object sender, ObjectDataSourceStatusEventArgs e)
        {
            MessageUserControl.HandleDataBoundException(e);
        }

        protected void ArtistFetch_Click(object sender, EventArgs e)
        {

            //code to go here
            TracksBy.Text = "Artist";
            SearchArg.Text = ArtistName.Text;
            TracksSelectionList.DataBind();
          }

        protected void MediaTypeFetch_Click(object sender, EventArgs e)
        {

            //code to go here
            TracksBy.Text = "MediaType";
            SearchArg.Text = MediaTypeDDL.SelectedValue;
            TracksSelectionList.DataBind();

        }

        protected void GenreFetch_Click(object sender, EventArgs e)
        {

            //code to go here
            TracksBy.Text = "Genre";
            SearchArg.Text = GenreDDL.SelectedValue;
            TracksSelectionList.DataBind();

        }

        protected void AlbumFetch_Click(object sender, EventArgs e)
        {

            //code to go here
            TracksBy.Text = "Album";
            SearchArg.Text = AlbumTitle.Text;
            TracksSelectionList.DataBind();

        }

        protected void PlayListFetch_Click(object sender, EventArgs e)
        {
            //code to go here
            if(string.IsNullOrEmpty(PlaylistName.Text))
            {
                MessageUserControl.ShowInfo("Required Data", "Playlist Name is required to lookup a playlist");
            }
            else
            {
                string username = "Webmaster"; //we will alter this when security is done
                string playlistname = PlaylistName.Text;
                MessageUserControl.TryRun(() =>
                {
                    PlaylistTracksController sysmgr = new PlaylistTracksController();
                    List<UserPlaylistTrack> datainfo = sysmgr.List_TracksForPlaylist(playlistname, username);
                    PlayList.DataSource = datainfo;
                    PlayList.DataBind();
                });
            }
 
        }

        protected void MoveDown_Click(object sender, EventArgs e)
        {
            //code to go here
            // validation
            //a) Playlist Name
            //b) Playlist set of tracks
            //c) have selected a track
            //d) cannot be last track
            // otherwise move
            if (string.IsNullOrEmpty(PlaylistName.Text))
            {
                MessageUserControl.ShowInfo("Move Track", "Missing Playlist Name.");
            }
            else
            {
                if(PlayList.Rows.Count == 0)
                {
                    MessageUserControl.ShowInfo("Move Track", "Missing Playlist.");
                }
                else
                {
                    //there will be data to collect
                    // create local variables to collect data
                    int trackid = 0;
                    int tracknumber = 0;
                    int rowselected = 0;
                    CheckBox playlisttrackselection = null;

                    //traverse the GridView, row by row
                    // and determine which checkbox(es) are turn on
                    for (int rowindex = 0; rowindex < PlayList.Rows.Count; rowindex++)
                    {
                        //access the control on the playlist row that is the checkbox (pointer to checkbox)
                        playlisttrackselection = PlayList.Rows[rowindex].FindControl("Selected") as CheckBox;
                        //is the checkbox on??
                        if(playlisttrackselection.Checked)
                        {
                            rowselected++; //count number of checkboxes turned on
                            // save the trackid
                            trackid = int.Parse((PlayList.Rows[rowindex].FindControl("TrackID") as Label).Text);
                            tracknumber = int.Parse((PlayList.Rows[rowindex].FindControl("TrackNumber") as Label).Text);
                        }
                    }

                    //How many tracks were selected
                    if(rowselected != 1)
                    {
                        MessageUserControl.ShowInfo("Move Item", "Multiple items selected - Only one track can be selected for movement.");
                    }

                    else
                    {
                        // is it the last track
                        //TrackNumber is a natural count value
                        if(tracknumber == PlayList.Rows.Count)
                        {
                            MessageUserControl.ShowInfo("Move Item", "This is the Last track in the Playlist and cannot be moved down.");
                        }
                        else
                        {
                            //move the track
                            MoveTrack(trackid, tracknumber, "down");
                        }
                    }
                }
            }
        }

        protected void MoveUp_Click(object sender, EventArgs e)
        {
            //code to go here
            //code to go here
            // validation
            //a) Playlist Name
            //b) Playlist set of tracks
            //c) have selected a track
            //d) cannot be first track
            // otherwise move
            if (string.IsNullOrEmpty(PlaylistName.Text))
            {
                MessageUserControl.ShowInfo("Move Track", "Missing Playlist Name.");
            }
            else
            {
                if (PlayList.Rows.Count == 0)
                {
                    MessageUserControl.ShowInfo("Move Track", "Missing Playlist.");
                }
                else
                {
                    //there will be data to collect
                    // create local variables to collect data
                    int trackid = 0;
                    int tracknumber = 0;
                    int rowselected = 0;
                    CheckBox playlisttrackselection = null;

                    //traverse the GridView, row by row
                    // and determine which checkbox(es) are turn on
                    for (int rowindex = 0; rowindex < PlayList.Rows.Count; rowindex++)
                    {
                        //access the control on the playlist row that is the checkbox (pointer to checkbox)
                        playlisttrackselection = PlayList.Rows[rowindex].FindControl("Selected") as CheckBox;
                        //is the checkbox on??
                        if (playlisttrackselection.Checked)
                        {
                            rowselected++; //count number of checkboxes turned on
                            // save the trackid
                            trackid = int.Parse((PlayList.Rows[rowindex].FindControl("TrackID") as Label).Text);
                            tracknumber = int.Parse((PlayList.Rows[rowindex].FindControl("TrackNumber") as Label).Text);
                        }
                    }

                    //How many tracks were selected
                    if (rowselected != 1)
                    {
                        MessageUserControl.ShowInfo("Move Item", "Multiple items selected - Only one track can be selected for movement.");
                    }

                    else
                    {
                        // is it the first track
                        //TrackNumber is a natural count value
                        if (tracknumber == 1)
                        {
                            MessageUserControl.ShowInfo("Move Item", "This is the First track in the Playlist and cannot be moved up.");
                        }
                        else
                        {
                            //move the track
                            MoveTrack(trackid, tracknumber, "up");
                        }
                    }
                }
            }

        }

        protected void MoveTrack(int trackid, int tracknumber, string direction)
        {
            //call BLL to move track
            MessageUserControl.TryRun(() =>
            {
                PlaylistTracksController sysmgr = new PlaylistTracksController();
                sysmgr.MoveTrack("Webmaster", PlaylistName.Text, trackid, tracknumber, direction);
                List<UserPlaylistTrack> datainfo = sysmgr.List_TracksForPlaylist(PlaylistName.Text, "Webmaster");
                PlayList.DataSource = datainfo;
                PlayList.DataBind();
            },"Move Track","Track has been moved");
        }


        protected void DeleteTrack_Click(object sender, EventArgs e)
        {
            //code to go here
 
        }

        protected void TracksSelectionList_ItemCommand(object sender, 
            ListViewCommandEventArgs e)
        {
            //code to go here
            if (string.IsNullOrEmpty(PlaylistName.Text))
            {
                MessageUserControl.ShowInfo("Required Data", "Playlist Name is required to add a track to a playlist");
            }
            else
            {
                string playlistname = PlaylistName.Text;
                string username = "Webmaster"; // change when security implemented
                //obtain track id from the ListView line that was selected
                //for the line I have created a CommandArgument
                //this value is available via the ListViewCommandEventArgs parameter e
                int trackid = int.Parse(e.CommandArgument.ToString());
                MessageUserControl.TryRun(() =>
                {
                    PlaylistTracksController sysmgr = new PlaylistTracksController();
                    sysmgr.Add_TrackToPLaylist(playlistname, username, trackid);
                    List<UserPlaylistTrack> datainfo = sysmgr.List_TracksForPlaylist(playlistname, username);
                    PlayList.DataSource = datainfo;
                    PlayList.DataBind();
                },"Adding a Track","Track has been added to the playlist.");
            }

        }

    }
}