﻿namespace Interview.Application.Mapper.DTO
{
    public class VacancyDTO_forGetandGetAll
    {
  
        public int Id { get; set; }

  
        public string Title { get; set; }


        public string Description { get; set; }


        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }


        public int PositionId { get; set; }

        public int StructureId { get; set; }

    }
}