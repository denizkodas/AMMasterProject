using System.ComponentModel.DataAnnotations;

namespace AMMasterProject.Models
{
    public class ItemPageDesignChild
    {
        [Key]
        public int ItemPageDesignChildID { get; set; }


        public int ItemPageDesignID { get; set; }

        public int SelecttionID { get; set; }


        [StringLength(50)]
        public string Selectiontype { get; set; }
        

        public DateTime? InsertDate { get; set; }

       

     

    }
}
