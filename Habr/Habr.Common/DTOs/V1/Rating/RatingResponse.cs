namespace Habr.Common.DTOs.V1.Rating;

public class RatingResponse
{
    public int Id { get; set; }
    public int Value { get; set; }
    public int PostId { get; set; }
    public int UserId { get; set; }
}