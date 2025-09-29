namespace AOC.Entities
{
    public class webReviews
    {
        public int idReview { get; set; }
        public int idCliente { get; set; }
        public int idProducto { get; set; }
        public DateTime fecha { get; set; }
        public string comentario { get; set; }
        public int rating { get; set; }
    }
}
