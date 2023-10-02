using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Interview.Application.Mapper.DTO
{
    public class DepartmentDTO_forCreate
    {

        [Required(ErrorMessage = "Department name is required")]
        public string DepartmentName { get; set; }

        [Column("BranchId_forDepartment")]
        public int BranchId { get; set; }


    }
}
