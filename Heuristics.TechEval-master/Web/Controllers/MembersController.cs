using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Heuristics.TechEval.Core;
using Heuristics.TechEval.Web.Models;
using Heuristics.TechEval.Core.Models;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace Heuristics.TechEval.Web.Controllers {

	public class MembersController : Controller {

		private readonly DataContext _context;

		public MembersController() {
			_context = new DataContext();
		}

		public ActionResult List(string dbRet="") {
			var allMembers = _context.Members.ToList();

			ViewData["AllMembers"] = allMembers;
			ViewData["DBError"] = dbRet;

			//return View(allMembers);
			return View();
		}

		[HttpPost]
		public ActionResult New(NewMember data) {
			if (CheckDupEmail(data.Email))
			{
				var newMember = new Member {
					Name = data.Name,
					Email = data.Email
					//Category = 1
				};

				_context.Members.Add(newMember);
				_context.SaveChanges();
				return Json(JsonConvert.SerializeObject(newMember));
			}
			else
            {
				//				TempData["ErrorMessage"] = "Email already exists.  Update aborted.";
				//				TempData["displayModal"] = "NewMemberModal";
				//				return View();
				//				ViewBag.Message = "Email already exists.  Update aborted.";
				//				return RedirectToAction("Error");
				string strRet = "Email already exists.  Please check and try again.";

				return RedirectToAction("List", new { dbRet = strRet });
			}
		}

		[HttpPost]
		public ActionResult Update()
		{
			var exisitingMember = _context.Members.Find(Convert.ToInt32(Request["mid"]));
			string strRet = "";

			if (CheckDupEmail(Request["Email"]))
			{
				exisitingMember.Name = Request["Name"];
				exisitingMember.Email = Request["Email"];

				_context.SaveChanges();
			}
			else 
				strRet = "Email already exists.  Please check and try again.";

			return RedirectToAction("List", new { dbRet = strRet });
		}

		[HttpPost]
		public ActionResult Delete()
		{
			var deletedMember = new Member() { Id = Convert.ToInt32(Request["delId"]) };

			_context.Members.Attach(deletedMember);
			_context.Members.Remove(deletedMember);
			_context.SaveChanges();

			return RedirectToAction("List");
		}


		public bool CheckDupEmail(string enteredEmail)
        {
			int cnt = 0;

			using (var connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Database=master;Integrated Security=True;MultipleActiveResultSets=True"))
			{
				connection.Open();
				var command = connection.CreateCommand();
				command.CommandText = "SELECT dbo.GetEmailCount(@enteredEmail) AS EmailCount";
				command.Parameters.AddWithValue("@enteredEmail", enteredEmail);
				SqlDataReader reader = command.ExecuteReader();
				while (reader.Read())
				{
					cnt = Int32.Parse(reader["EmailCount"].ToString()); 
				}
				reader.Close();
				command.Dispose();
				connection.Close();
			};

			if (cnt == 0)
				return true;
			else
				return false;
		}

	}
}