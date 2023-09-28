using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Interview.Application.Mapper.DTO
{
    public class DepartmentDTO_forCreate
    {

        [Required(ErrorMessage = "DepartmentName is required")]
        public string DepartmentName { get; set; }

        [Column("BranchId_forDepartment")]
        public int BranchIdForDepartment { get; set; }


    }
}
