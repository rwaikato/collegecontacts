using FarneFunds.Database;
using FarneFunds.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FarneFunds.Controllers
{
    public class TagController : AurthorizedController
    {
        //
        // GET: /Tag/
        public ActionResult Index()
        {
            TagList vm = new TagList(Dal, true);
            return View(vm);
        }

        public ActionResult ArchivedIndex()
        {
            TagList vm = new TagList(Dal, false);
            return View(vm);
        }

        [HttpGet]
        public ActionResult Create()
        {
            TagDetails vm = new TagDetails();
            return View(vm);
        }

        [HttpPost]
        public ActionResult Create( TagDetails vm )
        {
            if( ModelState.IsValid)
            {
                Tag SaveTag = new Tag();
                SaveTag.Name = vm.Name;
                SaveTag.IsActive = true;

                Dal.Tags.Add(SaveTag);
                Dal.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(vm);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            Tag DbTag = Dal.Tags.Where(t => t.IsActive && t.Id == id).First();
            TagDetails vm = new TagDetails(DbTag);

            return View(vm);
        }

        [HttpPost]
        public ActionResult Edit(TagDetails vm)
        {
            if (ModelState.IsValid)
            {
                Tag DbTag = Dal.Tags.Where(t => t.IsActive && t.Id == vm.Id).First();
                DbTag.Name = vm.Name;

                Dal.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(vm);
        }


        [HttpGet]
        public ActionResult Archive(int id)
        {
            Tag DbTag = Dal.Tags.Where(t => t.IsActive && t.Id == id).First();
            TagDetails vm = new TagDetails(DbTag);

            return View(vm);
        }

        [HttpPost]
        public ActionResult Archive(TagDetails vm)
        {
            if (ModelState.IsValid)
            {
                Tag DbTag = Dal.Tags.Where(t => t.IsActive && t.Id == vm.Id).First();
                DbTag.IsActive = false;

                Dal.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(vm);
        }

        [HttpGet]
        public ActionResult UnArchive(int id)
        {
            Tag DbTag = Dal.Tags.Where(t => t.IsActive == false && t.Id == id).First();
            DbTag.IsActive = true;

            Dal.SaveChanges();

            return RedirectToAction("Index");
        }
	}
}