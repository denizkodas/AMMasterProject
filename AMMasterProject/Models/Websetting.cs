using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMMasterProject
{

    [Table("Websetting")]
    public class Websetting
    {
        [Key]
        [Column("WebsettingID")]
        public int WebsettingID { get; set; }


        [StringLength(500)]
        [Column("WebsettingKey")]

        public string WebsettingKey { get; set; }




        [Column("WebsettingValue")]

        public string WebsettingValue { get; set; }



    }
}