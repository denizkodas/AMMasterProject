using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AMMasterProject.Pages.Admin.Blog
{

    [Authorize(Policy = "Community")]
    [BindProperties]
    public class addModel : PageModel
    {
        #region Model
        private readonly MyDbContext _dbContext;

        public Blogging blog { get; set; }

        public IEnumerable<SelectListItem> BlogCategory { get; set; }

        #endregion
        #region DI

        public addModel(MyDbContext context)
        {
            _dbContext = context;


            blog = new Blogging();
            blog.IsPublish = true;
            blog.Isaddonhomepage= true;
            blog.isfeatured = false;
           


        }
        #endregion


        #region DataPopulate    

        public void setup()
        {

            BlogCategory = _dbContext.BlogCategories
            .Where(u => u.IsPublish == true)
             .Select(u => new SelectListItem
             {
                 Value = u.BlogCategoryId.ToString(),
                 Text = u.BlogCategoryName
             }).ToList();

            if (Request.Query.ContainsKey("ID"))
            {
                int blogid = int.Parse(Request.Query["ID"].ToString());


                blog = _dbContext.Bloggings.FirstOrDefault(u => u.BlogId == blogid);


            }



        }
        #endregion
        public void OnGet()
        {
            setup();



          
        }

        public IActionResult OnPost()
        {
            int loginid = 0;
            if (User.Identity.IsAuthenticated)
            {
                loginid = int.Parse(User.FindFirst("UserID")?.Value ?? "0");
                // continue with loginid variable
            }


            #region Up-sert

            if (ModelState.IsValid)
            {
                #region Duplicat

                Blogging duplicate = _dbContext.Bloggings.FirstOrDefault(u => u.Title.ToLower().Trim() == blog.Title.ToLower().Trim() && u.BlogId != blog.BlogId);
                if (duplicate != null)
                {


                    ModelState.AddModelError("blog.Title", "Title already exist.");
                    setup();
                    return Page();
                }

                Blogging duplicateseo = _dbContext.Bloggings.FirstOrDefault(u => u.SeoPageName.ToLower().Trim() == blog.SeoPageName.ToLower().Trim() && u.BlogId != blog.BlogId);
                if (duplicateseo != null)
                {


                    ModelState.AddModelError("blog.SeoPageName", "Seo Page Name already exist.");
                    setup();
                    return Page();
                }



                #endregion



                #region Insert

                if (blog.BlogId == 0)
                {
                    Blogging insert = new Blogging();

                    insert.Title = blog.Title;
                    insert.Categoryid = blog.Categoryid;
                    insert.Image = blog.Image;
                    insert.Summary = blog.Summary;
                    insert.Description= blog.Description;  
                    insert.Isaddonhomepage = blog.Isaddonhomepage;
                    insert.isfeatured = blog.isfeatured;
                    
                    if(blog.Fileattached!=null)
                    {

                        insert.Fileattached = blog.Fileattached;
                    }
                    
                    if(blog.Externalurl!=null) { 
                    insert.Externalurl = blog.Externalurl;
                    }

                    insert.SeoPageName= blog.SeoPageName;
                    insert.SeoTitle= blog.SeoTitle;
                    insert.SeoKeyword= blog.SeoKeyword;
                    insert.SeoDescription   = blog.SeoDescription;

                  
                    insert.InsertDate = DateTime.Now;
                    insert.IsPublish = blog.IsPublish;
                    insert.ProfileId = loginid;

                    _dbContext.Bloggings.Add(insert);
                    _dbContext.SaveChanges();

                    TempData["success"] = "Blogging added successfully";
                }

                #endregion
                else
                {
                    Blogging update = _dbContext.Bloggings.FirstOrDefault(u => u.BlogId == blog.BlogId);

                    if (update != null)
                    {



                        update.Title = blog.Title;
                        update.Categoryid = blog.Categoryid;
                        update.Image = blog.Image;
                        update.Summary = blog.Summary;
                        update.Description = blog.Description;
                        update.Isaddonhomepage = blog.Isaddonhomepage;
                        update.isfeatured = blog.isfeatured;

                        if (blog.Fileattached != null)
                        {

                            update.Fileattached = blog.Fileattached;
                        }

                        if (blog.Externalurl != null)
                        {
                            update.Externalurl = blog.Externalurl;
                        }

                        update.SeoPageName = blog.SeoPageName;
                        update.SeoTitle = blog.SeoTitle;
                        update.SeoKeyword = blog.SeoKeyword;
                        update.SeoDescription = blog.SeoDescription;


                        update.InsertDate = DateTime.Now;
                        update.IsPublish = blog.IsPublish;
                        update.ProfileId = loginid;

                        _dbContext.Bloggings.Update(update);
                        _dbContext.SaveChanges();

                        TempData["success"] = "Blogging updated successfully";
                    }
                }

                #region Update

                #endregion
            }

            else
            {
                setup();
                return Page();
            }



            return RedirectToPage("/admin/blog/Index");



            #endregion
        }
    }
}
