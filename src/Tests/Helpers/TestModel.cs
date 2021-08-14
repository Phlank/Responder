using System.ComponentModel.DataAnnotations;

namespace Phlank.Responder.Tests.Helpers
{
    public class TestModel
    {
        [Required]
        public string Required { get; set; }
        [Range(1, 2)]
        public double Between1And2 { get; set; }
    }
}