namespace HardwareShop.Models.ViewModels.Reviews
{
    using System.ComponentModel.DataAnnotations;

    public class CreateReviewViewModel : ViewModelBase
    {
        public int ReviewId { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        [Range(1.0d, 5.0d)]
        public double Score { get; set; }

        public int ItemId { get; set; }
    }
}