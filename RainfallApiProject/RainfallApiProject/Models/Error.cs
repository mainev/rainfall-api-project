namespace RainfallApiProject.Models
{
    public class Error
    {
        public string Message { get; set; }
        public List<ErrorDetail> Detail { get; set;} = new List<ErrorDetail>();
    }
}
