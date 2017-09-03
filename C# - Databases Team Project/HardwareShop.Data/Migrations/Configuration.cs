namespace HardwareShop.Data.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Models.EntityModels;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.IO;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<HardwareShopContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(HardwareShopContext context)
        {
            if (!context.Roles.Any())
            {
                this.CreateRole(context, "User");
                this.CreateRole(context, "StoreManager");
                this.CreateRole(context, "Admin");
            }

            context.SaveChanges();

            if (!context.Users.Any())
            {
                this.CreateUser(context, "admin", "123");
                this.CreateUser(context, "ninja", "123");
                this.CreateUser(context, "manager", "123");
                this.CreateUser(context, "pesho", "123");
                this.CreateUser(context, "gosho", "123");
                this.CreateUser(context, "tosho", "123");
                this.CreateUser(context, "mosho", "123");
                this.CreateUser(context, "iulian", "123");

                this.SetRoleToUser(context, "admin", "User");
                this.SetRoleToUser(context, "admin", "StoreManager");
                this.SetRoleToUser(context, "admin", "Admin");
                this.SetRoleToUser(context, "ninja", "User");
                this.SetRoleToUser(context, "ninja", "StoreManager");
                this.SetRoleToUser(context, "ninja", "Admin");
                this.SetRoleToUser(context, "manager", "User");
                this.SetRoleToUser(context, "manager", "StoreManager");
                this.SetRoleToUser(context, "pesho", "User");
                this.SetRoleToUser(context, "gosho", "User");
                this.SetRoleToUser(context, "iulian", "User");
                this.SetRoleToUser(context, "tosho", "User");
                this.SetRoleToUser(context, "mosho", "User");
            }

            context.SaveChanges();

            var categories = new List<Category>();
            var subCategories = new List<SubCategory>();
            var items = new List<Item>();
            var reviews = new List<Review>();
            var comments = new List<Comment>();

            if (!context.Categories.Any())
            {
                categories = new List<Category>
                {
                    new Category { Name = "Peripherals" },
                    new Category { Name = "Parts" },
                    new Category { Name = "Consumables" }
                };

                context.Categories.AddRange(categories);
                context.SaveChanges();
            }

            if (!context.SubCategories.Any())
            {
                subCategories = new List<SubCategory>
                {
                    new SubCategory { Name = "Keyboards", Category = categories[0] },
                    new SubCategory { Name = "Monitors", Category = categories[0] },
                    new SubCategory { Name = "Printers", Category = categories[0] },
                    new SubCategory { Name = "CPUs", Category = categories[1] },
                    new SubCategory { Name = "GPUs", Category = categories[1] },
                    new SubCategory { Name = "Motherboards", Category = categories[1] },
                    new SubCategory { Name = "Ram memories", Category = categories[1] },
                    new SubCategory { Name = "Hard drives", Category = categories[1] },
                    new SubCategory { Name = "Paper", Category = categories[2] },
                    new SubCategory { Name = "Paperclips", Category = categories[2] },
                    new SubCategory { Name = "Stapler", Category = categories[2] },
                };

                context.SubCategories.AddRange(subCategories);
                context.SaveChanges();
            }

            string path = @"C:\Users\valko\Documents\Visual Studio 2015\Projects\3. Web Projects\Project From C# Databases\Team Project Hardware Store\HardwareShop.Web\Content\Img\";

            if (!context.Items.Any())
            {
                items = new List<Item>
                {
                    new Item
                    {
                        Picture = File.ReadAllBytes(path + "CPUPINTCELG1840_350x350.jpg"),
                        ManufacturerName = "Intel",
                        Model = "Celeron G1840, BOX",
                        WarrantyLengthMonths = 36,
                        ItemStatus = ItemStatus.Available,
                        Price = 71.51M,
                        Quantity = 1,
                        SubCategory = subCategories[3],
                        Description = @"# of Cores - 2
 # of Threads - 2
 Clock Speed - 2.8 GHz
 Cache - 2 MB
 DMI2 - 5 GT/s
 Instruction Set - 64-bit
 Instruction Set Extensions - SSE4.1/4.2
 Embedded Options Available - No
 Lithography - 22 nm
 Scalability - 1S Only
 Max TDP - 53 W
 Thermal Solution Specification - PCG 2013C"
                    },
                    new Item
                    {
                        Picture = File.ReadAllBytes(path + "CPUPINTELBX80677I57400_350x350.jpg"),
                        ManufacturerName = "Intel",
                        Model = "i5-7400 3.0/3.5GHz 6MB LGA1151 BOX",
                        WarrantyLengthMonths = 36,
                        ItemStatus = ItemStatus.Available,
                        Price = 403.92M,
                        Quantity = 5,
                        SubCategory = subCategories[3],
                        Description = @"# of Cores: 4
# of Threads: 4
Processor Base Frequency:	3.00 GHz
Max Turbo Frequency: 3.50 GHz
Cache: 6 MB
Bus Speed: 8 GT/s DMI3
TDP: 65 W"
                    },
                    new Item
                    {
                        Picture = File.ReadAllBytes(path + "CPUPZINTELBX80677I77700K_350x350.jpg"),
                        ManufacturerName = "Intel",
                        Model = "Core i7-7700K 4.2/4.5GHz 8MB LGA1151 BOX",
                        WarrantyLengthMonths = 36,
                        ItemStatus = ItemStatus.Promotion,
                        Price = 805.20M,
                        NewPrice = 750.30M,
                        Quantity = 7,
                        SubCategory = subCategories[3],
                        Description = @"# of Cores: 4
# of Threads: 8
Processor Base Frequency: 4.20 GHz
Max Turbo Frequency: 4.50 GHz
Cache: 8 MB
Bus Speed: 8 GT/s DMI3
TDP: 91 W"
                    },
                    new Item
                    {
                        Picture = File.ReadAllBytes(path + "CPUPAMDRYZEN71700_350x350.jpg"),
                        ManufacturerName = "AMD",
                        Model = "Ryzen 7 1700",
                        WarrantyLengthMonths = 36,
                        ItemStatus = ItemStatus.Available,
                        Price = 731.07M,
                        Quantity = 7,
                        SubCategory = subCategories[3],
                        Description = @"3.0 GHz Base- & 3,7 GHz Turbo Clock with Precision Boost
Unlocked multiplier for manual overclocking (unlocked CPU)
Large memory cache: 4 MB L2 & 16 MB L3"
                    },
                    new Item
                    {
                        Picture = File.ReadAllBytes(path + "MBIASROCH81PROBTCR20_350x350.jpg"),
                        ManufacturerName = "ASRock",
                        Model = "H81 Pro BTC R2.0",
                        WarrantyLengthMonths = 36,
                        ItemStatus = ItemStatus.New,
                        Price = 167.04M,
                        Quantity = 17,
                        SubCategory = subCategories[5],
                        Description = @"- ASRock Super Alloy
- Supports New 4th and 4th Generation Intel® Core™ i7/i5/i3/Xeon®/Pentium®/Celeron® Processors (Socket 1150)
- All Solid Capacitor design
- Digi Power, 4 Power Phase design
- 1 x PCIe 2.0 x16, 5 x PCIe 2.0 x1
- Graphics output options: D-Sub, HDMI
- 5.1 CH HD Audio (Realtek ALC662 Audio Codec)
- 2 x SATA3, 2 x SATA2
- 2 x USB 3.0, 6 x USB 2.0 (4 Front, 2 Rear)
- Realtek Gigabit LAN
- Supports Full Spike Protection, ASRock Live Update & APP Shop"
                    },
                    new Item
                    {
                        Picture = File.ReadAllBytes(path + "VCREVGA06GP46161KR_350x350.jpg"),
                        ManufacturerName = "EVGA",
                        Model = "GF GTX 1060 GAMING 6GB",
                        WarrantyLengthMonths = 36,
                        ItemStatus = ItemStatus.OutOfStock,
                        Price = 594.00M,
                        Quantity = 0,
                        SubCategory = subCategories[4],
                        Description = @"NVIDIA GTX 1060
1280 Pixel Pipelines
1506 MHz Base Clock
1708 MHz Boost Clock
120.4GT/s Texture Fill Rate"
                    },
                    new Item
                    {
                        Picture = File.ReadAllBytes(path + "KBREDRAGONVERAK551B_350x350.jpg"),
                        ManufacturerName = "Redragon",
                        Model = "Vara K551B",
                        WarrantyLengthMonths = 24,
                        ItemStatus = ItemStatus.OutOfStock,
                        Price = 99.57M,
                        Quantity = 0,
                        SubCategory = subCategories[0],
                        Description = @"Custom mechanical switches (Cherry Green equivalent) for ultimate gaming performance
Red LED adjustable lighting and double-shot injection molded keycaps for crystal clear backlighting
Aluminum and ABS construction, plate-mounted mechanical keys, and gold plated USB connector stand up to hardcore gaming
Custom mechanical switches designed for longevity, responsiveness, and durability. Mechanical keys with medium resistance, audible click sound, and tactile feedback"
                    },
                    new Item
                    {
                        Picture = File.ReadAllBytes(path + "MRAMCORSAIRCMK16GX4M2B3200C16_350x350.jpg"),
                        ManufacturerName = "Corsair",
                        Model = "Vengeance LPX",
                        WarrantyLengthMonths = 60,
                        ItemStatus = ItemStatus.Available,
                        Price = 293.96M,
                        Quantity = 3,
                        SubCategory = subCategories[6],
                        Description = @"Memory Configuration
Dual / Quad Channel

Memory Series
Vengeance

Memory Type
DDR4

Package Memory Format
DIMM"
                    },
                    new Item
                    {
                        Picture = File.ReadAllBytes(path + "SSDSAMSUNG250GBMZ75E250850EVO_350x350.jpg"),
                        ManufacturerName = "Samsung",
                        Model = "MZ-75E250, 850 EVO",
                        WarrantyLengthMonths = 60,
                        ItemStatus = ItemStatus.Available,
                        Price = 216.03M,
                        Quantity = 13,
                        SubCategory = subCategories[7],
                        Description = @"Product Type
Solid State Drive

Interface
SATA 6Gb/s Interface, compatible with SATA 3Gb/s & SATA 1.5Gb/s interface

Capacity
250 GB (1GB=1 Billionbyte by IDEMA)

Sequential Read Speed
Up to 540 MB/sec Sequential Read

Sequential Write Speed
Up to 520 MB/sec Sequential Write

Memory Speed
Samsung 32 layer 3D V-NAND
Samsung 512 MB Low Power DDR3 SDRAM"
                    }
                };

                context.Items.AddRange(items);
                context.SaveChanges();
            }

            var admin = context.Users.FirstOrDefault(u => u.UserName == "admin");
            var pesho = context.Users.FirstOrDefault(u => u.UserName == "pesho");

            if (!context.Reviews.Any())
            {
                reviews = new List<Review>
                {
                    new Review
                    {
                        Item = items[0],
                        Author = admin,
                        Content = @"Pros: decent speed, stays between 35 and 47 c depending on the workload. 43c average. excellent for htpc, internet.

Cons: for me none

Other Thoughts: I have bought cpu after cpu, looking for one that doesn't make my htpc look like a mini oven. because htpcs are small, they tend to get hot quicker. so I started looking for one that is pretty fast in response, able to work well with a video card. and most important of all that doesn't get hot. so after getting to this cpu and installing and testing my htpc, I said to my self, I have finally found what I was looking for. motherboard GA-H81M-HD3, memory g skill 1333 MHz , ARCTIC Freezer 7 Pro Rev. 2, CPU Cooler , case: nMedia HTPC 6000B Black , LG blue ray WH16NS40 , 500 w power supply DT, Video card H6850. todays Celerons are better than the older ones.",
                        Score = 4
                    },
                    new Review
                    {
                        Item = items[0],
                        Author = pesho,
                        Content = @"Pros: -It's great for crypto mining
-cheap
-low power consumption

Cons: It's not an AMD, but I can overlook that.

Other Thoughts: I will purchase more of these",
                        Score = 4
                    },
                    new Review
                    {
                        Item = items[0],
                        Author = admin,
                        Content = @"Pros: good delivery

Cons: no cons",
                        Score = 2
                    },
                    new Review
                    {
                        Item = items[0],
                        Author = pesho,
                        Content = @"Pros: - Price: I selected Intel CPUs, hit sort by price, and picked the top of the list. :)
- Graphics: I needed a CPU that had integrated graphics, and this one works like a champ.
- Embeddable: Throw this in a micro-ATX board and tuck it away for whatever project you may have that needs a bit more juice than a Raspberry Pie.

Cons: -None!",
                        Score = 3
                    }
                };

                context.Reviews.AddRange(reviews);
                context.SaveChanges();
            }

            if (!context.Comments.Any())
            {
                comments = new List<Comment>
                {
                    new Comment
                    {
                        Author = pesho,
                        Content = @"I bought a broken down dance arcade machine for super cheap and needed to find out what was wrong with it. Long story short, the motherboard was toast. Instead of replacing the 2005 era hardware, I threw it all out and built a modern, cheapo system. I replaced the Pentium 4 with this CPU, upgraded from 256MB RAM to 2GB, and replaced the 5400 RPM PATA drive with a Kingston SSD. Brought this sucker right back to life! The integrated graphics absolutely KILL the GeForce FX 5200 that was originally in the cab, haha.",
                        Review = reviews[0]
                    },
                    new Comment
                    {
                        Author = pesho,
                        Content = @" I set this up with 8 GB RAM and a SATAIII SSD, and it's a surprisingly-fast little low-power Mini-ITX system running Linux. The onboard GPU is acceptable for light gaming, I get 60 fps in Minecraft and 45 fps in StarMade at low/medium detail settings at 1280x1024. I measured the system with a Kill-A-Watt and at full load while gaming the entire PC only draws about 35 Watts, idling at about 10.",
                        Review =
                        reviews[0]
                    },
                    new Comment
                    {
                        Author = pesho,
                        Content = @"I bought this Celeron to replace the G3258 I'd had in my HTPC when I realized how silly it was to use a G3258 in an HTPC. I was looking for something cool, quiet and energy efficient. This little fella did the trick. Running in an older Antec Aria case which does NOT ventilate well, it still maintains about a 28-30 degree temp running Kodibuntu. Plays HD video with just the onboard graphics and never stutters. The heatsink that comes with it is the non-copper-core variety, but no worries. I'm very pleased with this Celeron, especially since I have traditionally disliked them. Haswell is really a solid line of CPU's and this one is no exception.",
                        Review = reviews[0]
                    },
                    new Comment
                    {
                        Author = pesho,
                        Content = @"I would buy this again in a heartbeat. This is a perfect HTPC CPU. I chose this over a newer Skylake CPU (which has better integrated graphics) because I already had some DDR3 RAM from my old computer and so I saved $20.00 on RAM, and spent the money on a dedicated graphics card instead (to save CPU resources). If you are building an HTPC completely from scratch, just build on a skylake (socket 1151) and skip the dedicated video card. Trust me on this one, it will be fine.",
                        Review = reviews[0]
                    },
                    new Comment
                    {
                        Author = pesho,
                        Content = @"cheap cpu for any casual/light loads. at this day and age haswells even if a celeron has enough muscle to get the job done. pair it with an ssd and you have a very strong office machine, the integrated graphics meet the minimum spec for some games too.",
                        Review = reviews[1]
                    },
                    new Comment
                    {
                        Author = pesho,
                        Content = @"I have build a budget office/gaming PC to replace my 7 year old Acer Aspire Desktop computer. I was very surprised that the PC performed very well on games like Race Driver Grid and Grid Autosport. I was under a tight budget at the time and I planned on re-using the old MicroATX case I found two years ago, the sata DVD Drive, and so forth.",
                        Review = reviews[1]
                    }
                };

                context.Comments.AddRange(comments);
                context.SaveChanges();
            }
        }

        private void SetRoleToUser(HardwareShopContext context, string username, string role)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            var user = context.Users.First(u => u.UserName == username);

            var result = userManager.AddToRole(user.Id, role);

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(";", result.Errors));
            }
        }

        private void CreateUser(HardwareShopContext context, string username, string password)
        {
            //create user manager
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            //set user manager password validator
            userManager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 1,
                RequireDigit = false,
                RequireLowercase = false,
                RequireNonLetterOrDigit = false,
                RequireUppercase = false,
            };

            //create user object
            var user = new ApplicationUser(username);
            user.Carts.Add(new Cart());

            //create user
            var result = userManager.Create(user, password);

            //validate result
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(";", result.Errors));
            }
        }

        private void CreateRole(HardwareShopContext context, string roleName)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var result = roleManager.Create(new IdentityRole(roleName));

            if (!result.Succeeded)
            {
                throw new Exception(string.Join(";", result.Errors));
            }
        }
    }
}