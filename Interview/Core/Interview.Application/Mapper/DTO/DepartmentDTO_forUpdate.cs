using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Interview.Application.Mapper.DTO
{
    public class DepartmentDTO_forUpdate
    {

        [Required(ErrorMessage = "Id is required")]
        public int Id { get; set; }



        [Required(ErrorMessage = "DepartmentName is required")]
        public string DepartmentName { get; set; }

        [Column("BranchId_forDepartment")]
        public int BranchId { get; set; }




    }
}
