﻿using MVC_Store.Models.Data;
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

        // GET: Admin/Pages/EditPage
        [HttpGet]
        public ActionResult EditPage(int Id)
        {
            //объявляем модель PafeVM
            PageVM model;
            using (Db db = new Db())
            {
                //получаем страницу
                PagesDTO dto = db.Pages.Find(Id);

                //проверяем доступна ли страница
                if (dto == null)
                    return Content("The page does not exist.");
            
                //Иниц модель данными
                model = new PageVM(dto);
            }
            return View(model);
        }
    
        // POST: Admin/Pages/AddPage
        [HttpPost]
        public ActionResult EditPage(PageVM model)
        {
            //проверка модели на валидность
            if(!ModelState.IsValid)
            {
            return View(model);
            }
            using (Db db = new Db())
            {
                // получаем Id страницы
               int id = model.Id;


                //объяв переменную для круткого заголовка
                string slug = "home";
                //получаем страницу по id
                PagesDTO dto = db.Pages.Find(id);
                //присвоить название из полученной модели в DTO
                dto.Title = model.Title;
                // проверка краткого заголовка и присваеваем, если это необходимо;
                if(model.Slug != "home")
                {
                    if(string.IsNullOrWhiteSpace(model.Slug))
                    {
                        slug = model.Title.Replace(" ", "-").ToLower();
                    }
                    else
                    {
                        slug = model.Slug.Replace(" ", "-").ToLower();
                    }
                }

                // проверяем slug and title на уникальность
                if(db.Pages.Where(x=> x.Id != id).Any(x => x.Title == model.Title))
                {
                    ModelState.AddModelError("", "That title alredy exsist");
                    return View(model); 
                }
                else if (db.Pages.Where(x => x.Id != id).Any(x => x.Slug == slug))
                {
                    ModelState.AddModelError("", "That slug alredy exsist");
                    return View(model);
                }
                //записываем остальные значения в DTO
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSideBar = model.HasSideBar;

                //сохраняем изменения в базу
                db.SaveChanges();
            }
            //Устанавливаем сообщения в TempData
            TempData["SM"] = "You have editted the page";
            //Переадресация пользователя
            return RedirectToAction("EditPage");
        }

        // GET: Admin/Pages/PageDetails/id
        public ActionResult PageDetails(int id)
        {
            // объявляем PageVM
            PageVM model;

            using (Db db = new Db())
            {
                //получаем страницу
                PagesDTO dto = db.Pages.Find(id);

                //проверка доступности страницы
                if (dto == null)
                {
                    return Content("The page does not exsist");
                }
                //присваиваем модели информацию из бд
                model = new PageVM(dto);
            }
            //Возврат модули в представление
            return View(model);
        }
    }

}