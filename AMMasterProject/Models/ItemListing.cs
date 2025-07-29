using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AMMasterProject
{
    public class ItemListing
    {
        [Key]
        public int ItemId { get; set; }

        [Column("ItemGUID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid? ItemGuid { get; set; }

        public int ProfileId { get; set; }


        public bool IsPublish { get; set; }

      



        public DateTime? InsertDate { get; set; }





        [Column("ItemMetaData")]
        public string ItemMetaData { get; set; }


        [Column("ItemDetailMetaData")]
        public string? ItemDetailMetaData { get; set; }


        [Column("ItemPolicyMetaData")]
        public string? ItemPolicyMetaData { get; set; }

        [Column("ItemShippingMetaData")]
        public string? ItemShippingMetaData { get; set; }

        [Column("ItemImagesMetaData")]
        public string? ItemImagesMetaData { get; set; }


        [Column("ItemDigitalMetaData")]
        public string? ItemDigitalMetaData { get; set; }


        [Column("AmenitiesMetaData")]
        public string? AmenitiesMetaData { get; set; }

        [Column("RelatedItemMetaData")]
        public string? RelatedItemMetaData { get; set; }


        [Column("VideoItemMetaData")]
        public string? VideoItemMetaData { get; set; }



        [Column("InventoryItemMetaData")]
        public string? InventoryItemMetaData { get; set; }


        [Column("ItemOtherMetaData")]
        public string? ItemOtherMetaData { get; set; }


        [Column("SellerDiscountMetaData")]
        public string? SellerDiscountMetaData { get; set; }



        [Column("ItemOtherProperites")]
        public string? ItemOtherProperites { get; set; }

        public bool IsAdminLocked { get; set; }
        public string? AdminMetaData { get; set; }
    }
}