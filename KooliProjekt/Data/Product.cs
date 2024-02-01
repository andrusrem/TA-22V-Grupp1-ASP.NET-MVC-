using System.Diagnostics.CodeAnalysis;

namespace KooliProjekt.Data
{
    [ExcludeFromCodeCoverage]
    public class Product : Entity
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
        
        public decimal EstimatedPrice 
        {
            get { return TimePrice + (DistancePrice * 100); }
        }
        public string? ImageId {get;set;}
        public Image Image {get;set;}

        //Taken By
    //    public int UserId {get; set;}
    }
}