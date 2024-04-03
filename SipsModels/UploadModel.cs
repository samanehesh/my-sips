using System.ComponentModel.DataAnnotations;

namespace Sips.SipsModels
{
    public class UploadModel
    {
        [Required(ErrorMessage = "Please select a file.")]
        public IFormFile ImageFile { get; set; }
    }

}
