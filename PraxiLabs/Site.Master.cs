using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PraxiLabs
{
    public partial class SiteMaster : MasterPage
    {
               
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!IsPostBack)
            {
                DropDownList1.DataTextField = "Title";
                DropDownList1.DataValueField = "Id";
                DropDownList1.DataSource = GetParentCategory().ToList();
                
                DropDownList1.DataBind();
                var catger = GetParentCategory();
                DynamicMenuControlPopulation(catger, 0, null);
            }
        }
        public IEnumerable<Category> GetParentCategory()
        {
            var db=new PraxiLabs.PraxiLabsDbEntities();
            var mainCategories = db.Category. Where(C => C.ParentId == null);
            return mainCategories;
            
        }

        protected void DynamicMenuControlPopulation(IEnumerable<Category> dt, int parentMenuId, MenuItem parentMenuItem)
        {
            string currentPage = System.IO.Path.GetFileName(Request.Url.AbsolutePath);
            if (dt == null)
            {
                return;
            }
            foreach (var row in dt)
            {
                MenuItem menuItem = new MenuItem
                {
                    Value = row.Id.ToString(),
                    Text = row.Title.ToString(),
                    NavigateUrl =!HasChild(row.Id) ? "DisplayExperiments.aspx?id="+row.Id:"#",
                    Selected = row.Id == dt.FirstOrDefault().Id ? true : false,
                };
                if (parentMenuId == 0)
                {
                    Menu1.Items.Add(menuItem);
                    // var dtChild = (int.Parse(menuItem.Value));
                    var child = GetChildCategory(row.Id);
                    DynamicMenuControlPopulation(child, int.Parse(menuItem.Value), menuItem);
                }
                else
                {
                    parentMenuItem.ChildItems.Add(menuItem);
                    var child = GetChildCategory(row.Id);

                    DynamicMenuControlPopulation(child, int.Parse(menuItem.Value), menuItem);

                }
            }
           
        }
        public IEnumerable<Category> GetChildCategory(int CategoryId)
            {
                //IEnumerable<Category> child;
                if (HasChild(CategoryId) == true)
                {
                    var db = new PraxiLabs.PraxiLabsDbEntities();
                    var child = db.Category.Where(C => C.ParentId == CategoryId);
                    //foreach (var item in child) { GetChildCategory(item.Id); }
                    return child;
                }
                else
                {
                return null;
                }
        }
        public bool HasChild(int CategoryId)
        {
            var db = new PraxiLabs.PraxiLabsDbEntities();
            var hasChild = db.Category.Any(C => C.ParentId == CategoryId);
            return hasChild;

        }
        
        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var id = DropDownList1.SelectedItem.Text;
            Label1.Text = id;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            var id = int.Parse(DropDownList1.SelectedValue);
            var catger = GetParentCategory().ToList();
            var category = GetallChildCategory(id);
            if (category != null)
            {
                GridView1.DataSource = GetallChildCategory(id).ToList();

                GridView1.DataBind();
                Label1.Text = "";
                GridView1.Style.Remove("display");
            }
            //Label1.Text = DropDownList1.SelectedValue;
            else
            {
                GridView1.Style.Add("display", "none");
                Label1.Text = "There are no SubCat";
            }
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            var id = int.Parse(DropDownList1.SelectedValue);
            var catger = GetParentCategory().ToList();
            var category = GetallChildCategory(id);
            var db = new PraxiLabs.PraxiLabsDbEntities();

            var experiment = db.Experiment.Where(a => a.CategoryId == id);
            if (category != null)
            {
                GridView1.DataSource = GetExperiment(id).ToList();

                GridView1.DataBind();
                Label1.Text = "";
                GridView1.Style.Remove("display");
            }
            else if(experiment!=null)
            {
                GridView1.DataSource = experiment.ToList();

                GridView1.DataBind();
                Label1.Text = "";
                GridView1.Style.Remove("display");
            }
            else
            {
                GridView1.Style.Add("display", "none");
                Label1.Text = "There are no Experiments";
            }
        }
        public IEnumerable<Experiment> GetCatExperiments(int CategoryId)
        {
            var db = new PraxiLabs.PraxiLabsDbEntities();
            var experiments = db.Experiment.Where(exp => exp.CategoryId == CategoryId);
            return experiments;
        }

        public IEnumerable<Category> GetallChildCategory(int CategoryId)
        {
            //IEnumerable<Category> child;
            if (HasChild(CategoryId) == true)
            {
                var db = new PraxiLabs.PraxiLabsDbEntities();
                var listofid = db.Category.Where(C => C.ParentId == CategoryId).Select(a => a.Id).ToList();
                var child = db.Category.Where(C => C.ParentId == CategoryId || listofid.Contains(C.ParentId.Value));
                return child;
            }
            else { return null; }
        }
      


        public IEnumerable<Experiment> GetExperiment(int CategoryId)
        {
            //IEnumerable<Category> child;
            if (HasChild(CategoryId) == true)
            {
                var db = new PraxiLabs.PraxiLabsDbEntities();
                var listofid = db.Category.Where(C => C.ParentId == CategoryId).Select(a => a.Id).ToList();
                var child = db.Experiment.Where(C => listofid.Contains(C.Category.ParentId.Value));
                
                return child;
            }
            else { return null; }
        }

    }
}