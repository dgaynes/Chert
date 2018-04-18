using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Chert.Controllers
{
    public class TargetController : ApiController
    {
        // GET: api/Target
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Target/5
        public string Get(int id)
        {
            return "value";
        }
        [Route("api/target/{xy1}/{xy2}/{xy3}")]
        [HttpGet]
        public string GetTarget(string xy1, string xy2, string xy3)
        {
            //  can determine correct frame from count of indentical x, y coords presented no matter array sizes (sizes set in app config)

            try
            {
                int ColCount = Convert.ToInt32(ConfigurationManager.AppSettings["ColCount"]);
                char FinalLetter = Convert.ToChar(ConfigurationManager.AppSettings["FinalLetter"]); 

                string sTargVal = "";

                // make lists can ref by index
                List<string> oRows = new List<string>();
                for (char c = 'A'; c <= FinalLetter; c++)
                {
                    oRows.Add(c.ToString());
                }
                oRows.Reverse();

                List<int> oCols = new List<int>();
                for (int x = 0; x <= ColCount; x++)
                {
                    oCols.Add(x);
                }

                string[] sXY1 = xy1.Split(',');
                string[] sXY2 = xy2.Split(',');
                string[] sXY3 = xy3.Split(',');

                int iColReturn1 = Convert.ToInt32(sXY1[0]);
                int iColReturn2 = Convert.ToInt32(sXY2[0]);
                int iColReturn3 = Convert.ToInt32(sXY3[0]);

                int iRowReturn1 = Convert.ToInt32(sXY1[1]);
                int iRowReturn2 = Convert.ToInt32(sXY2[1]);
                int iRowReturn3 = Convert.ToInt32(sXY3[1]);

                // get column with two indentical x
                int iColFinal = 0;
                List<int> oColFinal = new List<int> { iColReturn1, iColReturn2, iColReturn3 };

                var hasTwoX = (from i in oColFinal
                               group i by i into grp
                               orderby grp.Count() descending
                               select grp.Key).First();
                int iColTwoX = (int)hasTwoX;

                var hasOneX = (from i in oColFinal
                               group i by i into grp
                               orderby grp.Count()
                               select grp.Key).First();

                int iColOneX = (int)hasOneX;

                // which way does the triangle go from the vert axis?
                if (iColOneX > iColTwoX)
                    iColFinal = 2 * iColTwoX + 1;
                else
                    iColFinal = 2 * iColTwoX;

                if (iColFinal == 0) iColFinal = 1;

                // get row with two indentical y
                int iRowFinal = 0;
                List<int> oRowFinal = new List<int> { iRowReturn1, iRowReturn2, iRowReturn3 };
                
                var hasTwoY = (from i in oRowFinal
                               group i by i into grp
                               orderby grp.Count() descending
                               select grp.Key).First();
                int iRowTwoY = (int)hasTwoY;

                var hasOneY = (from i in oRowFinal
                               group i by i into grp
                               orderby grp.Count()
                               select grp.Key).First();

                int iRowOneY = (int)hasOneY;

                // which way does the triangle go from the horiz axis?
                if (iRowOneY > iRowTwoY)
                    iRowFinal = iRowOneY;
                else
                    iRowFinal = iRowTwoY;

                sTargVal = oRows[iRowFinal - 1] + oCols[iColFinal].ToString();
                return sTargVal;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        // POST: api/Target
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Target/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Target/5
        public void Delete(int id)
        {
        }
    }
}
