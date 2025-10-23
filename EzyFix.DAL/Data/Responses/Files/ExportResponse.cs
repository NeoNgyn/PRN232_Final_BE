namespace EzyFix.DAL.Data.Responses.Files
{
    public class ExportResponse
    {
        
        public byte[] FileContent { get; set; }
        
        public string ContentType { get; set; }
               
        public string FileName { get; set; }
    }
}