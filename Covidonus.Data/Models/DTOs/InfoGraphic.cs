namespace Covidonus.Data.Models.DTOs
{
    public class InfoGraphic
    {
        public string Title
        {
            get;
            set;
        }

        public string Description
        {
            get;
            set;
        }

        public string Url
        {
            get;
            set;
        }

        public string UrlToImage
        {
            get;
            set;
        }
        public int Order { get; set; }
        public string Type { get; set; }
    }
}