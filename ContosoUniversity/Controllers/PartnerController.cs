using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ContosoUniversity.DAL;
using ContosoUniversity.Models;
using System.Data.Entity.Infrastructure;

namespace ContosoUniversity.Controllers
{
    public class PartnerController : Controller
    {
        private SchoolContext db = new SchoolContext();

        // GET: Partner
        public ActionResult Index()
        {
            return View();
        }

        // GET: Partner/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Commenting out original code to show how to use a raw SQL query.
            //Partner partner = await db.Partners.FindAsync(id);

            // Create and execute raw SQL query.
            string query = "SELECT * FROM Partner WHERE PartnerID = @p0";
            Partner partner = await db.Partners.SqlQuery(query, id).SingleOrDefaultAsync();

            if (partner == null)
            {
                return HttpNotFound();
            }
            return View(partner);
        }

        // GET: Partner/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Partner/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "PartnerID,Name")] Partner partner)
        {
            if (ModelState.IsValid)
            {
                db.Partners.Add(partner);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            
            return View(partner);
        }

        // GET: Partner/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Partner partner = await db.Partners.FindAsync(id);
            if (partner == null)
            {
                return HttpNotFound();
            }

            return View(partner);
        }

        // POST: Partner/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? id, byte[] rowVersion)
        {
            string[] fieldsToBind = new string[] { "Name" };

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var partnerToUpdate = await db.Partners.FindAsync(id);
            if (partnerToUpdate == null)
            {
                Partner deletedPartner = new Partner();
                TryUpdateModel(deletedPartner, fieldsToBind);
                ModelState.AddModelError(string.Empty,
                    "Unable to save changes. The partner was deleted by another user.");

                return View(deletedPartner);
            }

            if (TryUpdateModel(partnerToUpdate, fieldsToBind))
            {
                try
                {
                    db.Entry(partnerToUpdate).OriginalValues["RowVersion"] = rowVersion;
                    await db.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var entry = ex.Entries.Single();
                    var clientValues = (Partner)entry.Entity;
                    var databaseEntry = entry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty,
                            "Unable to save changes. The partner was deleted by another user.");
                    }
                    else
                    {
                        var databaseValues = (Partner)databaseEntry.ToObject();

                        if (databaseValues.Name != clientValues.Name)
                            ModelState.AddModelError("Name", "Current value: "
                                + databaseValues.Name);
                    }
                }
                catch (RetryLimitExceededException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }

            return View(partnerToUpdate);
        }

        // GET: Partner/Delete/5
        public async Task<ActionResult> Delete(int? id, bool? concurrencyError)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Partner partner = await db.Partners.FindAsync(id);
            if (partner == null)
            {
                if (concurrencyError.GetValueOrDefault())
                {
                    return RedirectToAction("Index");
                }
                return HttpNotFound();
            }

            if (concurrencyError.GetValueOrDefault())
            {
                ViewBag.ConcurrencyErrorMessage = "The record you attempted to delete "
                    + "was modified by another user after you got the original values. "
                    + "The delete operation was canceled and the current values in the "
                    + "database have been displayed. If you still want to delete this "
                    + "record, click the Delete button again. Otherwise "
                    + "click the Back to List hyperlink.";
            }

            return View(partner);
        }

        // POST: Partner/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Partner partner)
        {
            try
            {
                db.Entry(partner).State = EntityState.Deleted;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToAction("Delete", new { concurrencyError = true, id = partner.PartnerID });
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name after DataException and add a line here to write a log.
                ModelState.AddModelError(string.Empty, "Unable to delete. Try again, and if the problem persists contact your system administrator.");
                return View(partner);
            }
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
