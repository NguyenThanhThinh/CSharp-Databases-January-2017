namespace HardwareShop.Web.App_Start
{
    using AutoMapper;
    using Models.EntityModels;
    using Models.ViewModels.Categories;
    using Models.ViewModels.Comments;
    using Models.ViewModels.Items;
    using Models.ViewModels.Manage;
    using Models.ViewModels.Reviews;
    using Models.ViewModels.Sale;
    using Models.ViewModels.SubCategories;
    using Models.ViewModels.Users;
    using System;
    using System.IO;
    using System.Linq;
    using System.Web;

    public static class AutoMappingWebConfig
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Category, CategoryViewModel>();

                cfg.CreateMap<CategoryViewModel, Category>();

                cfg.CreateMap<Category, CategoriesSubCategoriesViewModel>()
                    .ForMember(vm => vm.SubCategories, opt => opt.MapFrom(src => src.SubCategories));

                cfg.CreateMap<Category, NavbarCategoriesViewModel>()
                    .ForMember(vm => vm.SubCategories, opt => opt.MapFrom(src => src.SubCategories.Where(c => c.IsDeleted == false)));

                cfg.CreateMap<SubCategory, SubCategoryViewModel>();

                cfg.CreateMap<SubCategory, EditSubCategoryViewModel>();

                cfg.CreateMap<EditSubCategoryViewModel, SubCategory>();

                cfg.CreateMap<SubCategory, DeleteSubCategoryViewModel>();

                cfg.CreateMap<SubCategory, NavbarSubCategoriesViewModel>();

                cfg.CreateMap<ApplicationUser, UserViewModel>()
                    .ForMember(vm => vm.IsDeleted, opt => opt.MapFrom(src => src.LockoutEnabled));

                cfg.CreateMap<ApplicationUser, EditUserViewModel>();

                cfg.CreateMap<ApplicationUser, DeleteUserViewModel>();

                cfg.CreateMap<ApplicationUser, ManageUserViewModel>();

                cfg.CreateMap<Item, HomeItemsViewModel>()
                    .ForMember(vm => vm.Picture, opt => opt.MapFrom(src => "data:image/jpeg;base64," + Convert.ToBase64String(src.Picture)));

                cfg.CreateMap<CreateItemViewModel, Item>();

                cfg.CreateMap<Item, ItemViewModel>()
                    .ForMember(vm => vm.CategoryName, opt => opt.MapFrom(src => src.SubCategory.Category.Name))
                    .ForMember(vm => vm.SubCategoryName, opt => opt.MapFrom(src => src.SubCategory.Name))
                    .ForMember(vm => vm.IsSubCategoryDeleted, opt => opt.MapFrom(src => src.SubCategory.IsDeleted));

                cfg.CreateMap<Item, DetailsItemViewModel>()
                    .ForMember(vm => vm.Picture, opt => opt.MapFrom(src => "data:image/jpeg;base64," + Convert.ToBase64String(src.Picture)))
                    .ForMember(vm => vm.Rating, opt => opt.MapFrom(src => src.Reviews.Any() ? Math.Round(src.Reviews.Average(r => r.Score), 1) : 0))
                    .ForMember(vm => vm.CategoryName, opt => opt.MapFrom(src => src.SubCategory.Category.Name))
                    .ForMember(vm => vm.SubCategoryName, opt => opt.MapFrom(src => src.SubCategory.Name))
                    .ForMember(vm => vm.IsSubCategoryDeleted, opt => opt.MapFrom(src => src.SubCategory.IsDeleted));

                cfg.CreateMap<CreateItemViewModel, Item>()
                    .ForMember(vm => vm.Picture, opt => opt.MapFrom(src => GetBytesFromFile(src.Picture)));

                cfg.CreateMap<Item, EditItemViewModel>()
                    .ForMember(vm => vm.Picture, opt => opt.MapFrom(src => "data:image/jpeg;base64," + Convert.ToBase64String(src.Picture)));

                cfg.CreateMap<Item, DeleteItemViewModel>()
                    .ForMember(vm => vm.Rateing, opt => opt.MapFrom(src => src.Reviews.Any() ? Math.Round(src.Reviews.Average(r => r.Score), 1) : 0))
                    .ForMember(vm => vm.SubCategoryName, opt => opt.MapFrom(src => src.SubCategory.Name))
                    .ForMember(vm => vm.CategoryName, opt => opt.MapFrom(src => src.SubCategory.Category.Name));

                cfg.CreateMap<Review, ListReviewsViewModel>()
                    .ForMember(vm => vm.Comments, opt => opt.MapFrom(src => src.Comments.Where(c => c.IsDeleted == false)))
                    .ForMember(vm => vm.AuthorName, opt => opt.MapFrom(src => src.Author.UserName));

                cfg.CreateMap<Review, CreateReviewViewModel>();

                cfg.CreateMap<CreateReviewViewModel, Review>();

                cfg.CreateMap<Review, EditReviewViewModel>();

                cfg.CreateMap<Review, DeleteReviewViewModel>()
                    .ForMember(vm => vm.AuthorUsername, opt => opt.MapFrom(src => src.Author.UserName));

                cfg.CreateMap<Comment, ListCommentsViewModel>()
                    .ForMember(vm => vm.AuthorName, opt => opt.MapFrom(src => src.Author.UserName));

                cfg.CreateMap<CommentViewModel, Comment>();

                cfg.CreateMap<Comment, CommentViewModel>()
                    .ForMember(vm => vm.ItemId, opt => opt.MapFrom(src => src.Review.ItemId));

                cfg.CreateMap<Comment, DeleteCommentViewModel>()
                    .ForMember(vm => vm.ItemId, opt => opt.MapFrom(src => src.Review.ItemId))
                    .ForMember(vm => vm.AuthorUsername, opt => opt.MapFrom(src => src.Author.UserName));

                cfg.CreateMap<Sale, SaleDetailsViewModel>()
                    .ForMember(vm => vm.Username, opt => opt.MapFrom(src => src.Cart.User.UserName));

                cfg.CreateMap<CreateSaleViewModel, Sale>();

                cfg.CreateMap<Sale, SalesViewModel>()
                    .ForMember(vm => vm.Username, opt => opt.MapFrom(src => src.Cart.User.UserName));
            });
        }

        private static byte[] GetBytesFromFile(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return new byte[0];
            }

            MemoryStream stream = new MemoryStream();
            file.InputStream.CopyTo(stream);
            byte[] pictureInData = stream.ToArray();

            return pictureInData;
        }
    }
}