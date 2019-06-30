using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PraxiLabs
{
    public partial class DisplaySubCategory : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //    var categoryId = int.Parse(Request.QueryString[("id")]);

            //    var all = GetAll();
            //    GetTree(all,categoryId);
            if (!IsPostBack)
            {
                //DataTable dt = this.BindMenuData(0);
                var catger = GetParentCategory();
                DynamicMenuControlPopulation(catger, 0, null);
            }
        }
        public IEnumerable<PraxiLabs.Category> GetAll()
        {
            var db = new PraxiLabs.PraxiLabsDbEntities();
            var categories = db.Category.Include(C => C.Experiment);
            return categories; 
        }

        public Category GetCategory(int CategoryId)
        {
            var db = new PraxiLabs.PraxiLabsDbEntities();
            var category = GetAll().FirstOrDefault(C => C.Id == CategoryId);
            return category;
        }

        public IEnumerable<Experiment> GetCatExperiments(int CategoryId)
        {
            var db = new PraxiLabs.PraxiLabsDbEntities();
            //var experiments = GetAll().FirstOrDefault(C => C.Id == CategoryId).Experiment;
            var experiments = db.Experiment.Where(exp => exp.CategoryId == CategoryId);
            return experiments;
        }

        public IEnumerable<Category> GetParentCategory()
        {
            var db = new PraxiLabs.PraxiLabsDbEntities();
            var mainCategories = db.Category.Where(C => C.ParentId == null);
            return mainCategories;
        }

        public bool HasChild(int CategoryId)
        {
            var db = new PraxiLabs.PraxiLabsDbEntities();
            var hasChild = db.Category.Any(C => C.ParentId == CategoryId);
            return hasChild;

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
            else { return null; }
        }
        public bool HasParent(int CategoryId)
        {
            var db = new PraxiLabs.PraxiLabsDbEntities();
            var parentId = db.Category.FirstOrDefault(c => c.Id == CategoryId).ParentId;
            var hasParent = parentId != null;
            return hasParent;

        }

        private TreeNode[] GetTree(IEnumerable<Category> allCategories,int parent)
        {
            return allCategories.Where(C => C.ParentId == parent).Select(C =>
                {
                    var node = new TreeNode(C.Title);
                    Console.Write(C.Title);
                   // node.Tag = C;
                   // node.ChildNodes.AddRange(GetTree(allCategories, C.Id));
                    return node;
                }).ToArray();

        }
        protected void DynamicMenuControlPopulation(IEnumerable<Category> dt, int parentMenuId, MenuItem parentMenuItem)
        {
            string currentPage = System.IO.Path.GetFileName(Request.Url.AbsolutePath);
            if(dt==null)
            {
                return;
            }
            foreach (var row in dt)
            {
                MenuItem menuItem = new MenuItem
                {
                    Value = row.Id.ToString(),
                    Text = row.Title.ToString(),
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
                    // DataTable dtChild = this.BindMenuData(int.Parse(menuItem.Value));
                    var child = GetChildCategory(row.Id);

                    DynamicMenuControlPopulation(child, int.Parse(menuItem.Value), menuItem);

                }
            }
        }

        }
    }
