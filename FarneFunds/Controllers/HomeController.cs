using FarneFunds.Classes;
using FarneFunds.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Diagnostics;

namespace FarneFunds.Controllers
{
	[Authorize]
	public class HomeController : AurthorizedController
	{
		public ActionResult Index()
		{
			HomePageDetails vm = new HomePageDetails();

			var Camps = Dal.Campaigns.Where(c => c.IsActive).OrderByDescending(o => o.PinToHomePage).ThenByDescending(o => o.DateCreated).Take(10).ToList();
			var Cons = Dal.Contacts.Where(c => c.IsActive && c.DateCreated.HasValue).OrderByDescending(o => o.DateCreated).Take(10).ToList();
			var Dons = Dal.Donations.Include(i => i.Contact).Where(d => d.Contact.IsActive && d.IsActive && d.Campaign.IsActive).OrderByDescending(o => o.DateCreated).Take(10).ToList();
			var Tags = Dal.Tags.Where(t => t.IsActive).ToList();

			var pastYear = DateTime.Now.AddYears(-1);
			vm.DonationCount = (decimal)Dal.Donations.Where(d => d.IsActive && d.Campaign.IsActive && d.Contact.IsActive && d.DateDonated > pastYear).Select(s => s.Amount).ToList().Sum(s => s);
			vm.ContactCount = Dal.Contacts.Where(c => c.IsActive).Count();
			vm.CampaignCount = Dal.Campaigns.Where(c => c.IsActive && !c.IsComplete).Count();

			vm.CampaignList.AddRange(Camps.Select(s => new CampaignDetails(s, Dal)));
			vm.ContactList.AddRange(Cons.Select(s => new ContactDetails(s, Dal, Tags)));
			vm.DontaionList.AddRange(Dons.Select(s => new DonationDetails(s)));

			ViewBag.SelectedTab = (int)Enums.Controller.Home;
			return View(vm);
		}

		[HttpGet]
		public ActionResult UploadAddressFile(string password, int? totalCount, int? completeCount, bool fileUploaded = false )
		{
			if( password == "PartyDownSouth")
			{
				ViewBag.fileUploaded = fileUploaded;
				ViewBag.totalCount = totalCount;
				ViewBag.completeCount = completeCount;

				return View();
			}
			else
			{
				return RedirectToAction("Home");
			}
		}

		[HttpPost]
		public ActionResult UploadAddressFile()
		{
			var totalCount = 0;
			var completeCount = 0;

			//open file
			if (Request.Files.Count == 1)
			{
				//get file
				var postedFile = Request.Files[0];

				if (postedFile.ContentLength > 0)
				{
					//read data from input stream
					using (var csvReader = new System.IO.StreamReader(postedFile.InputStream))
					{
						string inputLine = "";

						//read each line
						while ((inputLine = csvReader.ReadLine()) != null)
						{
							if (totalCount > 0)
							{
								//get lines values
								string[] values = inputLine.Split(new char[] { ',' });

								if (!string.IsNullOrWhiteSpace(values[0]))
								{
									int contactId = 0;
									int.TryParse(values[0], out contactId);

									if (contactId != 0)
									{
										var contact = Dal.Contacts.Where(c => c.Id == contactId).FirstOrDefault();

										if (contact != null)
										{
											contact.Street = values[2].Trim();
											contact.Town = values[3].Trim();
											contact.City = values[4].Trim();
											contact.Province = values[5].Trim();
											contact.ZipCode = values[6].Trim();
											contact.Country = values[7].Trim();

											Dal.SaveChanges();

											completeCount++;
										}
									}

									Debug.WriteLine(contactId);
								}

								Debug.WriteLine(values[0]);
							}
							totalCount++;
						}

						csvReader.Close();
					}
				}
			}

			return RedirectToAction("UploadAddressFile", new { password = "PartyDownSouth", fileUploaded = true, totalCount, completeCount });
		}
	}
}