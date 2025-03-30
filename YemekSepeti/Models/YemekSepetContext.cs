using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using YemekSepeti.DTO;


namespace YemekSepeti.Models;

public partial class YemekSepetContext : IdentityDbContext<IdentityUser<int>,IdentityRole<int>,int>
{
 




    public YemekSepetContext(DbContextOptions<YemekSepetContext> options)
        : base(options)
    {
       
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<DeliveryPersonnel> DeliveryPersonnel { get; set; }

    public virtual DbSet<Meal> Meals { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Restaurant> Restaurants { get; set; }

    public virtual DbSet<RestaurantReview> RestaurantReviews { get; set; }
    public virtual DbSet<Notification> Notifications { get; set; }





    protected override async void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Notification>()
            .HasOne(n => n.User)
            .WithMany()
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(e => e.Id).UseIdentityColumn();
        });

        modelBuilder.Entity<Order>()
        .ToTable(tb => tb.HasTrigger("trg_SiparisDurumunuSetEt"));
        modelBuilder.Entity<RestaurantReview>()
        .ToTable(tb => tb.HasTrigger("trg_RestoranRatingiGuncelle"));
        modelBuilder.Entity<RestaurantReview>()
        .ToTable(tb => tb.HasTrigger("trg_YorumSilindigindeRestoranRatingiGuncelle"));

        modelBuilder
        .Entity<DeliveryPersonnelPerformance>(
            eb =>
            {
                eb.HasNoKey();
                eb.ToView("vw_KuryePerformansi");
                eb.Property(v => v.DeliveryPersonnelName).HasColumnName("DeliveryPersonnelName");
                eb.Property(v => v.DeliveryPersonnelId).HasColumnName("DeliveryPersonnelId");
                eb.Property(v => v.TotalDeliveredOrders).HasColumnName("TotalDeliveredOrders");
            });


        base.OnModelCreating(modelBuilder);

        //modelBuilder.Entity<CustomerEntity>().ToTable("AspNetUsers").HasBaseType<IdentityUser<int>>();
        //modelBuilder.Entity<RestaurantEntity>().ToTable("AspNetUsers").HasBaseType<IdentityUser<int>>();
        //modelBuilder.Entity<DeliveryPersonnelEntity>().ToTable("AspNetUsers").HasBaseType<IdentityUser<int>>();
        //modelBuilder.Entity<Restaurant>().HasData(
        //new Restaurant()
        //{
        //    Id = -1,
        //    Name = "Maydonoz Döner",
        //    Address = "MERSIS No: 0773072861500001",
        //    PhoneNumber = "(0442) 238 23 22",
        //    WorkingHours = "11:00 - 23:59",
        //    Rating = 3.3,
        //    Image = "https://images.deliveryhero.io/image/fd-tr/tr-logos/cp9bu-logo.jpg",

        //    Email = "maydanozdoner@gmail.com",
        //    Password = "Maydanozdoner1234!",

        //},
        //new Restaurant()
        //{
        //    Id = -2,
        //    Name = "Musqa Burger",
        //    Address = "Ömer Nasuhi Bilmen Mahallesi Kombina Caddesi No:9 Yakutiye Erzurum",
        //    PhoneNumber = "0545 442 85 25",
        //    WorkingHours = "11:00 - 01:00",
        //    Rating = 3.9,
        //    Image = "https://images.deliveryhero.io/image/fd-tr/tr-logos/ch8or-logo.jpg",
        //    Email = "musqaburger@gmail.com",
        //    Password = "Musqaburger1234!",

        //},
        //new Restaurant()
        //{
        //    Id = -3,
        //    Name = "Burger King",
        //    Address = "Palerium AVM,No:22/3 Mağaza:100",
        //    PhoneNumber = "444 5 464",
        //    WorkingHours = "11:00 - 22:00",
        //    Rating = 3.8,
        //    Image = "https://images.deliveryhero.io/image/fd-tr/tr-logos/cl3by-logo.jpg",

        //    Email = "burgerking@gmail.com",
        //    Password ="Burgerking1234!",


        //},
        //new Restaurant()
        //{
        //    Id = -4,
        //    Name = "Cheff Pizza",
        //    Address = "GEZ Mah. Spor Yolu Sokak Alin Sitesi NO:52 Düzgün Market ilerisi YAKUTİYE ERZURUM",
        //    PhoneNumber = "0553 697 79 42",
        //    WorkingHours = "11:00 - 02:00",
        //    Rating = 4.4,
        //    Image = "https://images.deliveryhero.io/image/fd-tr/LH/jbue-listing.jpg",
        //    Meals = new List<Meal>()
        //    {


        //    },
        //    Email = "cheffpizza@gmail.com",
        //    Password = "Chefpizza1234!",


        //},
        //new Restaurant()
        //{
        //    Id = -5,
        //    Name = "Popeyes",
        //    Address = "Yukarı Mumcu Mah. Cumhuriyet Caddesi No: 22/3 MGZ No: 100 Palerium AVM Yakutiye",
        //    PhoneNumber = "444 7 677",
        //    WorkingHours = "11:00 - 22:00",
        //    Rating = 3.3,
        //    Image = "https://images.deliveryhero.io/image/fd-tr/tr-logos/cr5ff-logo.jpg",
        //    Meals = new List<Meal>()
        //    {


        //    },
        //    Email = "popeyes@gmail.com",
        //    Password ="Poeyes1234!",

        //}
        //);
        //modelBuilder.Entity<DeliveryPersonnel>().HasData(

        //    new DeliveryPersonnel() { Id = -10, Name = "Hakan Yıldız", PhoneNumber = "0538 613 36 08", VeichleType = "Araba", Email = "hakanyildiz@gmail.com", Password =  "Hakan1234!", }
        //    );
        //modelBuilder.Entity<Customer>().HasData(
        //    new Customer()
        //    {

        //        Id = -9,
        //        Name = "Fatime Murtuzova",
        //        Email = "fatimemurtuzova@gmail.com",
        //        PhoneNumber = "05053661826",
        //        Address = "Yakutiye,Erzurum",
        //        Password = "Fatime1234!",

        //    }
        //    );
        //modelBuilder.Entity<Meal>().HasData(
        //    new Meal() { Id = -1, Name = "Tavuk Döner Medium Dürüm", Price = 150, Description = "Özel fırın lavaşa; 75 gr. tavuk döner, salatalık turşusu, patates kızartması, özel sos, isteğe göre soğan, sarımsaklı mayonez sos. Cin biberi ile", IsAvailable = true, RestaurantId = -1, Image = "https://images.deliveryhero.io/image/fd-tr/tr-logos/cp9bu-logo.jpg", Quantity = 1 },
        //    new Meal() { Id = -2, Name = "Tavuk Döner XLarge Dürüm (45 cm.)", Price = 205, Description = "2 adet özel fırın lavaşa; 125 gr. tavuk döner, patates kızartması, salatalık turşusu, özel sos, isteğe göre soğan, sarımsaklı mayonez sos. Cin biberi ile", IsAvailable = true, RestaurantId = -1, Image = "https://images.deliveryhero.io/image/fd-tr/Products/10775609.jpg??width=500", Quantity = 1 },
        //    new Meal() { Id = -3, Name = "Külah Patates Kızartması", Price = 50, Description = "Dilimlenmiş patateslerin kızartılarak koni şeklinde kartonda servis edildiği bir atıştırmalıktır.", IsAvailable = true, RestaurantId = -1, Image = "https://images.deliveryhero.io/image/fd-tr/Products/10775630.jpg??width=500", Quantity = 1 },
        //    new Meal() { Id = -4, Name = "Tavuk Döner Large Dürüm Menü", Price = 210, Description = "Large Tavuk Döner Dürüm (45 cm.) + Ayran (27 cl.)", IsAvailable = true, RestaurantId = -1, Image = "https://images.deliveryhero.io/image/fd-tr/Products/10775572.jpg??width=500", Quantity = 1 },
        //    new Meal()
        //    {
        //        Id = -5,
        //        Name = "Tavuk Döner Small Dürüm",
        //        Price = 125,
        //        Description = "Özel fırın lavaşa; 50 gr. tavuk döner, patates kızartması, salatalık turşusu, özel sos, isteğe göre soğan, sarımsaklı mayonez sos. Cin biberi ile",
        //        IsAvailable = true,
        //        RestaurantId = -1,
        //        Image = "https://images.deliveryhero.io/image/fd-tr/Products/10775606.jpg??width=500",
        //        Quantity = 1
        //    },
        //    new Meal() { Id = -6, Name = "Musqa Burger", Price = 249.90M, Description = "Ev yapımı hamburger ekmeği, ev yapımı hamburger köftesi, Musqa sos, domates, yeşillik, turşu. Patates kızartması ile", IsAvailable = true, RestaurantId = -2, Image = "https://images.deliveryhero.io/image/fd-tr/Products/26725904.jpg??width=500", Quantity = 1 },
        //    new Meal() { Id = -7, Name = "Musqa Chicken Burger", Price = 200.90M, Description = "Ev yapımı hamburger ekmeği, ev yapımı tavuk burger köftesi, özel sos, mayonez, domates, yeşillik. Patates kızartması ile", IsAvailable = true, RestaurantId = -2, Image = "https://images.deliveryhero.io/image/fd-tr/Products/26727171.jpg??width=500", Quantity = 1 },
        //    new Meal() { Id = -8, Name = "Musqa Rich Burger", Price = 263.90M, Description = "Ev yapımı hamburger ekmeği, ev yapımı hamburger köftesi, Musqa sos, cheddar peyniri, soğan, turşu, domates, yeşillik. Patates kızartması ile", IsAvailable = true, RestaurantId = -2, Image = "https://images.deliveryhero.io/image/fd-tr/Products/26725995.jpg??width=500", Quantity = 1 },
        //    new Meal() { Id = -9, Name = "Big Boss Burger", Price = 299.90M, Description = "Ev yapımı hamburger ekmeği, ev yapımı 2 adet 70 gr. hamburger köftesi, özel sos, double cheddar peyniri, dana jambon, eritilmiş kaşar peyniri. Patates kızartması ile", IsAvailable = true, RestaurantId = -2, Image = "https://images.deliveryhero.io/image/fd-tr/Products/29087186.jpg??width=500", Quantity = 1 },
        //    new Meal() { Id = -10, Name = "Soğan Halkası (7 Adet)", Price = 45, Description = "7 adet soğan halkası", IsAvailable = true, RestaurantId = -2, Image = "https://images.deliveryhero.io/image/fd-tr/Products/7506896.jpg??width=500", Quantity = 1 },
        //    new Meal() { Id = -11, Name = "Yemeksepeti Özel 2’li Whopper Jr.® Kampanyası", Price = 220, Description = "2 Adet Whopper Jr.® + Patates Kızartması (Orta) + Soğan Halkası (4'lü) + 1 L. İçecek", IsAvailable = true, RestaurantId = -3, Image = "https://images.deliveryhero.io/image/fd-tr/Products/44479596.jpg??width=500", Quantity = 1 },
        //    new Meal() { Id = -12, Name = "Kral İkili", Price = 280, Description = "Big King® + King Chicken® + Patates Kızartması (Orta) + 1 L. İçecek", IsAvailable = true, RestaurantId = -3, Image = "https://images.deliveryhero.io/image/fd-tr/Products/27388696.jpg??width=500", Quantity = 1 },
        //    new Meal() { Id = -13, Name = "Whopper® Menü", Price = 235, Description = "Whopper® + Patates Kızartması (Orta) + Kutu İçecek", IsAvailable = true, RestaurantId = -3, Image = "https://images.deliveryhero.io/image/fd-tr/Products/27388717.jpg??width=500", Quantity = 1 },
        //    new Meal() { Id = -14, Name = "Big King® Menü", Price = 230, Description = "Big King® + Patates Kızartması (Orta) + Kutu İçecek", IsAvailable = true, RestaurantId = -3, Image = "https://images.deliveryhero.io/image/fd-tr/Products/27388722.jpg??width=500", Quantity = 1 },
        //    new Meal() { Id = -15, Name = "Coca-Cola Fırsatı (2'li Big King® Fırsatı)", Price = 380, Description = "Big King® + Big King® + Patates Kızartması (Orta) + Coca-Cola (1 L.)", IsAvailable = true, RestaurantId = -3, Image = "https://images.deliveryhero.io/image/fd-tr/Products/27388683.jpg??width=500", Quantity = 1 },
        //    new Meal() { Id = -16, Name = "Karışık Pizza (Orta)", Price = 190, Description = "Pizza sosu, mozzarella peyniri, sucuk, salam, mantar, mısır, zeytin, kapya biberi, jalapeno biberi, çarliston biber", IsAvailable = true, RestaurantId = -4, Image = "https://images.deliveryhero.io/image/fd-tr/Products/37824103.jpg??width=500", Quantity = 1 },
        //    new Meal() { Id = -17, Name = "Karışık Pizza (Büyük)", Price = 275, Description = "Pizza sosu, mozzarella peyniri, sucuk, salam, mantar, mısır, zeytin, kapya biberi, jalapeno biberi, çarliston biber", IsAvailable = true, RestaurantId = -4, Image = "https://images.deliveryhero.io/image/fd-tr/Products/37824108.jpg??width=500", Quantity = 1 },
        //    new Meal() { Id = -18, Name = "Patates Kızartması", Price = 65, Description = "Parmak dilim patatas kızartması", IsAvailable = true, RestaurantId = -4, Image = "https://images.deliveryhero.io/image/fd-tr/Products/37824124.jpg??width=500", Quantity = 1 },
        //    new Meal() { Id = -19, Name = "Orta Boy Menü", Price = 275, Description = "Karışık Pizza (Orta) + Patates Kızartması + Kutu İçecek", IsAvailable = true, RestaurantId = -4, Image = "https://images.deliveryhero.io/image/fd-tr/Products/37824095.jpg??width=500", Quantity = 1 },
        //    new Meal() { Id = -20, Name = "Karışık Pizza (Küçük)", Price = 149, Description = "Pizza sosu, mozzarella peyniri, sucuk, salam, mantar, mısır, zeytin, kapya biberi, jalapeno biberi, çarliston biber\r\n\r\n", IsAvailable = true, RestaurantId = -4, Image = "https://images.deliveryhero.io/image/fd-tr/Products/37824098.jpg??width=500", Quantity = 1 },
        //    new Meal() { Id = -21, Name = "Yemeksepeti Özel 2 Kişilik Ekonomik Menü", Price = 280, Description = "2 Adet Popchicken Sandviç + Patates Kızartması (Orta) + Nuggets (4'lü) + Soğan Halkası (4'lü) + 1L. İçecek\r\n\r\n", IsAvailable = true, RestaurantId = -5, Image = "https://images.deliveryhero.io/image/fd-tr/Products/45539560.jpg??width=500", Quantity = 1 },
        //    new Meal() { Id = -22, Name = "Yemeksepeti Özel Ekonomix Menü", Price = 210, Description = "2 Adet Tavukburger® + Patates Kızartması (Orta) + Seçeceğiniz 2 Adet Sos + İçecek (1 L.)", IsAvailable = true, RestaurantId = -5, Image = "https://images.deliveryhero.io/image/fd-tr/Products/62936401.jpg??width=500", Quantity = 1 },
        //    new Meal() { Id = -23, Name = "Maxi XL Menü", Price = 290, Description = "Popeyes XL Sandviç + Acılı Kanat (2'li) + Nuggets (4'lü) + Patates Kızartması (Orta) + Kutu İçecek", IsAvailable = true, RestaurantId = -5, Image = "https://images.deliveryhero.io/image/fd-tr/Products/28499489.jpg??width=500", Quantity = 1 },
        //    new Meal() { Id = -24, Name = "2 Kişilik Fırsat Menüsü", Price = 310, Description = "2 Adet Poppy Sandviç + Nuggets (4'lü) + Soğan Halkası (4'lü) + Tenders (2'li) + Patates Kızartması (Orta) +1L. İçecek", IsAvailable = true, RestaurantId = -5, Image = "https://images.deliveryhero.io/image/fd-tr/Products/28499446.jpg??width=500", Quantity = 1 },
        //    new Meal() { Id = -25, Name = "Coca-Cola Fırsatı (3 Kişilik Doyuran Kova)", Price = 390, Description = "3 Adet Tavukburger® + Nuggets (12'li) + Patates Kızartması (Büyük) + Seçeceğiniz 2 Adet Sos + 1L. İçecek", IsAvailable = true, RestaurantId = -5, Image = "https://images.deliveryhero.io/image/fd-tr/Products/28499435.jpg??width=500", Quantity = 1 }
        //    );
        //modelBuilder.Entity<Category>().HasData(
        //     new Category()
        //     {
        //         Id = -1,
        //         Name = "Fast Food"
        //     }
        //    );
        //modelBuilder.Entity<Restaurant>().HasData(
        //    new Restaurant()
        //    {
        //        Id = -6,
        //        Name = "Öncü Döner",
        //        Address = "Bu restoranın adres bilgileri MirtitOrder'da bulunmaktadır",
        //        PhoneNumber = "(0442) 233 05 05",
        //        WorkingHours = "10:30 - 02:00",
        //        Rating = 3.8,
        //        Image = "https://images.deliveryhero.io/image/fd-tr/tr-logos/co9zi-logo.jpg",
        //        Meals = new List<Meal>()
        //        {


        //        },
        //        Email = "oncudoner@gmail.com",
        //        Password =  "Oncudoner1234!",
        //    },
        //    new Restaurant()
        //    {
        //        Id = -7,
        //        Name = "Lapidis Pide",
        //        Address = "Taşmağazalar Cd. No: 16",
        //        PhoneNumber = "(0442) 215 48 25",
        //        WorkingHours = "11:00 - 22:00",
        //        Rating = 4.4,
        //        Image = "https://images.deliveryhero.io/image/fd-tr/LH/gp7a-listing.jpg",
        //        Meals = new List<Meal>()
        //        {


        //        },
        //        Email = "lapidispide@gmail.com",
        //        Password ="Lapidispide1234!",
        //    },
        //    new Restaurant()
        //    {
        //        Id = -8,
        //        Name = "Usta Dönerci",
        //        Address = "Bu restoranın adres bilgileri MirtitOrder'da bulunmaktadır",
        //        PhoneNumber = "(0442) 343 99 99",
        //        WorkingHours = "11:00 - 22:00",
        //        Rating = 3.2,
        //        Image = "https://images.deliveryhero.io/image/fd-tr/tr-logos/cj7mp-logo.jpg",
        //        Meals = new List<Meal>()
        //        {


        //        },
        //        Email = "ustadonerci@gmail.com",
        //        Password = "Ustadonerci1234!",
        //    }
        //    );
        //modelBuilder.Entity<Meal>().HasData(
        //    new Meal() { Id = -26, Name = "Tavuk Döner Dürüm (80 gr.)", Price = 110.42M, Description = "80 gr. tavuk döner, patates kızartması, turşu, özel Öncü sos, sarımsaklı mayonez", IsAvailable = true, RestaurantId = -6, Image = "https://images.deliveryhero.io/image/fd-tr/Products/61470322.jpg??width=500", Quantity = 1 },
        //    new Meal() { Id = -27, Name = "Kazandıran Menü", Price = 229.49M, Description = "2 Adet Tavuk Döner Dürüm (80 gr.) + 2 Adet Patates Kızartması + İçecek (1 L.)", IsAvailable = true, RestaurantId = -6, Image = "https://images.deliveryhero.io/image/fd-tr/Products/30279841.jpg??width=500", Quantity = 1 },
        //    new Meal() { Id = -28, Name = "Öncü Zurna Tavuk Döner Dürüm (120 gr.)", Price = 144.42M, Description = "120 gr. tavuk döner, patates kızartması, turşu, özel Öncü sos, sarımsaklı mayonez", IsAvailable = true, RestaurantId = -6, Image = "https://images.deliveryhero.io/image/fd-tr/Products/61470323.jpg??width=500", Quantity = 1 },
        //    new Meal() { Id = -29, Name = "Avantajlı Tavuk Döner Dürüm Menü", Price = 174.24M, Description = "Tavuk Döner Dürüm (80 gr.) + Patates Kızartması + Sütaş Ayran (27.5 cl.)", IsAvailable = true, RestaurantId = -6, Image = "https://images.deliveryhero.io/image/fd-tr/Products/61470317.jpg??width=500", Quantity = 1 },
        //    new Meal() { Id = -30, Name = "Patates Kızartması (100 gr.)", Price = 42.41M, Description = "100 gr.", IsAvailable = true, RestaurantId = -6, Image = "https://images.deliveryhero.io/image/fd-tr/Products/61470328.jpg??width=500", Quantity = 1 },
        //    new Meal() { Id = -31, Name = "İkili Menü", Price = 305.91M, Description = "2 Adet Tavuk Döner Dürüm (80 gr.) + Patates Kızartması + içecek (1 L.)", IsAvailable = true, RestaurantId = -6, Image = "https://images.deliveryhero.io/image/fd-tr/Products/61482869.jpg??width=500", Quantity = 1 },
        //    new Meal() { Id = -32, Name = "Combo Tavuk Döner Dürüm Menü", Price = 178.42M, Description = "Tavuk Döner Dürüm (80 gr.) + Patates Kızartması + İçecek (33 cl.)", IsAvailable = true, RestaurantId = -6, Image = "https://images.deliveryhero.io/image/fd-tr/Products/61470321.jpg??width=500", Quantity = 1 },
        //    new Meal() { Id = -33, Name = "Combo Öncü Zurna Tavuk Döner Dürüm Menü", Price = 212.42M, Description = "Öncü Zurna Tavuk Döner Dürüm (120 gr.) + Patates Kızartması + İçecek (33 cl.)", IsAvailable = true, RestaurantId = -6, Image = "https://images.deliveryhero.io/image/fd-tr/Products/61471097.jpg??width=500", Quantity = 1 },
        //    new Meal() { Id = -34, Name = "Kuşbaşılı Kaşarlı Pide", Price = 260M, Description = "Kuşbaşı et, kaşar peyniri", IsAvailable = true, RestaurantId = -7, Image = "https://images.deliveryhero.io/image/fd-tr/Products/61276437.jpg??width=500", Quantity = 1 },
        //    new Meal() { Id = -35, Name = "Lahmacun", Price = 80M, Description = "Salata, sumak, meze ile", IsAvailable = true, RestaurantId = -7, Image = "https://images.deliveryhero.io/image/fd-tr/Products/9802942.jpg??width=500", Quantity = 1 },
        //    new Meal() { Id = -36, Name = "Kapalı Kıymalı Pide", Price = 210M, Description = "Kıyma, soğan", IsAvailable = true, RestaurantId = -7, Image = "https://images.deliveryhero.io/image/fd-tr/Products/61276434.jpg??width=500", Quantity = 1 },
        //    new Meal() { Id = -37, Name = "Kavurmalı Pide (Kapalı)", Price = 290M, Description = "Özel kavurmalı", IsAvailable = true, RestaurantId = -7, Image = "https://images.deliveryhero.io/image/fd-tr/Products/61276439.jpg??width=500", Quantity = 1 },
        //    new Meal() { Id = -38, Name = "Açık Lapidis Özel Karışık", Price = 270M, Description = "Kıyma, kavurma, kaşar peyniri, salam, sucuk, sosis", IsAvailable = true, RestaurantId = -7, Image = "https://images.deliveryhero.io/image/fd-tr/Products/65580785.jpg??width=500", Quantity = 1 },
        //    new Meal() { Id = -39, Name = "Kuşbaşılı Pide", Price = 260M, Description = "Günlük, taze, sinirsiz et", IsAvailable = true, RestaurantId = -7, Image = "https://images.deliveryhero.io/image/fd-tr/Products/61276436.jpg??width=500", Quantity = 1 },
        //    new Meal() { Id = -40, Name = "Sucuklu Kaşarlı Pide", Price = 240M, Description = "Dana sucuk, taze kaşar peyniri", IsAvailable = true, RestaurantId = -7, Image = "https://images.deliveryhero.io/image/fd-tr/Products/61276438.jpg??width=500", Quantity = 1 },
        //    new Meal() { Id = -41, Name = "Döner Pide", Price = 170M, Description = "Tavuk döner, kaşar peyniri", IsAvailable = true, RestaurantId = -7, Image = "https://images.deliveryhero.io/image/fd-tr/Products/66786044.jpg??width=500", Quantity = 1 },
        //    new Meal() { Id = -42, Name = "3'lü Baget Et Döner Menü", Price = 360M, Description = "3 Adet Baget Et Döner + Patates Kızartması (Büyük) + Soğan Halkası (6’lı) + İçecek (1 L.)", IsAvailable = true, RestaurantId = -8, Image = "https://images.deliveryhero.io/image/fd-tr/Products/37095618.jpg??width=500", Quantity = 1 },
        //    new Meal() { Id = -43, Name = "UD® Et İskender", Price = 255M, Description = "İskender sosu, İskender tereyağı, sivri biber, domates, yoğurt", IsAvailable = true, RestaurantId = -8, Image = "https://images.deliveryhero.io/image/fd-tr/Products/37095662.jpg??width=500", Quantity = 1 },
        //    new Meal() { Id = -44, Name = "2’li Tombik Et Döner Menü", Price = 340M, Description = "2 Adet Tombik Et Döner + Patates Kızartması (Orta) + (İçecek 1 L.)", IsAvailable = true, RestaurantId = -8, Image = "https://images.deliveryhero.io/image/fd-tr/Products/37095624.jpg??width=500", Quantity = 1 },
        //    new Meal() { Id = -45, Name = "2'li Baget Tavuk Döner Menü", Price = 185M, Description = "2 Adet Baget Tavuk Döner + Patates Kızartması (Orta) + Ayran (20 cl.)", IsAvailable = true, RestaurantId = -8, Image = "https://images.deliveryhero.io/image/fd-tr/Products/12742732.jpg??width=500", Quantity = 1 },
        //    new Meal() { Id = -46, Name = "Dürüm Et Döner Menü", Price = 240M, Description = "Dürüm Et Döner + Patates Kızartması (Orta) + Ayran (20 cl.)", IsAvailable = true, RestaurantId = -8, Image = "https://images.deliveryhero.io/image/fd-tr/Products/37095643.jpg??width=500", Quantity = 1 },
        //    new Meal() { Id = -47, Name = "Dürüm Tavuk Döner Menü", Price = 190M, Description = "Dürüm Tavuk Döner + Patates Kızartması (Orta) + Ayran (20 cl.)", IsAvailable = true, RestaurantId = -8, Image = "https://images.deliveryhero.io/image/fd-tr/Products/37095642.jpg??width=500", Quantity = 1 },
        //    new Meal() { Id = -48, Name = "Coca-Cola Fırsatı (Seçmeli Baget Menü)", Price = 260M, Description = "3 Adet Baget Tavuk Döner + Patates Kızartması (Büyük) + Soğan Halkası (6’lı) + Coca-Cola (1 L.)", IsAvailable = true, RestaurantId = -8, Image = "https://images.deliveryhero.io/image/fd-tr/Products/37095608.jpg??width=500", Quantity = 1 },
        //    new Meal() { Id = -49, Name = "Coca-Cola Fırsatı (3'lü Dürüm Menü)", Price = 440M, Description = "3 Adet Dürüm Tavuk Döner + Patates Kızartması (Büyük) + Soğan Halkası (6'lı) + Coca-Cola (1 L.)", IsAvailable = true, RestaurantId = -8, Image = "https://images.deliveryhero.io/image/fd-tr/Products/37095605.jpg??width=500", Quantity = 1 }



        //    );
        //modelBuilder.Entity<Meal>().HasData(
        //    new Meal()
        //    {
        //        Id = -50,
        //        Name = "Maytako (Tavuk Dönerden) 2 Adet",
        //        Price = 190M,
        //        Description = "2 Adet Maytako (50 gr. tavuk döner, tortilla lavaş, chedar peyniri, salatalık turşusu, patates kızartması, soğan, acılı sarımsaklı mayonez sos. Cin biberi ile)",
        //        IsAvailable = true,
        //        RestaurantId = -1,
        //        Image = "https://images.deliveryhero.io/image/fd-tr/Products/10775561.jpg??width=500"
        //    },
        //    new Meal()
        //    {
        //        Id = -51,
        //        Name = "Maytako (Et Dönerden) 2 Adet",
        //        Price = 280M,
        //        Description = "2 Adet Maytako (50 gr. et döner, tortilla lavaş, chedar peyniri, salatalık turşusu, patates kızartması, soğan, acılı sarımsaklı mayonez sos. Cin biberi ile)",
        //        IsAvailable = true,
        //        RestaurantId = -1,
        //        Image = "https://images.deliveryhero.io/image/fd-tr/Products/10775562.jpg??width=500"
        //    },
        //    new Meal()
        //    {
        //        Id = -52,
        //        Name = "Maytako Parti (Tavuk Dönerden)",
        //        Price = 520M,
        //        Description = "4 Adet Maytako Tavuk Döner + 2'li Külah Patates Kızartması + İçecek (1 L.)",
        //        IsAvailable = true,
        //        RestaurantId = -1,
        //        Image = "https://images.deliveryhero.io/image/fd-tr/Products/10775565.jpg??width=500"
        //    },
        //    new Meal()
        //    {
        //        Id = -53,
        //        Name = "Big Bang Burger",
        //        Price = 333.90M,
        //        Description = "Ev yapımı hamburger ekmeği, 2 x 70 ev yapım hamburger köftesi, kaşar peyniri (2 adet), cheddar peyniri (2 adet), ortası et soslu lokum bonfile, Musqa sos. Patates kızartması ile",
        //        IsAvailable = true,
        //        RestaurantId = -2,
        //        Image = "https://images.deliveryhero.io/image/fd-tr/Products/26725832.jpg??width=500"
        //    },
        //    new Meal()
        //    {
        //        Id = -54,
        //        Name = "Musqover Burger",
        //        Price = 318.90M,
        //        Description = "Ev yapımı hamburger ekmeği, ev yapımı hamburger köftesi, sarımsaklı mayonez, ince dilim bonfile, domates, soğan, cheddar peyniri. Patates kızartması ile",
        //        IsAvailable = true,
        //        RestaurantId = -2,
        //        Image = "https://images.deliveryhero.io/image/fd-tr/Products/26726101.jpg??width=500"
        //    },
        //    new Meal()
        //    {
        //        Id = -55,
        //        Name = "Chef - X Burger",
        //        Price = 377.90M,
        //        Description = "Ev yapımı hamburger ekmeği, ev yapımı 3 x 70 gr. hamburger köftesi, Musqa sos, közlenmiş biber, 3 adet cheddar peyniri, double dana jambon, karamelize soğan. Patates kızartması ile",
        //        IsAvailable = true,
        //        RestaurantId = -2,
        //        Image = "https://images.deliveryhero.io/image/fd-tr/Products/26726741.jpg??width=500"
        //    },
        //    new Meal()
        //    {
        //        Id = -56,
        //        Name = "Algida Menü (Algida’lı Dev Menü)",
        //        Price = 600M,
        //        Description = "Big King® + Whopper® Jr. + Chicken Royale® + Tavuklu Barbekü Deluxe Burger + Patates Kızartması (Büyük) + Soğan Halkası (8'li) + Algida Frigola (570 ml.) yada Algida Maraş Usulü Sade Dondurma (500 ml.) + 1 L. İçecek",
        //        IsAvailable = true,
        //        RestaurantId = -3,
        //        Image = "https://images.deliveryhero.io/image/fd-tr/Products/42100301.jpg??width=500"
        //    },
        //    new Meal()
        //    {
        //        Id = -57,
        //        Name = "Tek Kişilik Algida Menü",
        //        Price = 235M,
        //        Description = "Double Cheese Burger + Patates Kızartması (Orta) + Seçeceğin Algida Max + Kutu İçecek",
        //        IsAvailable = true,
        //        RestaurantId = -3,
        //        Image = "https://images.deliveryhero.io/image/fd-tr/Products/28459724.jpg??width=500"
        //    },
        //    new Meal()
        //    {
        //        Id = -58,
        //        Name = "Benim Üçlüm",
        //        Price = 295M,
        //        Description = "3 Adet Seçeceğiniz Sandviç + Patates Kızartması (Büyük) + 1 L. İçecek",
        //        IsAvailable = true,
        //        RestaurantId = -3,
        //        Image = "https://images.deliveryhero.io/image/fd-tr/Products/27388702.jpg??width=500"
        //    },
        //    new Meal()
        //    {
        //        Id = -59,
        //        Name = "Mozzarella Sticks (4 Adet)",
        //        Price = 65M,
        //        Description = "4 adet",
        //        IsAvailable = true,
        //        RestaurantId = -4,
        //        Image = "https://images.deliveryhero.io/image/fd-tr/Products/6492540.jpg??width=500"
        //    },
        //    new Meal()
        //    {
        //        Id = -60,
        //        Name = "Cheff Box",
        //        Price = 199M,
        //        Description = "250 gr. patates kızartması, 4 adet nugget, 4 adet soğan halkası, 4 adet mozzarella sticks",
        //        IsAvailable = true,
        //        RestaurantId = -4,
        //        Image = "https://images.deliveryhero.io/image/fd-tr/Products/35603055.jpg??width=500"
        //    },
        //    new Meal()
        //    {
        //        Id = -61,
        //        Name = "Cheff Chicken Pizza (Büyük Boy)",
        //        Price = 275M,
        //        Description = "Tavuk, mozzarella peyniri, pizza sosu",
        //        IsAvailable = true,
        //        RestaurantId = -4,
        //        Image = "https://images.deliveryhero.io/image/fd-tr/Products/6492527.jpg??width=500"
        //    },
        //    new Meal()
        //    {
        //        Id = -62,
        //        Name = "Algida Menüsü (Algida Keyif)",
        //        Price = 270M,
        //        Description = "Popchicken + Büyük Boy Patates + 4’lü Soğan Halkası + 4’lü Nuggets + Magnum Mini Badem + Kutu İçecek",
        //        IsAvailable = true,
        //        RestaurantId = -5,
        //        Image = "https://images.deliveryhero.io/image/fd-tr/Products/68486480.jpg??width=500"
        //    },
        //    new Meal()
        //    {
        //        Id = -63,
        //        Name = "Ekonomix Menü",
        //        Price = 220M,
        //        Description = "2 Adet Tavukburger® + Patates Kızartması (Orta) + 1L. İçecek",
        //        IsAvailable = true,
        //        RestaurantId = -5,
        //        Image = "https://images.deliveryhero.io/image/fd-tr/Products/28499447.jpg??width=500"
        //    },
        //    new Meal()
        //    {
        //        Id = -64,
        //        Name = "Beşiktaş Taraftar Menüsü",
        //        Price = 430M,
        //        Description = "3 adet Poppy Sandviç + Patates Kızartması (Büyük) + Nuggets (12’li) + Tenders (3'lü) + 1L. İçecek",
        //        IsAvailable = true,
        //        RestaurantId = -5,
        //        Image = "https://images.deliveryhero.io/image/fd-tr/Products/65679780.jpg??width=500"
        //    }
        //    );


        //base.OnModelCreating(modelBuilder);
        OnModelCreatingPartial(modelBuilder);

    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    
}
