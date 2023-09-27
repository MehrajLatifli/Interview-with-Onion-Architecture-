using Interview.Domain.Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interview.Application.Mapper.DTO
{
    public class SectorDTO_forCreate
    {


        [Required(ErrorMessage = "SectorName is required")]
        public string SectorName { get; set; }



    }
}
