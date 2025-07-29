using Amazon.S3.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMMasterProject
{
    public class ItemPageDesign
    {

        [Key]
        public int ItemPageDesignID { get; set; }


        [Column("Title")]
        [StringLength(200)]
        public string Title { get; set; }

        public int SortOrder { get; set; }

        public bool Ispublish { get; set; }

        public DateTime? InsertDate { get; set; }

        public int ProfileId { get; set; }

        [Column("PageDesignMetaData")]
        public string PageDesignMetaData { get; set; }


        [Column("SelectionMetaData")]
        [StringLength(1000)]
        public string? SelectionMetaData { get; set; }

    }
}