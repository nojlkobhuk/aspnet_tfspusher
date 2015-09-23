using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.TeamFoundation.Git.CoreServices;
using NtlmGitTest;
using LibGit2Sharp;

public partial class _Default : Page
{
    private string cloneUrl = "repo/url/is/here";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            try
            {
                if (Request.QueryString["xml"] != null)
                {
                    var gitPath = Server.MapPath("xml/" + Request.QueryString["xml"] + "/.git");
                    var localPath = Server.MapPath("xml/" + Request.QueryString["xml"]);
                    var xmlFilePath = Server.MapPath("xml/" + Request.QueryString["xml"] + "/config.xml"); //Location of the XML file.
                    //Cache.Insert("key", xmlFilePath, new System.Web.Caching.CacheDependency(Server.MapPath("test.xml")));                
                    var targetBranch = Request.QueryString["xml"].Split('/');
                    switch (targetBranch[0])
                    {
                        case "DV":
                            cloneUrl = "http://tfs-web:8080/tfs/DefaultCollection/_git/DocsVision";
                            break;
                        case "MB":
                            cloneUrl = "http://tfs-web:8080/tfs/TelebankCollection/_git/MobileBank";
                            break;
                    }

                        if (Directory.Exists(gitPath))
        
                        {
                            using (new NtlmGitSession())
                            {

                                var repo = new Repository(localPath);

                                Remote remote = repo.Network.Remotes["origin"];

                                // Committer and author
                                Signature committer = new Signature("Sergey Zhurbenko", "zhurbenkoss@vtb24.ru", DateTime.Now);
                                Signature author = committer;

                                var branchName = targetBranch[1];
                                var branch = repo.Branches[branchName];

                               
                                //var branchMaster = repo.Branches["master"];
                                //repo.Checkout(branchMaster);
                                
                                //repo.Merge(branch, author, mergeOptions);
                                //repo.Network.Pull(author, pullOptions);
                                //var branchName = "origin/" + targetBranch[1];
                                //var branch = repo.Branches[branchName];
                                //repo.Checkout(branch);

                            }
                        }
                        else
                        {
                            NtlmGitTest.Program.DirectoryDeleteAll(localPath);
                            using (new NtlmGitSession())
                            {

                                //var err = Repository.Clone(cloneUrl, localPath);
                                Repository.Clone(cloneUrl, localPath);
                                var repo = new Repository(localPath);
                                // Committer and author
                                Signature committer = new Signature("Sergey Zhurbenko", "zhurbenkoss@vtb24.ru", DateTime.Now);
                                Signature author = committer;

                                var branchName = targetBranch[1];
                                var branch = repo.Branches[branchName];

                                if (branch == null)
                                {
                                    branch = repo.CreateBranch(branchName, "origin/" + branchName);
                                }

                                repo.Checkout(branch);

                                //Label1.Text = err;

            }
        }
                    
                    DataSet dataSet = new DataSet(); //create an empty dataset
                    dataSet.ReadXml(xmlFilePath); //fill it with xml file content
                    DataTable datatable = new DataTable();
                    foreach (DataTable datatbl in dataSet.Tables)
                    {
                        datatable.Merge(datatbl);

                    }
                    
                    
                    GridView1.DataSource = datatable; //first table from dataset to be loaded 

                    GridView1.DataBind(); //load the data into gridview
                    var rowsCnt = GridView1.Rows.Count;
                    DataTable dt = GridView1.DataSource as DataTable;
                    Session["TaskSet"] = dataSet;
                    Session["TaskTable"] = datatable;
                }
            }
            catch (Exception ex)
            {
                string Err = ex.Message;
                Label1.Text = Err;
            }
        }
    }
    private void BindData()
    {
        GridView1.DataSource = Session["TaskTable"];
        GridView1.DataBind();
    }
    private void GitSendChanges(string cloneUrl, string localPath, string branch)
    {
        //const string cloneUrl = "http://tfs-web:8080/tfs/DefaultCollection/_git/DocsVision";
        //var localPath = Environment.ExpandEnvironmentVariables(@"%TMP%\NtlmGitTest");

        // ensure that local folder does not exist
        if (Directory.Exists(localPath))
        //NtlmGitTest.Program.DirectoryDeleteAll(localPath);
        {
            using (new NtlmGitSession())
            {

                var repo = new Repository(localPath);
                
                Remote remote = repo.Network.Remotes["origin"];
                               
                //string content = "Test libgit2sharp commit!";
                string filePath = Path.Combine(localPath, "config.xml");

                // Committer and author
                Signature committer = new Signature("Sergey Zhurbenko", "zhurbenkoss@vtb24.ru", DateTime.Now);
                Signature author = committer;

                // Create binary stream from the text
                CommitOptions commitOptions = new CommitOptions()
                {
                    AmendPreviousCommit = false,
                    AllowEmptyCommit = false
                };

                //File.WriteAllText(filePath, "ABCDEFGH");

                repo.Index.Stage("config.xml");

                try
                {
                    Commit commit = repo.Commit("Checkin commit", author, commitOptions);
                }
                catch (Exception ex)
                {
                    Label2.Text = ex.Message;
                }

                
                repo.Network.Push(remote, @"refs/heads/" + branch);

            }
        }
        else
        {
            using (new NtlmGitSession())
            {

                var err = Repository.Clone(cloneUrl, localPath);
                var repo = new Repository(localPath);
                Label1.Text = err;

                Remote remote = repo.Network.Remotes["origin"];

                //string content = "Test libgit2sharp commit!";
                string filePath = Path.Combine(localPath, "config.xml");

                // Committer and author
                Signature committer = new Signature("Sergey Zhurbenko", "zhurbenkoss@vtb24.ru", DateTime.Now);
                Signature author = committer;

                // Create binary stream from the text
                CommitOptions commitOptions = new CommitOptions()
                {
                    AmendPreviousCommit = false,
                    AllowEmptyCommit = false
                };

                repo.Index.Stage("config.xml");

                Commit commit = repo.Commit("Checkin commit", author, commitOptions);

                repo.Network.Push(remote, @"refs/heads/" + branch);

                Commit commit_lbl = repo.Lookup<Commit>("4cf4f4");


                Label2.Text = commit_lbl.MessageShort;


            }
        }
            


    }
    protected void GridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        //Bind data to the GridView control.
        BindData();
    }

    protected void GridView_RowEditing(object sender, GridViewEditEventArgs e)
    {

        //Set the edit index.
        GridView1.EditIndex = e.NewEditIndex;      
        //GridViewRow row = GridView1.Rows[e.NewEditIndex];
        //Bind data to the GridView control.
        BindData();
    }

    protected void GridView_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        //Retrieve the table from the session object.
        var xmlFilePath = Server.MapPath("xml/" + Request.QueryString["xml"] + "/config.xml"); //Location of the XML file.
        DataSet dt = (DataSet)Session["TaskSet"];
        DataTable dtbl = new DataTable();
        GridViewRow row = GridView1.Rows[e.RowIndex];
        Object[] array = new Object[3];

        array[0] = ((TextBox)(row.Cells[2].Controls[0])).Text;
        array[1] = ((TextBox)(row.Cells[3].Controls[0])).Text;
        array[2] = ((TextBox)(row.Cells[4].Controls[0])).Text;
        //Update the values.
        DataTable table = dt.Tables[0];
        var rowCount = table.Rows.Count;
        if (e.RowIndex < rowCount)
        {
            dt.Tables[0].Rows[row.DataItemIndex].ItemArray = array;
        }
        else
        {
            dt.Tables[1].Rows[e.RowIndex - rowCount].ItemArray = array;
        }
        
        dt.AcceptChanges();
        dt.WriteXml(xmlFilePath);

        foreach (DataTable datatbl in dt.Tables)
        {
            dtbl.Merge(datatbl);

        }


        GridView1.DataSource = dtbl;
        GridView1.DataBind();
        //Reset the edit index.
        GridView1.EditIndex = -1;

        Session["TaskSet"] = dt;
        Session["TaskTable"] = dtbl;

        //Bind data to the GridView control.
        BindData();
    }

    protected void GridView_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        //Reset the edit index.
        GridView1.EditIndex = -1;
        //Bind data to the GridView control.
        BindData();
    }
    protected void OnRowDataBound(object sender, System.Web.UI.WebControls.GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            e.Row.Attributes["ondblclick"] = Page.ClientScript.GetPostBackClientHyperlink(GridView1, "Edit$" + e.Row.RowIndex);
           
        }

    }
    public void Menu1_MenuItemClick(object sender, MenuEventArgs e)
    {
        //Display the selected menu item.
        if (e.Item.Parent != null)
        {
            Label1.Text = "Pressed " + e.Item.Value + " from " + e.Item.Parent;
        }
        else
        {
            Label1.Text = "Pressed " + e.Item.Value;
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        var xmlFilePath = Server.MapPath("xml/" + Request.QueryString["xml"] + "/config.xml"); //Location of the XML file.
        var localPath = Server.MapPath("xml/" + Request.QueryString["xml"]);
        var targetBranch = Request.QueryString["xml"].Split('/');
        
        GitSendChanges(cloneUrl, localPath, targetBranch[1]);
    }
}