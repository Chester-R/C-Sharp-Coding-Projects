using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarInsurance.Models;

namespace CarInsurance.Controllers
{
    public class AdminController : Controller
    {
        private InsuranceEntities1 db = new InsuranceEntities1();

        // GET: Admin
        public ActionResult Index()
        {
            return View(db.Insurees.ToList());
        }

        // GET: Admin/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // GET: Admin/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // POST: Admin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                Calculator(insuree, "Save");
                db.Entry(insuree).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(insuree);
        }

        // GET: Admin/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insuree insuree = db.Insurees.Find(id);
            if (insuree == null)
            {
                return HttpNotFound();
            }
            return View(insuree);
        }

        // POST: Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Insuree insuree = db.Insurees.Find(id);
            db.Insurees.Remove(insuree);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        public ActionResult Calculator(Insuree insuree, string calculator)
        {
            Insuree calculate = new Insuree();

            calculate.DateOfBirth = insuree.DateOfBirth;
            int age = DateTime.Now.Year - calculate.DateOfBirth.Year;
            decimal agequote;
            if (age <= 18)
            {
                agequote = 100;
            }
            else if (age > 18 && age < 26)
            {
                agequote = 50;
            }
            else
            {
                agequote = 25;
            }

            calculate.CarYear = insuree.CarYear;
            int carYear = calculate.CarYear;
            decimal yearquote;
            if (carYear < 2000 || carYear > 2015)
            {
                yearquote = 25;
            }
            else yearquote = 0;

            calculate.CarMake = insuree.CarMake;
            string carMake = calculate.CarMake.ToLower();
            decimal makequote;
            if (carMake == "porsche")
            {
                makequote = 25;
            }
            else makequote = 0;

            calculate.CarModel = insuree.CarModel;
            string carModel = insuree.CarModel.ToLower();
            decimal modelquote;
            if (carModel == "911carrera" || carModel == "911 Carrera" || carModel == "911 carrera" || carModel == "911Carrera")
            {
                modelquote = 25;
            }
            else modelquote = 0;

            calculate.SpeedingTickets = insuree.SpeedingTickets;
            int speedingTicket = calculate.SpeedingTickets;
            decimal ticketquote = speedingTicket * 10;

            decimal initialquote = agequote + yearquote + makequote + modelquote + ticketquote + 50;

            calculate.DUI = insuree.DUI;
            bool dui = calculate.DUI;
            calculate.CoverageType = insuree.CoverageType;
            bool coverage = calculate.CoverageType;
            decimal specialquote;
            if (dui == true && coverage == true)
            {
                specialquote = initialquote * 1.75m;
            }
            else if (dui == true && coverage == false)
            {
                specialquote = initialquote * 1.25m;
            }
            else if (dui == false && coverage == true)
            {
                specialquote = initialquote * 1.5m;
            }
            else specialquote = initialquote;

            insuree.Quote = specialquote;

            if (calculator == "Calculate")
            {
                return View(insuree);
            }
            else return View(insuree.Quote);
        }
    }
}
