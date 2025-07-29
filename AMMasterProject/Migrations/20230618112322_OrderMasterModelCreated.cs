using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMMasterProject.Migrations
{
    /// <inheritdoc />
    public partial class OrderMasterModelCreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Order_Charges_V2");

            migrationBuilder.DropTable(
                name: "Order_Delivery_V2");

            migrationBuilder.DropTable(
                name: "Order_Disbursement");

            migrationBuilder.DropTable(
                name: "Order_Master_V2");

            migrationBuilder.DropTable(
                name: "Order_Status_V2");

            migrationBuilder.DropTable(
                name: "Order_Variation_V2");

            migrationBuilder.DropTable(
                name: "OrderIDGeneration_V2");

            migrationBuilder.CreateTable(
                name: "OrderMasters",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BuyerId = table.Column<int>(type: "int", nullable: false),
                    SellerId = table.Column<int>(type: "int", nullable: false),
                    ItemType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    OrderStatus = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    OrderProcessStatus = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    VendorNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    AdminNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ItemMetaData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VariationMetaData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderChargesMetaData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CouponMetaData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderStatusMetaData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderMasters", x => x.OrderId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderMasters");

            migrationBuilder.CreateTable(
                name: "Order_Charges_V2",
                columns: table => new
                {
                    OrderChargeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdminNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    ChargeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ChargeType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    OrderChargeGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, defaultValueSql: "(newid())"),
                    OrderMasterGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    VendorNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order_Charges_V2", x => x.OrderChargeId);
                });

            migrationBuilder.CreateTable(
                name: "Order_Delivery_V2",
                columns: table => new
                {
                    OrderShippingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdminNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CustomerAddressGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DiscountCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DiscountType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    OrderMasterGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OrderShippingGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, defaultValueSql: "(newid())"),
                    PaymentDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    PaymentMode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PaymentReferenceId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    VendorNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order_Delivery_V2", x => x.OrderShippingId);
                });

            migrationBuilder.CreateTable(
                name: "Order_Disbursement",
                columns: table => new
                {
                    OrderEarningId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DisburseDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DisbursementId = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DisbursementMode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    InsertBy = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    OrderId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order_Disbursement", x => x.OrderEarningId);
                });

            migrationBuilder.CreateTable(
                name: "Order_Master_V2",
                columns: table => new
                {
                    OrderMasterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActualPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BuyerId = table.Column<int>(type: "int", nullable: true),
                    BuyerNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CommisionAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CommissionPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ConversionCurrencySymbol = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CouponCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CurrencyConversionRate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    CurrencyId = table.Column<int>(type: "int", nullable: true),
                    DeliveryStatusId = table.Column<int>(type: "int", nullable: true),
                    GroupOrderGeneratedGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsCoupon = table.Column<bool>(type: "bit", nullable: true),
                    LastOrderStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OrderMasterCustomGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, defaultValueSql: "(newid())"),
                    OrderMasterGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, defaultValueSql: "(newid())"),
                    OrderStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    PaymentMode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PaymentReferenceId = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ProductAttributePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ProductDiscount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ProductId = table.Column<int>(type: "int", nullable: true),
                    ProductJobAppliedGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProductPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    QTY = table.Column<int>(type: "int", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    VendorNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order_Master_V2", x => x.OrderMasterId);
                });

            migrationBuilder.CreateTable(
                name: "Order_Status_V2",
                columns: table => new
                {
                    OrderStatusId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdminNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    GeneralSetupId = table.Column<int>(type: "int", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    OrderMasterGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OrderMasterId = table.Column<int>(type: "int", nullable: true),
                    OrderStatusGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, defaultValueSql: "(newid())"),
                    Statustype = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    VendorNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order_Status_V2", x => x.OrderStatusId);
                });

            migrationBuilder.CreateTable(
                name: "Order_Variation_V2",
                columns: table => new
                {
                    OrderVariationId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdminNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Attributeprice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    InsertDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    OrderMasterGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OrderVariationGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, defaultValueSql: "(newid())"),
                    ProductAttributeGUID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProductAttributeOptionId = table.Column<int>(type: "int", nullable: true),
                    ProductAttributeSelectedOption = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    VendorNotes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order_Variation_V2", x => x.OrderVariationId);
                });

            migrationBuilder.CreateTable(
                name: "OrderIDGeneration_V2",
                columns: table => new
                {
                    ordergeneratedguidid = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    insertdate = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__OrderIDG__CFBD77621CD6197B", x => x.ordergeneratedguidid);
                });
        }
    }
}
