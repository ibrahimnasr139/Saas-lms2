using System.Net;

namespace Domain.Errors
{
    public static class FileError
    {
        public static Error UploadFailed =>
            new Error("UploadFailed", "فشل رفع الملف على خادم التخزين", HttpStatusCode.BadRequest);

        public static Error NotFound =>
            new Error("FileNotFound", "الملف غير موجود أو تم حذفه مسبقاً", HttpStatusCode.NotFound);
    }
}
