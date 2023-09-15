using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MyWebApplication.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Name Can not be blank!!")]
        public string Name { get; set; }

        [Required(ErrorMessage ="Display Order Can not be blank!!!")]
        [DisplayName("Display Name")]
        public int DisplayOrder { get; set; }
        public DateTime CreateDateTime { get; set; }= DateTime.Now;

    }
}
