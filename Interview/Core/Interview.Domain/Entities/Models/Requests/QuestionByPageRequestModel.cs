using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Interview.Domain.Entities.Models.Requests
{
    public class QuestionByPageRequestModel
    {


        [Required]
        [JsonPropertyName("vacantionId")]
        public int VacantionId { get; set; }

        [Required]
        [JsonPropertyName("page")]
        public int Page { get; set; }
    }


}
