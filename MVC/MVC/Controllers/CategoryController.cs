using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Mvc;
using MVC.Models;
using MVC.Models.Service;

namespace MVC.Controllers
{
    public class CategoryController : Controller
    {
        private DataTable dt = new DataTable();

        // GET: Category
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetCategoryAll()
        {
            // กรณีที่ Datatables กำหนดให้เป็น server side เราต้องกำหนดค่า ต่างๆ เอง
            // กำหนด จำนวน row
            // กำหนดค่า ฟิลเตอร์ row
            // กำหนดค่าค้นหาใน box search ใน datatables
            // กำหนดค่าให้แสดงกี่แถวต่อหนเา
            // กำหนดค่า การเรียงลำดับ
            // กำหนดค่า paging

            int totalrecord = 0;
            int filterrecord = 0;

            IEnumerable<CategoryModel> _data = new List<CategoryModel>();

            try
            {
                // server side parameter
                
                int start = Convert.ToInt32(Request["start"]);
                int length = Convert.ToInt32(Request["length"]);
                string searchvalue = Request["search[value]"];
                string sortcolunmname = Request["columns[" + Request["order[0][column]"] + "][name]"];
                string sortdiraction = Request["order[0][dir]"];
                //

                dt.Clear();
                CategoryService catService = new CategoryService();
                CommonService commService = new CommonService();
                dt = catService.FindCategoryByName("Str", "");
                if (dt.Rows.Count > 0)
                {
                    
                    _data = commService.ConvertToList<CategoryModel>(dt);

                    // กำหนดค่า จำนวน row
                    totalrecord = _data.Count();

                    // กำหนดให้ช่อง search ใน datatables สามารถค้นหาได้ ตามc colunm ที่กำหนด
                    _data = _data.
                        Where(x => x.CategoryName.ToLower().Contains(searchvalue.ToLower())||
                        x.Description.ToLower().Contains(searchvalue.ToLower())).ToList<CategoryModel>();

                    // กำหนดค่า ฟิลเตอร์ row
                    filterrecord = _data.Count();

                    // กำนหดค่าก่ารเรียงลำดับ  (ต้อง Add System.Linq.Dynamic libery เพิ่ม)

                    _data = _data.OrderBy(sortcolunmname + " " + sortdiraction).ToList<CategoryModel>();

                    // กำหนดค่าของ Paing
                    _data = _data.Skip(start).Take(length).ToList<CategoryModel>();



                }

            }
            catch (Exception ex)
            {

            }
            return Json(new { draw = Request["draw"], recordsTotal = totalrecord, recordsFiltered= filterrecord, data = _data},JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCategoryByName(string search)
        {
            IEnumerable<CategoryModel> _data = new List<CategoryModel>();

            try
            {
                dt.Clear();
                CategoryService catService = new CategoryService();
                CommonService commService = new CommonService();
                if (string.IsNullOrEmpty(search))
                { dt = catService.FindAllCategory(); }
                else
                { dt = catService.FindCategoryByName("Str", search); }

                if (dt.Rows.Count > 0)
                {
                    _data = commService.ConvertToList<CategoryModel>(dt);
                }


            }
            catch (Exception ex)
            {

            }

            return Json(_data, JsonRequestBehavior.AllowGet);
        }

        // GET: Category/Detail/ID
        public ActionResult Detail(string id)
        {
            CategoryModel _data = new CategoryModel();
            try
            {
                dt.Clear();
                CategoryService catService = new CategoryService();
                if (string.IsNullOrEmpty(id) == false)
                {
                    dt = catService.FindCategoryByID("Int", Convert.ToInt32(id));
                }

                if (dt.Rows.Count > 0)
                {
                    _data.CategoryID = dt.Rows[0].Field<int>(0);
                    _data.CategoryName = dt.Rows[0].Field<string>(1);
                    _data.Description = dt.Rows[0].Field<string>(2);
                }

            }
            catch (Exception ex)
            {

            }

            return View(_data);
        }

        // GET: Categories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CategoryName,Description")] CategoryModel Category)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(Category.CategoryName)==false && string.IsNullOrEmpty(Category.Description)==false)
                {
                    CategoryService catService = new CategoryService();
                    int result = catService.AddNewCategory(Category.CategoryName, Category.Description);
                    if (result > 0)
                    {
                        return Content("<script>parent.jQuery.fancybox.close();</script>");
                    }
                }
            }
            return View(Category);
        }

        public ActionResult Edit(string id)
        {
            CategoryModel _data = new CategoryModel(); 

            try
            {
                
                CategoryService catService = new CategoryService();
                CommonService comService = new CommonService();
                dt.Clear();
                if (string.IsNullOrEmpty(id) == false)
                {
                    dt = catService.FindCategoryByID("Int", Convert.ToInt16(id));
                }

                if(dt.Rows.Count >0)
                {
                    _data.CategoryID = dt.Rows[0].Field<int>(0);
                    _data.CategoryName = dt.Rows[0].Field<string>(1);
                    _data.Description = dt.Rows[0].Field<string>(2);
                }

            }
            catch (Exception ex)
            {

            }
            return View(_data);
        }

        // POST: Categories/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CategoryID,CategoryName,Description")] CategoryModel Category)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(Category.CategoryName) == false && string.IsNullOrEmpty(Category.Description) == false)
                {
                    CategoryService catService = new CategoryService();
                    int result = catService.UpdateCategoryByID(Category.CategoryID,Category.CategoryName,Category.Description) ;
                    if (result > 0)
                    {
                        return Content("<script>parent.jQuery.fancybox.close();</script>");
                    }
                }
            }
            return View(Category);
        }




    }
}
