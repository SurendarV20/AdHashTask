namespace EBay.Dto
{
    public class ScrappedPartInfo
    {
        public List<string> PartNumbers { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public List<string> Images { get; set; } = new List<string>();
    }
}
