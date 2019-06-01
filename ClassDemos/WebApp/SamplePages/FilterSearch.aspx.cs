using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

#region Additional Namespaces
using ChinookSystem.BLL;
using ChinookSystem.Data.Entities;
#endregion

namespace WebApp.SamplePages
{
    public partial class FilterSearch : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindArtistList();
                //setting of a Validation Control property in code-behind
                RangeReleaseYear.MaximumValue = DateTime.Today.Year.ToString();
            }
        }

        protected void BindArtistList()
        {
            MessageUserControl.TryRun(() =>
            {
                ArtistController sysmgr = new ArtistController();
                List<Artist> info = sysmgr.Artist_List();
                info.Sort((x, y) => x.Name.CompareTo(y.Name));
                ArtistList.DataSource = info;
                ArtistList.DataTextField = nameof(Artist.Name);
                ArtistList.DataValueField = nameof(Artist.ArtistId);
                ArtistList.DataBind();
            });
        }

        protected void AlbumList_SelectedIndexChanged(object sender, EventArgs e)
        {
            //access data located on the gridviewrow
            //template web control access syntax
            GridViewRow agvrow = AlbumList.Rows[AlbumList.SelectedIndex];
            string albumid = (agvrow.FindControl("AlbumID") as Label).Text;
           
            MessageUserControl.TryRun(() => {
                //standard lookup
                AlbumController sysmgr = new AlbumController();
                Album datainfo = sysmgr.Album_Get(int.Parse(albumid));
                if (datainfo == null)
                {
                    ClearControls();
                    throw new Exception("No record found for selected album. Refresh you list of albums.");
                }
                else
                {
                    EditAlbumID.Text = datainfo.AlbumId.ToString();
                    EditTitle.Text = datainfo.Title;
                    EditAlbumArtistList.SelectedValue = datainfo.ArtistId.ToString();
                    EditReleaseYear.Text = datainfo.ReleaseYear.ToString();
                    EditReleaseLabel.Text = datainfo.ReleaseLabel == null ? "" : datainfo.ReleaseLabel;
                }
            },"Album Search", "View Album details");
        }

        protected void ClearControls()
        {
            EditAlbumID.Text ="";
            EditTitle.Text ="";
            EditAlbumArtistList.ClearSelection();
            EditReleaseYear.Text = "";
            EditReleaseLabel.Text = "";
        }

        protected void Add_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                //the validation that exists within the event is probably validation that 
                //   a) is not possible on the form
                //   b) required for specific events only
                //   c) for some reason, it was decided to the validation in code-behind
                if (int.Parse(EditReleaseYear.Text) >= 1950 &&
                    int.Parse(EditReleaseYear.Text) <= DateTime.Today.Year)
                {
                    MessageUserControl.TryRun(() =>
                    {
                        Album item = new Album();
                        item.Title = EditTitle.Text;
                        item.ReleaseYear = int.Parse(EditReleaseYear.Text);
                        item.ReleaseLabel = string.IsNullOrEmpty(EditReleaseLabel.Text) ? null : EditReleaseLabel.Text;
                        item.ArtistId = int.Parse(EditAlbumArtistList.SelectedValue);
                        AlbumController sysmgr = new AlbumController();
                        int newalbumid = sysmgr.Album_Add(item);
                        EditAlbumID.Text = newalbumid.ToString();
                        ArtistList.SelectedValue = item.ArtistId.ToString();
                        AlbumList.DataBind();
                    });
                }
                else
                {
                    MessageUserControl.ShowInfo("Adding", "Release Year must be 1950 to today");
                }
            }
        }

        protected void Update_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                //the validation that exists within the event is probably validation that 
                //   a) is not possible on the form
                //   b) required for specific events only
                //   c) for some reason, it was decided to the validation in code-behind
                if (string.IsNullOrEmpty(EditAlbumID.Text))
                {
                    MessageUserControl.ShowInfo("Updating", "Search for an exsiting album before updating");
                }
                else if (int.Parse(EditReleaseYear.Text) >= 1950 &&
                        int.Parse(EditReleaseYear.Text) <= DateTime.Today.Year)
                {
                    MessageUserControl.TryRun(() =>
                    {
                        Album item = new Album();
                        item.AlbumId = int.Parse(EditAlbumID.Text);
                        item.Title = EditTitle.Text;
                        item.ReleaseYear = int.Parse(EditReleaseYear.Text);
                        item.ReleaseLabel = string.IsNullOrEmpty(EditReleaseLabel.Text) ? null : EditReleaseLabel.Text;
                        item.ArtistId = int.Parse(EditAlbumArtistList.SelectedValue);
                        AlbumController sysmgr = new AlbumController();
                        int rowsaffected = sysmgr.Album_Update(item);
                        if (rowsaffected == 0)
                        {
                            throw new Exception("Album no longer on file. Refresh your search.");
                        }
                        else
                        {
                            ArtistList.SelectedValue = item.ArtistId.ToString();
                            AlbumList.DataBind();
                        }

                    }, "Update", "Action successful");
                }
                else
                {
                    MessageUserControl.ShowInfo("Adding", "Release Year must be 1950 to today");
                }
            }
        }

        protected void Remove_Click(object sender, EventArgs e)
        {
            //the validation that exists within the event is probably validation that 
            //   a) is not possible on the form
            //   b) required for specific events only
            //   c) for some reason, it was decided to the validation in code-behind
            if (string.IsNullOrEmpty(EditAlbumID.Text))
            {
                MessageUserControl.ShowInfo("Removing", "Search for an exsiting album before removing");
            }
            else 
            {
                MessageUserControl.TryRun(() =>
                {
                    AlbumController sysmgr = new AlbumController();
                    int rowsaffected = sysmgr.Album_Delete(int.Parse(EditAlbumID.Text));
                    if (rowsaffected == 0)
                    {
                        throw new Exception("Album no longer on file. Refresh your search.");
                    }
                    else
                    {
                        EditAlbumID.Text = "";
                        AlbumList.DataBind();
                    }

                }, "Remove", "Action successful");
            }
        }

        protected void FetchAlbums_Click(object sender, EventArgs e)
        {

        }
    }
}