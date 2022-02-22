using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPNETLIVE.Models
{
    public class Product
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // รันอัตโนมัส
        public int ProductId { get; set; }

        [Required(ErrorMessage = "ชื่อสินค้า ห้ามว่าง")]
        public string ProductName { get; set; } = null!; 

        [Required(ErrorMessage = "ราคา ห้ามว่าง")]
        [Range(0.00, 9999999999.99, ErrorMessage = "ข้อมูลราคา ไม่ถูกต้อง")]
        [Column(TypeName = "decimal(10, 2)")] // 8 หลัก ทศนืยม 2 ตำแหน่ง
        public decimal ProductPrice { get; set; }

        [Column(TypeName = "datetime")] // col type sql server
        public DateTime ProductExpire { get; set; } = DateTime.Now.AddMonths(3); // ให้หมดอายุอีก 3 เดือนข้างหน้า

        //FK 
        //[ForeignKey("cat_id")]
        [Required(ErrorMessage = "รหัสประเภทสินค้า ห้ามว่าง")]
        public int CategoryId { get; set; }

        //Relation => ไปที่ Model อะไร
        //many to one
        public Category? Category { get; set; }
    }
}
