namespace AMMasterProject.ViewModel
{
    public class PageViewModel
    {

        

     
    }
    #region PageDetail

    public class PageDetailViewModel
    {
        public string PageDescription { get; set;}


        public string PageName { get; set; }

        public string SEOTitle { get; set; }

        public string SEOKeyword { get; set; }

        public string SEOKeyDescription { get; set; }
    }

    #endregion


    #region FooterViewModel
    public class FooterViewModel
    {
        public List<PageCategory> Footer1 { get; set; }
        public List<PageName> Footer2 { get; set; }


    }
    #endregion
}
