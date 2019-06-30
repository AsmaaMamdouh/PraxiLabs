using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace PraxiLabs
{
    public partial class DisplayExperiments : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    var categoryId = int.Parse(Request.QueryString[("id")]);
            //}
        }
        public IEnumerable<Experiment> GetCatExperiments()
            {
            var CategoryId = int.Parse(Request.QueryString[("id")]);
            var db = new PraxiLabs.PraxiLabsDbEntities();
                //var experiments = GetAll().FirstOrDefault(C => C.Id == CategoryId).Experiment;
                var experiments = db.Experiment.Where(exp => exp.CategoryId == CategoryId);
                return experiments;
            }
    }
}