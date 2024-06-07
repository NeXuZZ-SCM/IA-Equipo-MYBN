using System.Text.Json.Serialization;

namespace API.Model
{
    /// <summary>
    /// Permite establecer la relacion entre la comprar de dos producto
    /// Se compra un primer producto, y si luego se obtienen un segundo 
    /// Se genera un ralacion de desicion del usuario
    /// </summary>
    public class ProductCoPurchase
    {
        //[JsonIgnore]
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int CoPurchaseProductId { get; set; }
        public DateTime Date { get; set; }
    }
}
