using MVC_Store.Models.Data;
using MVC_Store.Models.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_Store.Areas.Admin.Controllers
{
    public class PagesController : Controller
    {
        // GET: Admin/Pages
        public ActionResult Index()
        {
            List<PageVM> pageList;
            // инициализируем список смисок для представления
            using (Db db = new Db())
            {
                pageList = db.Pages.ToArray().OrderBy(x => x.Sorting).Select(x => new PageVM(x)).ToList();
            }
                return View(pageList);
        }
        // GET: Admin/Pages/AddPage
        [HttpGet]
        public ActionResult AddPage()
        {
            return View();
        }
        // POST: Admin/Pages/AddPage
        [HttpPost]
        public ActionResult AddPage(PageVM model)
        {
           // проверка на валидность
           // объявление переменных для краткого описания slug
           //Иниц класс PageDTO
           //Присвоить заголовок модели
           //если !краткое описание то присвоить
           // убеждаемся, что заголовок и краткое описание уникальны
           // присваиваем ост  значения модели
           // сохраняем модель в базу данных
           // передаем сообщения через TempData;
        }
    }


}