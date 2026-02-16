namespace EduGame.Core.Entities
{
    // İçeriğin hangi aşamada olduğunu takip eder
    public enum ContentStatus
    {
        Draft = 0,              // Yüklendi ama işlem yapılmadı
        Processing = 1,         // AI şu an okuyor/işliyor
        WaitingApproval = 2,    // AI özet çıkardı, kullanıcının onayı bekleniyor
        Approved = 3,           // Kullanıcı onayladı, oyun oynanabilir
        Archived = 4            // Silindi veya arşivlendi
    }

    // İçeriğin kaynağı nedir?
    public enum ContentSourceType
    {
        FileUpload = 0,   // PDF, Word vb. dosya
        ImageUpload = 1,  // Defter fotoğrafı (OCR yapılacak)
        TextPrompt = 2,   // Kullanıcı "Bana tarihi anlat" yazdı
        ManualEntry = 3   // Kopyala-Yapıştır metin
    }
}