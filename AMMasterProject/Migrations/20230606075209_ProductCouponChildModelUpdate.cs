using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AMMasterProject.Migrations
{
    /// <inheritdoc />
    public partial class ProductCouponChildModelUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CouponTypeId",
                table: "Product_Coupon_Child");

            migrationBuilder.DropColumn(
                name: "ProductCouponGUID",
                table: "Product_Coupon_Child");

            migrationBuilder.AlterColumn<int>(
                name: "ReferenceId",
                table: "Product_Coupon_Child",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductCouponId",
                table: "Product_Coupon_Child",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ReferenceType",
                table: "Product_Coupon_Child",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductCouponId",
                table: "Product_Coupon_Child");

            migrationBuilder.DropColumn(
                name: "ReferenceType",
                table: "Product_Coupon_Child");

            migrationBuilder.AlterColumn<int>(
                name: "ReferenceId",
                table: "Product_Coupon_Child",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "CouponTypeId",
                table: "Product_Coupon_Child",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductCouponGUID",
                table: "Product_Coupon_Child",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}
