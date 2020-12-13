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
    public class InsureeController : Controller
    {
        private InsuranceEntities1 db = new InsuranceEntities1();

        // GET: Insuree
        public ActionResult Index()
        {
            return View(db.Insurees.ToList());
        }

        // GET: Insuree/Details/5
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

        // GET: Insuree/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Insuree/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Insuree insuree, string create, string calculator)
        {
            if (create == "Create")
            {
                if (ModelState.IsValid)
                {
                    db.Insurees.Add(insuree);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            else if (calculator == "Calculate")
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
                calculate.CarModel = insuree.CarModel;
                string carModel = insuree.CarModel.ToLower();
                decimal makequote;
                if (carMake == "porsche")
                {
                    makequote = 25;
                }
                else if (carMake == "porsche" && carModel == "911 Carrera")
                {
                    makequote = 50;
                }
                else makequote = 0;

                calculate.SpeedingTickets = insuree.SpeedingTickets;
                int speedingTicket = calculate.SpeedingTickets;
                decimal ticketquote = speedingTicket * 10;

                decimal initialquote = agequote + yearquote + makequote + ticketquote + 50;

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

                return View(insuree);
            }

            return View(insuree);
        }

        // GET: Insuree/Edit/5
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

        // POST: Insuree/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType,Quote")] Insuree insuree)
        {
            if (ModelState.IsValid)
            {
                db.Entry(insuree).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(insuree);
        }

        // GET: Insuree/Delete/5
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

        // POST: Insuree/Delete/5
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

        //public ActionResult Calculate()
        //{
        //    return View(new Insuree());
        //}
        //[HttpPost]
        //public ActionResult Calculate([Bind(Include = "Id,FirstName,LastName,EmailAddress,DateOfBirth,CarYear,CarMake,CarModel,DUI,SpeedingTickets,CoverageType")] Insuree insuree)
        //{

        //    Insuree calculate = new Insuree();
        //    calculate.DateOfBirth = insuree.DateOfBirth;
            
        //    int age = DateTime.Now.Year - calculate.DateOfBirth.Year;
        //    decimal agequote;
        //    if (age <= 18)
        //    {
        //        agequote = 100;
        //    }
        //    else if (age > 18 && age < 26)
        //    {
        //        agequote = 50;
        //    }
        //    else
        //    {
        //        agequote = 25;
        //    }

        //    calculate.CarYear = insuree.CarYear;
        //    int carYear = calculate.CarYear;
        //    decimal yearquote;
        //    if (carYear < 2000 || carYear > 2015)
        //    {
        //        yearquote = 25;
        //    }
        //    else yearquote = 0;

        //    calculate.CarMake = insuree.CarMake;
        //    string carMake = calculate.CarMake.ToLower();
        //    calculate.CarModel = insuree.CarModel;
        //    string carModel = insuree.CarModel.ToLower();
        //    decimal makequote;
        //    if (carMake == "porsche")
        //    {
        //        makequote = 25;
        //    }
        //    else if (carMake == "porsche" && carModel == "911 Carrera")
        //    {
        //        makequote = 50;
        //    }
        //    else makequote = 0;

        //    calculate.SpeedingTickets = insuree.SpeedingTickets;
        //    int speedingTicket = calculate.SpeedingTickets;
        //    decimal ticketquote = speedingTicket * 10;

        //    decimal initialquote = agequote + yearquote + makequote + ticketquote + 50;

        //    calculate.DUI = insuree.DUI;
        //    bool dui = calculate.DUI;
        //    calculate.CoverageType = insuree.CoverageType;
        //    bool coverage = calculate.CoverageType;
        //    decimal specialquote;
        //    if (dui == true && coverage == true)
        //    {
        //        specialquote = initialquote * 1.75m;
        //    }
        //    else if (dui == true && coverage == false)
        //    {
        //        specialquote = initialquote * 1.25m;
        //    }
        //    else if (dui == false && coverage == true)
        //    {
        //        specialquote = initialquote * 1.5m;
        //    }
        //    else specialquote = initialquote;

        //    calculate.Quote = specialquote;

        //    return View(insuree);
        //}

        

    }
}
