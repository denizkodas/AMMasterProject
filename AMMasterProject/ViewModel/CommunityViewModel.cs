namespace AMMasterProject.ViewModel
{
    public class CommunityViewModel
    {
    }

    #region Blog
    public class BlogViewModel
    {



        public int BlogId { get; set; }

        public string? SpanTitle { get; set; }
        public string? Title { get; set; }

        public string? Summary { get; set; }

        public string? Description { get; set; }
        public string? Image { get; set; }

        public string? Category { get; set; }

        public string? SEOCategory { get; set; }
        public DateTime? InsertDate { get; set; }

        public bool IsPublish { get; set; }
        public bool Isaddonhomepage { get; set; }

        public bool IsFeatured { get; set; }

        public string SeoPageName { get; set; }

        public string SeoPageTitle { get; set; }
        public string SeoPageKeyword { get; set; }

        public string SeoPageDescription { get; set; }

    }
    #endregion
    #region Career

    public class CareerListView
    {
        public int CareerId { get; set; }

        public Guid CareerGuid { get; set; }

        public string? Title { get; set; }

        public string? Category { get; set; }


        public DateTime? InsertDate { get; set; }

        public bool IsPublish { get; set; }


        public int? TotalApplication { get; set; }

    }

    #endregion


    #region Event
    public class EventListView
    {
        public int EventId { get; set; }

        public Guid EventGuid { get; set; }

        public string? Title { get; set; }


        public DateTime? EventStartDate { get; set; }

        public string? Image { get; set; }

        public string? Category { get; set; }


        public DateTime? InsertDate { get; set; }

        public bool IsPublish { get; set; }


        public int? TotalRegistration { get; set; }

    }
    #endregion

    #region Category

    public class CommunityCategory
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public string SeoURL { get; set; }
       
    }
    #endregion
}
