using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
public class BookModel
{
    [Key]
    public int Id {set; get;}
    [DisplayName("Tiêu đề"), Required]
    public string Title {set; get;}
    [DisplayName("Tác giả"), Required]
    public string Author {set; get;}
    [DisplayName("Nhà xuất bản"), Required]
    public string Publisher {set; get;}
    [DisplayName("Năm xuất bản"), Required]
    // [DataType(DataType.Date)]
    public DateTime PublishYear {set; get;}
    [DisplayName("Tóm tắt")]
    // [DataType(DataType.MultilineText)]
    public string? Description { get; set; }
    [DisplayName("File")]
    public string? DataFile { get; set; }
}