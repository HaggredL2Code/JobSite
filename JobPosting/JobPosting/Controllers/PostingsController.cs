using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using JobPosting.DAL;
using JobPosting.Models;
using JobPosting.ViewModels;
using JobPosting.Code;

using NLog;

namespace JobPosting.Controllers
{

    [Authorize]
    public class PostingsController : Controller
    {
        private Logger logger = LogManager.GetCurrentClassLogger();
        private JBEntities db = new JBEntities();

        // GET: Postings
        public ActionResult Index(string sortDirection, string sortField, string actionButton, int? PositionID, int? JobGroupID, int? Location, int? PaymentTypeID)
        {
            PopulateDropdownList();

            var postings = db.Postings.Include(p => p.Position).Include(p => p.Position.JobGroup);
            var PostingTemplates = (from pt in db.PostingTemplates
                                    select pt.PositionID).Distinct().ToArray();
            ViewBag.JobRequirements = db.JobRequirements.OrderBy(a => a.QualificationID);
            ViewBag.JobLocations = db.JobLocations.OrderBy(a => a.LocationID);
            ViewBag.Positions = db.Positions.Where(p => PostingTemplates.Contains(p.ID));
            ViewBag.postingTemplate = db.PostingTemplates;

            if (PositionID.HasValue)
            {
                postings = postings.Where(u => u.PositionID == PositionID);

            }
            if (JobGroupID.HasValue)
            {
                postings = postings.Where(u => u.Position.JobGroupID == JobGroupID);
            }

            if (PaymentTypeID.HasValue)
            {
                postings = postings.Where(u => u.pstCompensationType == PaymentTypeID);
            }

            if (Location.HasValue)
            {
                var postingID = (from jl in db.JobLocations
                                 where jl.LocationID == Location
                                 select jl.PostingID
                                  ).ToArray();
                postings = postings.Where(p => postingID.Contains(p.ID));
            }


            postings = postings.OrderBy(p => p.pstEndDate);
                      
                        if (!String.IsNullOrEmpty(actionButton)) //Form Submitted
                        {
                            if (actionButton != "Filter")//Change of sort is requested
                            {
                                if (actionButton == sortField) //Reverse order on same field
                                {
                                    sortDirection = String.IsNullOrEmpty(sortDirection) ? "desc" : "";
                                }
                                sortField = actionButton;//Sort by the button clicked
                            }

                            if (sortField.Contains(""))
                            {
                                if (String.IsNullOrEmpty(sortDirection))
                                {
                             //       postings = postings.OrderBy(p => p.);
                                }
                                else
                                {
                          //          postings = postings.OrderByDescending(p => p.);
                                }
                            }

                        }
                        ViewBag.sortField = sortField;
                        ViewBag.sortDirection = sortDirection;
                        
            return View(postings.ToList());
        }

        // GET: Postings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                logger.Info("Details/ Bad HTTP Request with ID {0}", id);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Posting posting = db.Postings.Find(id);
            if (posting == null)
            {
                logger.Info("Details/ Failed to find Posting with ID {0}", id);
                return HttpNotFound();
            }
            ViewBag.JobRequirements = db.JobRequirements.Where(j => j.PostingID == id);
            ViewBag.JobLocations = db.JobLocations.Where(jl => jl.PostingID == id);
            ViewBag.PostingSkills = db.PostingSkills.Where(ps => ps.PostingID == id);
            return View(posting);
        }

        // GET: Postings/Create
        [Authorize(Roles = "Admin, Manager, Hiring Team")]
        public ActionResult Create(string name)
        {
            Posting posting = new Posting();
            // checking if there is any id passing to the Action.
            if (name != null)
            {
                posting = Template_fn(name, posting);
                return View(posting);
            }
            
            posting.Days = new List<Day>();
            ViewBag.Flag = false;
            PopulateDropdownList();
            PopulateListBox();
            PopulateAssignedDay(posting);
            return View();
        }



        // POST: Postings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Manager, Hiring Team")]
        public ActionResult Create([Bind(Include = "ID,pstNumPosition,pstFTE,pstSalary,pstCompensationType,pstJobDescription,pstOpenDate,pstEndDate,pstJobStartDate,pstJobEndDate,PositionID"/*,CreatedBy,CreatedOn,UpdatedBy,UpdatedOn,RowVersion*/)] Posting posting, string[] selectedQualification, string[] selectedDay, string[] selectedLocation, string[] selectedSkill, bool? SavedAsTemplate, string templateName)
        {
            try
            {
                string Locations = "";
                string Requirements = "";
                string Days = "";
                string Skills = "";
                if (SavedAsTemplate == null)
                {
                    SavedAsTemplate = false;
                }
                if (selectedQualification != null)
                {
                    foreach (var r in selectedQualification)
                    {
                        Requirements += r + ",";
                        var qualificateToAdd = db.Qualification.Find(int.Parse(r));
                        JobRequirement jobRequirement = new JobRequirement
                        {
                            Posting = posting,
                            Qualification = qualificateToAdd,
                            PostingID = posting.ID,
                            QualificationID = qualificateToAdd.ID
                        };
                        db.JobRequirements.Add(jobRequirement);

                    }
                }
                if (selectedLocation != null)
                {
                    foreach (var l in selectedLocation)
                    {
                        Locations += l + ",";
                        var locationToAdd = db.Locations.Find(int.Parse(l));
                        JobLocation jobLocation = new JobLocation
                        {
                            Posting = posting,
                            Location = locationToAdd,
                            PostingID = posting.ID,
                            LocationID = locationToAdd.ID
                        };
                        db.JobLocations.Add(jobLocation);

                    }
                }
                if (selectedSkill != null)
                {
                    foreach (var s in selectedSkill)
                    {
                        Skills += s + ",";
                        var skillToAdd = db.Skills.Find(int.Parse(s));
                        PostingSkill postingSkill = new PostingSkill
                        {
                            Posting = posting,
                            Skill = skillToAdd,
                            PostingID = posting.ID,
                            SkillID = skillToAdd.ID
                        };
                        db.PostingSkills.Add(postingSkill);
                    }
                }
                if (selectedDay != null)
                {
                    posting.Days = new List<Day>();
                    foreach (var d in selectedDay)
                    {
                        Days += d + ",";
                        var dayToAdd = db.Days.Find(int.Parse(d));
                        posting.Days.Add(dayToAdd);
                    }
                }
                
                if (ModelState.IsValid)
                {
                    if ((bool)SavedAsTemplate)
                    {
                        SavedAsTemplate_fn(templateName, posting, Requirements, Skills, Locations, Days);
                    }
                    db.Postings.Add(posting);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (RetryLimitExceededException)
            {
                logger.Error("Create/ Retry Limit Exceeded For Update");
                ModelState.AddModelError("", "Unable to save changes after multiple attemps. Try Again!");
            }
            catch (DataException dex)
            {
                logger.Error("Create/ Data Exception Error '{0}'", dex.ToString());
                if (dex.InnerException.InnerException.Message.Contains("IX_Unique_Code"))
                {
                    ModelState.AddModelError("PositionCode", "Unable to save changes. The Position Code is already existed.");
                }
                if (dex.InnerException.InnerException.Message.Contains("IX_Unique_Name"))
                {
                    ModelState.AddModelError("", "Unable to save changes. The Template Name is already existed.");
                }
                else if (dex.InnerException.InnerException.Message.Contains("IX_Unique_Desc"))
                {
                    ModelState.AddModelError("PositionDescription", "Unable to save changes. The Position can not have the same name.");
                }
                else
                {
                    ModelState.AddModelError("", "Unable to save changes. Try Again!");
                }
            }

            if (templateName != null)
            {
                posting = Template_fn(templateName, posting);
                return View(posting);
            }

            PopulateListBox();
            PopulateDropdownList(posting);
            PopulateAssignedDay(posting);
            return View(posting);
        }

        public ActionResult SavedPosting()
        {
            return View();
        }

        // GET: Postings/Edit/5
        [Authorize(Roles = "Admin, Manager, Hiring Team")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                logger.Info("Edit/ Bad HTTP Request with ID {0}", id);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Posting posting = db.Postings.Find(id);
            if (posting == null)
            {
                return HttpNotFound();
            }
            PopulateDropdownList(posting);
            PopulateListBox();
            PopulateAssignedDay(posting);

            int realID = id.Value;
            PopulateListBoxByID(realID);
            return View(posting);
        }

        // POST: Postings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Manager, Hiring Team")]
        public ActionResult EditPost(int? id, Byte[] rowVersion, string[] selectedQualification, string[] selectedDay, string[] selectedLocation, string[] selectedSkill)
        {
            int realID;
            if (id == null)
            {
                logger.Info("EditPost/ Bad HTTP Request with ID {0}", id);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            else {
                realID = id.Value;
            }
            var postingToUpdate = db.Postings
                                     .Include(p => p.JobRequirements)
                                     .Where(i => i.ID == realID)
                                     .SingleOrDefault();
            if (User.IsInRole("Hiring Team"))
            {
                if (postingToUpdate.CreatedBy != User.Identity.Name)
                {
                    logger.Info("EditPost/ Bad HTTP Gateway Creator({0}) is not User({1})", postingToUpdate.CreatedBy, User.Identity.Name);
                    return new HttpStatusCodeResult(HttpStatusCode.BadGateway);
                }
            }
            if (TryUpdateModel(postingToUpdate, "",
                new string[] { "pstNumPosition","pstFTE", "pstSalary", "pstCompensationType", "pstJobDescription", "pstOpenDate", "pstEndDate", "pstJobStartDate", "pstJobEndDate", "PositionID" }))
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        UpdatePositionQualification(selectedQualification, realID, postingToUpdate);
                        UpdatePositionDay(selectedDay, postingToUpdate);
                        UpdateLocation(selectedLocation, realID, postingToUpdate);
                        UpdateSkill(selectedSkill, realID, postingToUpdate);
                        db.Entry(postingToUpdate).OriginalValues["RowVersion"] = rowVersion;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    logger.Error("Database Concurrency Error");
                    var entry = ex.Entries.Single();
                    var clientValues = (Posting)entry.Entity;
                    var databaseEntry = entry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError("", "Unable to save changes. The Posting was deleted.");
                    }
                    else
                    {
                        var databaseValues = (Posting)databaseEntry.ToObject();
                        if (databaseValues.pstNumPosition != clientValues.pstNumPosition)
                            ModelState.AddModelError("pstNumPosition", "Current Value: " + databaseValues.pstNumPosition);
                        if (databaseValues.pstJobDescription != clientValues.pstJobDescription)
                            ModelState.AddModelError("pstJobDescription", "Current Value: " + databaseValues.pstJobDescription);
                        if (databaseValues.pstOpenDate != clientValues.pstOpenDate)
                            ModelState.AddModelError("pstOpenDate", "Current Value: " + String.Format("{0:yyyy-MM-dd HH:mm}", databaseValues.pstOpenDate));
                        if (databaseValues.pstEndDate != clientValues.pstEndDate)
                            ModelState.AddModelError("pstEndDate", "Current Value: " + String.Format("{0:yyyy-MM-dd HH:mm}", databaseValues.pstEndDate));
                        if (databaseValues.PositionID != clientValues.PositionID)
                            ModelState.AddModelError("PositionID", "Current Value: " + db.Positions.Find(databaseValues.PositionID).PositionDescription);
                        ModelState.AddModelError(string.Empty, "The record you attempted to edit was modified by another User. Your changes were not saved.");
                        postingToUpdate.RowVersion = databaseValues.RowVersion;
                    }

                }
                catch (DataException)
                {
                    logger.Error("Data Exception - Unable to save changes.");
                    ModelState.AddModelError("", "Unable to save changes. Try Again!");
                }
            }
            PopulateDropdownList(postingToUpdate);
            PopulateListBox();
            PopulateAssignedDay(postingToUpdate);
            ViewBag.JobRequirements = db.JobRequirements.Where(j => j.PostingID == realID);
            ViewBag.JobLocations = db.JobLocations.Where(l => l.PostingID == realID);
            return View(postingToUpdate);
        }

        private void UpdateSkill(string[] selectedSkill, int id, Posting postingToUpdate)
        {
            if (selectedSkill == null)
            {
                postingToUpdate.PostingSkills = new List<PostingSkill>();
                return;
            }
            var selectedSkillHS = new HashSet<string>(selectedSkill);
            var postingSkills = new HashSet<int>(db.PostingSkills.Where(s => s.PostingID == id).Select(s => s.SkillID));
            foreach (var s in db.Skills)
            {
                var SkillToUpdate = db.Skills.Find(s.ID);
                PostingSkill postingSkill = new PostingSkill
                {
                    Posting = postingToUpdate,
                    PostingID = id,
                    Skill = SkillToUpdate,
                    SkillID = s.ID
                };
                if (selectedSkillHS.Contains(s.ID.ToString()))
                {
                    if (!postingSkills.Contains(s.ID))
                    {
                        db.PostingSkills.Add(postingSkill);
                    }
                }
                else
                {
                    if (postingSkills.Contains(s.ID))
                    {
                        var selectedItem = (from i in db.PostingSkills
                                            where i.PostingID == id & i.SkillID == s.ID
                                            select i).Single();
                        db.PostingSkills.Remove(selectedItem);
                    }
                }
            }
        }

        private void UpdateLocation(string[] selectedLocation, int id, Posting postingToUpdate)
        {
            if (selectedLocation == null)
            {
                postingToUpdate.JobLocations = new List<JobLocation>();
                return;
            }
            var selectedLocationHS = new HashSet<string>(selectedLocation);
            var postingLocations = new HashSet<int>(db.JobLocations.Where(l => l.PostingID == id).Select(l => l.LocationID));

            foreach (var l in db.Locations)
            {
                var LocationToUpdate = db.Locations.Find(l.ID);
                JobLocation jobLocation = new JobLocation
                {
                    Posting = postingToUpdate,
                    Location = LocationToUpdate,
                    PostingID = id,
                    LocationID = l.ID
                };

                if (selectedLocationHS.Contains(l.ID.ToString()))
                {
                    if (!postingLocations.Contains(l.ID))
                    {
                        db.JobLocations.Add(jobLocation);
                    }
                }
                else
                {
                    if (postingLocations.Contains(l.ID))
                    {
                        var selectedItem = (from a in db.JobLocations
                                            where a.PostingID == id && a.LocationID == l.ID
                                            select a).Single();
                        db.JobLocations.Remove(selectedItem);
                    }
                }
            }

        }

        private void UpdatePositionQualification(string[] selectedQualification, int id, Posting postingToUpdate)
        {
            if (selectedQualification == null)
            {
                postingToUpdate.JobRequirements = new List<JobRequirement>();
                return;
            }

            var selectQualificationsHS = new HashSet<string>(selectedQualification);
            var PostingQualifications = new HashSet<int>(db.JobRequirements.Where(j => j.PostingID == id).Select(j => j.QualificationID));

            foreach (var q in db.Qualification)
            {
                var QualificationToUpdate = db.Qualification.Find(q.ID);
                JobRequirement jobRequirement = new JobRequirement
                {
                    Posting = postingToUpdate,
                    Qualification = QualificationToUpdate,
                    PostingID = id,
                    QualificationID = q.ID
                };

                if (selectQualificationsHS.Contains(q.ID.ToString()))
                {
                    if (!PostingQualifications.Contains(q.ID))
                    {
                        db.JobRequirements.Add(jobRequirement);
                    }
                }
                else
                {
                    if (PostingQualifications.Contains(q.ID))
                    {
                        var selectedItem = (from a in db.JobRequirements
                                            where a.PostingID == id && a.QualificationID == q.ID
                                            select a).Single();
                        db.JobRequirements.Remove(selectedItem);

                    }
                }
            }
        }

        private void UpdatePositionDay(string[] selectedDay, Posting postingToUpdate)
        {
            if (selectedDay == null)
            {
                postingToUpdate.Days = new List<Day>();
                return;
            }

            var selectedDaysHS = new HashSet<string>(selectedDay);
            var positionDays = new HashSet<int>(postingToUpdate.Days.Select(p => p.ID));

            foreach (var day in db.Days)
            {
                if (selectedDaysHS.Contains(day.ID.ToString()))
                {
                    if (!positionDays.Contains(day.ID))
                    {
                        postingToUpdate.Days.Add(day);
                    }
                }
                else
                {
                    if (positionDays.Contains(day.ID))
                    {
                        postingToUpdate.Days.Remove(day);
                    }
                }
            }
        }

        // GET: Postings/Delete/5
        [Authorize(Roles = "Admin, Manager, Hiring Team")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                logger.Info("Delete/ Bad HTTP Request with ID {0}", id);
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Posting posting = db.Postings.Find(id);
            if (User.IsInRole("Hiring Team"))
            {
                if (posting.CreatedBy != User.Identity.Name)
                {
                    logger.Info("Delete/ Bad HTTP Gateway");
                    return new HttpStatusCodeResult(HttpStatusCode.BadGateway);
                }
            }
            if (posting == null)
            {
                logger.Info("Delete/ HTTP Not Found Posting ID {0}", id);
                return HttpNotFound();
            }
            ViewBag.JobRequirements = db.JobRequirements.Where(j => j.PostingID == id).OrderBy(a => a.QualificationID);
            ViewBag.JobLocations = db.JobLocations.Where(jl => jl.PostingID == id);
            ViewBag.PostingSkills = db.PostingSkills.Where(ps => ps.PostingID == id);
            return View(posting);
        }

        // POST: Postings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Manager, Hiring Team")]
        public ActionResult DeleteConfirmed(int id)
        {
            Posting posting = db.Postings.Find(id);
            try
            {
                db.Postings.Remove(posting);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try Again!");
            }
            ViewBag.JobRequirements = db.JobRequirements.Where(j => j.PostingID == id).OrderBy(a => a.QualificationID);
            ViewBag.JobLocations = db.JobLocations.Where(jl => jl.PostingID == id);
            ViewBag.PostingSkills = db.PostingSkills.Where(ps => ps.PostingID == id);

            return View(posting);
        }

        [Authorize(Roles = "Admin, Manager, Hiring Team")]
        public ActionResult DeleteTemplate(string name)
        {
            if (name == "")
            {
                logger.Info("DeleteTemplate: Template name is blank (Bad Request)");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PostingTemplate postingTemplate = db.PostingTemplates
                                                .Where(p => p.templateName == name).SingleOrDefault();
            try
            {
                db.PostingTemplates.Remove(postingTemplate);
                db.SaveChanges();
                
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try Again!");
            }
            return RedirectToAction("Index");

        }

        private void PopulateDropdownList(Posting posting = null)
        {
            ViewBag.PositionID = new SelectList(db.Positions.OrderBy(p => p.PositionDescription), "ID", "PositionDescription", posting?.PositionID);
            ViewBag.JobGroupID = new SelectList(db.JobGroups.OrderBy(p => p.GroupTitle), "ID", "GroupTitle", posting?.Position.JobGroupID);
            ViewBag.Location = new SelectList(db.Locations.OrderBy(l => l.Address), "ID", "Address");
            ViewBag.Skill = new SelectList(db.Skills.OrderBy(s => s.SkillDescription), "ID", "SkillDescription");
            ViewBag.Requirement = new SelectList(db.Qualification.OrderBy(q => q.QlfDescription), "ID", "QlfDescription");
            ViewBag.Days = new SelectList(db.Days.OrderBy(d => d.dayOrder), "ID", "dayName");
        }

        private void PopulateDropdownListTemplate(PostingTemplate postingTemplate = null)
        {
            ViewBag.PositionID = new SelectList(db.Positions.OrderBy(p => p.PositionDescription), "ID", "PositionDescription", postingTemplate?.PositionID);
            
        }

        private void PopulateListBox()
        {
            ViewBag.Qualifications = new MultiSelectList(db.Qualification.OrderBy(q => q.QlfDescription), "ID", "QlfDescription");
            ViewBag.Locations = new MultiSelectList(db.Locations, "ID", "Address");
            ViewBag.Skills = new MultiSelectList(db.Skills, "ID", "SkillDescription");
           
        }
        private void PopulateListBoxByID(int id)
        {
            // LINQ Query select Qualification table in order to receive ID and QlfDescription that correspond to PostingID
            var qJobRequirements = (from jr in db.JobRequirements
                                    join q in db.Qualification on jr.QualificationID equals q.ID
                                    where jr.PostingID == id
                                    select q);

            // LINQ Query select Location table in order to receive ID and Address that correspond to PostingID
            var qJobLocations = (from jl in db.JobLocations
                                 join l in db.Locations on jl.LocationID equals l.ID
                                 where jl.PostingID == id
                                 select l);

            // LINKQ Query select Skill table in order to receive ID and SkillDescription that correspond to PostingID
            var qPostingSkills = (from ps in db.PostingSkills
                                  join s in db.Skills on ps.SkillID equals s.ID
                                  where ps.PostingID == id
                                  select s);

            // Create ViewBags with value is MultiSelectList
            ViewBag.JobRequirements = new MultiSelectList(qJobRequirements, "ID", "QlfDescription");
            ViewBag.JobLocations = new MultiSelectList(qJobLocations, "ID", "Address");
            ViewBag.PostingSkills = new MultiSelectList(qPostingSkills, "ID", "SkillDescription");
        }

        private void PopulateAssignedDay(Posting posting)
        {
            var allDays = db.Days;
            var pDays = new HashSet<int>(posting.Days.OrderBy(d => d.dayOrder).Select(d => d.ID));
            var viewModel = new List<DayVM>();
            foreach (var con in allDays)
            {
                viewModel.Add(new DayVM
                {
                    DayID = con.ID,
                    dayName = con.dayName,
                    Assigned = pDays.Contains(con.ID)
                });
            }
            ViewBag.Day = new MultiSelectList(viewModel, "DayID", "dayName", pDays.ToArray());
            ViewBag.SelectedDay = new MultiSelectList(posting.Days.Select(d => new { d.ID, d.dayName }), "ID", "dayName");
        }

        private void SavedAsTemplate_fn(string templateName, Posting posting, string selectedRequirements, string selectedSkills, string selectedLocations, string selectedDays)
        {
            //var postingTemplateHS = new HashSet<int>(db.PostingTemplates.Where(pt => pt.PositionID == posting.PositionID).Select(pt => pt.PositionID));

            PostingTemplate postingTemplate = new PostingTemplate
            {
                templateName = templateName,
                pstNumPosition = posting.pstNumPosition,
                pstFTE = posting.pstFTE,
                pstSalary = posting.pstSalary,
                pstCompensationType = posting.pstCompensationType,
                pstJobDescription = posting.pstJobDescription,
                pstOpenDate = posting.pstOpenDate,
                pstEndDate = posting.pstEndDate,
                pstJobStartDate = posting.pstJobStartDate,
                pstJobEndDate = posting.pstJobEndDate,
                PositionID = posting.PositionID,
                RequirementIDs = selectedRequirements,
                SkillIDs = selectedSkills,
                LocationIDs = selectedLocations,
                dayIDs = selectedDays

            };

            //if (postingTemplateHS.Contains(posting.PositionID))
            //{
            //    var postingTemplateToDelete = db.PostingTemplates.Find(posting.PositionID);
            //    db.PostingTemplates.Remove(postingTemplateToDelete);
            //}
           
            db.PostingTemplates.Add(postingTemplate);

            
            
        }

        private Posting Template_fn(string name, Posting posting)
        {
                // retrieving record from PostingTemplate table by Template Name (Unique)
                var postingTemplate = db.PostingTemplates.Where(p => p.templateName == name).SingleOrDefault();
                var positionToAdd = db.Positions.Where(p => p.ID == postingTemplate.PositionID).SingleOrDefault();
                // Assigned record of postingTemplate to Posting Object
                // to be able to use the record from postingTemplate table in View/Create
                posting = new Posting
                {
                    pstNumPosition = postingTemplate.pstNumPosition,
                    pstFTE = postingTemplate.pstFTE,
                    pstSalary = postingTemplate.pstSalary,
                    pstCompensationType = postingTemplate.pstCompensationType,
                    pstJobDescription = postingTemplate.pstJobDescription,
                    pstOpenDate = postingTemplate.pstOpenDate,
                    pstEndDate = postingTemplate.pstEndDate,
                    pstJobStartDate = postingTemplate.pstJobStartDate,
                    pstJobEndDate = postingTemplate.pstJobEndDate,
                    PositionID = postingTemplate.PositionID,
                    Position = positionToAdd

                };

                // Populatedropdownlist for PositionID
                PopulateDropdownList(posting);

                // Passing values to View/Create and Convert IDs string to List ( to be able to use Contains function)
                ViewBag.LocationIDs = ConvertStringToList.ConvertToInt(postingTemplate.LocationIDs);
                ViewBag.SkillIDs = ConvertStringToList.ConvertToInt(postingTemplate.SkillIDs);
                ViewBag.RequirementIDs = ConvertStringToList.ConvertToInt(postingTemplate.RequirementIDs);
                ViewBag.DayIDs = ConvertStringToList.ConvertToInt(postingTemplate.dayIDs);
                ViewBag.Days = db.Days;
                ViewBag.Flag = true;
                PopulateListBox();
                PopulateDropdownList(posting);
                PopulateAssignedDay(posting);


            return posting;
            
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
