using EduGame.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace EduGame.Data
{
    public class AppDbContext : DbContext
    {
        // 1. Yapıcı Metot (Constructor)
        // API katmanından gelen ayarları (veritabanı şifresi vs.) buraya alır.
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // 2. Tabloların Tanımlanması (DbSet)
        // Core katmanındaki sınıflar burada veritabanı tablosuna dönüşür.
        public DbSet<User> Users { get; set; }
        public DbSet<GameContent> GameContents { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<ContentAttachment> ContentAttachments { get; set; }

        // 3. Tablo Ayarları ve İlişkiler (Fluent API)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // --- User Ayarları ---
            // Kullanıcı silinirse, oluşturduğu içerikler de silinsin mi?
            // Genelde kullanıcıyı silmeyiz (Soft Delete yaparız) ama teknik ilişkiyi kuralım.
            modelBuilder.Entity<User>()
                .HasMany(u => u.Contents)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade); // Kullanıcı silinirse içerikleri de gider.

            // --- GameContent Ayarları (Burası Çok Önemli) ---
            
            // Bir İçerik (GameContent) silinirse, ona bağlı SORULAR da silinsin.
            modelBuilder.Entity<GameContent>()
                .HasMany(c => c.Questions)
                .WithOne(q => q.GameContent)
                .HasForeignKey(q => q.GameContentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Bir İçerik silinirse, SOHBET GEÇMİŞİ de silinsin.
            modelBuilder.Entity<GameContent>()
                .HasMany(c => c.ChatHistory)
                .WithOne(m => m.GameContent)
                .HasForeignKey(m => m.GameContentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Bir İçerik silinirse, EKLENEN DOSYALAR (Attachments) da silinsin.
            modelBuilder.Entity<GameContent>()
                .HasMany(c => c.Attachments)
                .WithOne(a => a.GameContent)
                .HasForeignKey(a => a.GameContentId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(modelBuilder);
        }
    }
}