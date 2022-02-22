using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASPNETLIVE.Models
{
    //[Table("categories")] // ชื่อตารางไม่ตรงกับชื่อ Model
    public class Category
    {
        //[Column("cat_id")] // ชื่อฟิลด์ไม่ตรงกับ Model
        //[Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // รันอัตโนมัส
        public int CategoryId { get; set; }

        //[Column(TypeName = "ntext")] // กำหนดชนิดข้อมูล
        [Required(ErrorMessage = "ชื่อประเภทสินค้า ห้ามว่าง")]
        public string CategoryName { get; set; } = null!; // = null! => ห้ามเป็นค่าว่าง || string? => สามารถเป็นค่าว่างได้

        public bool IsActive { get; set; } = true;
    }
}
