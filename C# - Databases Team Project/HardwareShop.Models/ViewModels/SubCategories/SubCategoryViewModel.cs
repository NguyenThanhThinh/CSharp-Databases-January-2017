namespace HardwareShop.Models.ViewModels.SubCategories
{
    public class SubCategoryViewModel : ViewModelBase
    {
        public int SubCategoryId { get; set; }
        
        public string Name { get; set; }

        public bool IsDeleted { get; set; }
    }
}
