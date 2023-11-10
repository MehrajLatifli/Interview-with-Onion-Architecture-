using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Interview.Domain.Entities.Requests
{
    public class RegisterUserRequestModel
    {


        [Required]
        [JsonPropertyName("customRoles")]
        public string customRoles { get; set; }

        [Required]
        [JsonPropertyName("roleAccesstype")]
        public int roleAccesstype { get; set; }
    }


}
