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
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            using (Db db = new Db())
            {
                // объявление переменных для краткого описания slug
                string slug;
               
                //Иниц класс PageDTO
                PagesDTO dto = new PagesDTO();

                //Присвоить заголовок модели
                dto.Title = model.Title.ToUpper();
               
                //если !краткое описание то присвоить
                if(string.IsNullOrWhiteSpace(model.Slug))
                {
                    slug = model.Title.Replace(" ", "-").ToLower();

                }
                else
                {
                    slug = model.Slug.Replace(" ", "-").ToLower();
                }
                
                // убеждаемся, что заголовок и краткое описание уникальны
                if(db.Pages.Any(x => x.Title == model.Title))
                {
                    ModelState.AddModelError("", "That title allready exist.");
                        return View(model);
                }
                else if(db.Pages.Any(x => x.Slug == model.Slug))
                {
                    ModelState.AddModelError("", "That slug allready exist.");
                    return View(model);
                }
               
                // присваиваем ост  значения модели
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSideBar = model.HasSideBar;
                dto.Sorting = 100;

                // сохраняем модель в базу данных
                db.Pages.Add(dto);
                db.SaveChanges();
            }
            // передаем сообщения через TempData;
            TempData["SM"] = "You have added a new page";
            
            // преадрессовываем пользователя на метод индекс
            return RedirectToAction("Index");

        }
    }


}