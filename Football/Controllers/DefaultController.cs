using Football.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Football.Controllers
{
    public class DefaultController : Controller
    {
        SoccerContext db = new SoccerContext();
        // GET: Default
        public ActionResult Index()
        {
            var players = db.Players.Include(p => p.Team);
            return View(players.ToList());
        }
public ActionResult ListTeams()
        {
            return View(db.Teams);
        }
        public ActionResult TeamDetails(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Team team = db.Teams.Include(p => p.Players).FirstOrDefault(t => t.Id == id);
            if (team == null)
            {
                return HttpNotFound();
            }
            return View(team);
        }
        [HttpGet]
        public ActionResult Create()
        {
            SelectList team = new SelectList(db.Teams, "Id", "Name");
            ViewBag.Teams = team;
            return View();
            
        }
        [HttpPost]
        public ActionResult Create(Player player)
        {
            db.Players.Add(player);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");// Временно так
            }
            SelectList team = new SelectList(db.Teams, "Id", "Name");
            ViewBag.Teams = team;
            Player player = db.Players.Include(p => p.Team).FirstOrDefault(p => p.Id == id);

            return View(player);
        }
        [HttpPost]
        public ActionResult Edit(Player player)
        {

            db.Entry(player).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");

        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Player player = db.Players.Include(p => p.Team).FirstOrDefault(p => p.Id == id);
            return View(player);
        }
        [HttpPost,ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return HttpNotFound();
            }
            db.Players.Remove(player);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}