namespace KooliProjekt.Data
{
    public class Product
    {
        public int Id {get; set;}
        public string Brand {get; set;}
        public string Model {get; set;}
        public string Manufacturer {get; set;}
        public string CarNum {get; set;}
        public string CarName {
            get { return Brand + " " + Model + " " + CarNum; }
        }
        public CarType CarType {get;set;}
        public decimal DistancePrice {get;set;}
        public decimal TimePrice {get;set;}
        
        public string? ImageId {get;set;}
        public Image Image {get;set;}

        //Taken By
    //    public int UserId {get; set;}
    }
}