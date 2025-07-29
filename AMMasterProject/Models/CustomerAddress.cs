using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AMMasterProject;

[Table("Customer_Address")]
public partial class CustomerAddress
{
    [Key]
    public int CustomerAddressId { get; set; }

    [Column("CustomerAddressGUID")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid? CustomerAddressGuid { get; set; }

    [StringLength(200)]
    [DisplayName("First Name")]
    [Required(ErrorMessage = "Full Name Is Required")]
    //[RegularExpression("^[A-Za-z]*$", ErrorMessage = "Only characters are allowed.")]
    public string FirstName { get; set; }

    [StringLength(200)]
    [DisplayName("Last Name")]
    //[Required(ErrorMessage = "Last Name Is Required")]
    //[RegularExpression("^[A-Za-z]*$", ErrorMessage = "Only characters are allowed.")]
    public string? LastName { get; set; }

    [StringLength(300)]

    [DisplayName("Email")]
    [Required(ErrorMessage = "Email Is Required")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]

    public string Email { get; set; }

    [StringLength(500)]
    [DisplayName("Address")]
    [Required(ErrorMessage = "Address Is Required")]
    public string Address { get; set; }



    [StringLength(300)]
    [DisplayName("HouseNumber")]
    //[Required(ErrorMessage = "House Number Is Required")]
    public string? HouseNumber { get; set; }

    [StringLength(100)]
    [DisplayName("Latitude")]
    [Required(ErrorMessage = "Latitude Is Required")]
    public string Latitude { get; set; }

    [StringLength(100)]
    [DisplayName("Longitude")]
    [Required(ErrorMessage = "Longitude Is Required")]
    public string Longitude { get; set; }

    [StringLength(1000)]
    [DisplayName("Country")]
    [Required(ErrorMessage = "Country Is Required")]
    public string Country { get; set; }

    [StringLength(100)]
    [DisplayName("State")]
    [Required(ErrorMessage = "State Is Required")]
    public string State { get; set; }

    [StringLength(200)]
    [DisplayName("Street")]
    //[Required(ErrorMessage = "Street Is Required")]
    public string? Street { get; set; }

    [StringLength(200)]
    [DisplayName("City")]
    [Required(ErrorMessage = "City Is Required")]
    public string City { get; set; }

    [StringLength(200)]
    [DisplayName("Zip Code")]
    //[Required(ErrorMessage = "Zip Code Is Required")]
    public string? Zipcode { get; set; }

    [StringLength(50)]
    [DisplayName("Phone")]
    [Required(ErrorMessage = "Phone Is Required")]
    [RegularExpression(@"^\d+$", ErrorMessage = "Phone must contain only numeric digits.")]
    public string Phone { get; set; }


    [StringLength(50)]
    [DisplayName("AddressType")]
    [Required(ErrorMessage = "Address Type Is Required")]
    
    public string AddressType { get; set; }
    public int BuyerId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? InsertDate { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime? UpdatedDate { get; set; }

    public bool IsActive { get; set; }


    //[NotMapped]
    //public bool IsZipCodeHide { get; set; } =false;

    //[NotMapped]
    //public bool IsStreetHide { get; set; } = false;
}
